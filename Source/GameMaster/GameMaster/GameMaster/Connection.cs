using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Timers;
using Messages.GameMaster;
using Messages.XmlHandling;
using Messages.MessageParsing;

namespace GameMaster
{
    public class Connection : Messages.GameMaster.IConnection
    {
        //in miliseconds
        protected MessageParser msgParser = new MessageParser();
        protected string msg = "";
        private int _keepAliveInterval;
        private int DISCONNECTTIMERINTERVAL = 300000;
        private int ARBITRATYKEEPALIVEINTERVAL = 1000;
        protected System.Timers.Timer _timer;
        protected string _address = "127.0.0.1";
        protected int _port;
        protected TcpClient _tcpClient;
        protected bool _status = false;
        protected List<string> _messageList;
        protected Task _receivingThread, _sendingThread, _keepAliveThread;
        protected bool _gameRegistered = false;
        protected IGameState _gameState;
        protected XmlRootReader _xmlRootReader = new XmlRootReader();
        protected DelayGenerator _delay = new DelayGenerator();
        //Maximum number of players that can join
        public IGameState GameState { get { return _gameState; } set { _gameState = value; } }
        
               
        public List<string> MessageList
        {
            get { return _messageList; }
        }
        public Connection(string ipAddress, int port, Messages.ParametersReader inputParameters)
        {
            _gameState = new GameState(inputParameters);
            _messageList = new List<string>();
            _port = port;
            _address = ipAddress;
            if (inputParameters != null)
                _keepAliveInterval = (int)inputParameters.ReadGameMasterSettings.KeepAliveInterval;
            else
                _keepAliveInterval = ARBITRATYKEEPALIVEINTERVAL;
            _timer = new System.Timers.Timer();
            _timer.Interval = DISCONNECTTIMERINTERVAL; //the timer is set to 5000ms (5s)
            _timer.Elapsed += new ElapsedEventHandler(this.timerHandler);
        }
        public Connection(string ipAddress, int port, string[] parameters)
        {
            _gameState = new GameState(parameters);
            _messageList = new List<string>();
            _port = port;
            _address = ipAddress;
            _keepAliveInterval = ARBITRATYKEEPALIVEINTERVAL;
            _timer = new System.Timers.Timer();
            _timer.Interval = DISCONNECTTIMERINTERVAL; //the timer is set to 5000ms (5s)
            _timer.Elapsed += new ElapsedEventHandler(this.timerHandler);
        }
        public Connection(string ipAddress, int port, string[] parameters, object placeholder)
        {
            _gameState = new GameState(parameters);
            _messageList = new List<string>();
            _port = port;
            _address = ipAddress;
            _keepAliveInterval = ARBITRATYKEEPALIVEINTERVAL;
            _timer = new System.Timers.Timer();
            _timer.Interval = DISCONNECTTIMERINTERVAL; //the timer is set to 5000ms (5s)
            _timer.Elapsed += new ElapsedEventHandler(this.timerHandler);
        }

        public void Connect()
        {
            _tcpClient = new TcpClient();
            /*
             * tcpClient exception should be handled
             */
            try
            {
                _tcpClient.Connect(_address, _port);
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception has been encountered while connecting to {0} on port {1}. Exception message: {2}", _address, _port, e.Message);
                Console.WriteLine("{0} will now exit", nameof(Connection));
                throw new Exception(e.Message);
            }
            _status = true;
            _tcpClient.ReceiveBufferSize = 1024;
            _tcpClient.SendBufferSize = 1024;
            InitiateThreads();
        }

        public void InitiateThreads()
        {
            _receivingThread = new Task(() => Receive());
            _receivingThread.Start();

            _sendingThread = new Task(() => Send());
            _sendingThread.Start();

            _keepAliveThread = new Task(() => KeepAlive());
            _keepAliveThread.Start();

            _timer.Start();
        }

        public void Disconnect()
        {
            _messageList.Clear();
            Thread.Sleep(1000);
            Console.WriteLine("[Client] Client disconnected!");
            _status = false;
            Task.WaitAll();
            if (_tcpClient != null)
            {
                if(_tcpClient.Client.Connected)
                {
                    _tcpClient.Client.Disconnect(false);
                }
                
                _tcpClient.Close();
            }
        }
        public void SendMessage(string message)
        {
            if (message.Length != 1)
            {
                message += (char)23;
            }
            _messageList.Add(message);

        }

        public void AddPlayer(Guid guid, ulong id, Xsd2.TeamColour team, Xsd2.PlayerType type)
        {
            this.GameState.AddPlayer(new Player(guid, id, type, team));
        }

        public void DelayMessage (Messages.XmlHandling.Serializer serializer, uint delay )
        {
            _delay.DelayMessage(this, serializer, delay);
        }

        private void Send()
        {
            while (_status)
            {
                if (_messageList.Count > 0)
                {
                    string m = _messageList[0];
                    byte[] arr = Encoding.UTF8.GetBytes(m);
                    try
                    {
                        _tcpClient.GetStream().Write(arr, 0, arr.Length);
                    }
                    catch(SocketException ex)
                    {
                        Console.WriteLine("Lost Connection" + ex);
                        Console.WriteLine("Socket error");
                        //Disconnect();
                    }

                    _messageList.Remove(m);
                }

                Thread.Sleep(30);
            }
        }



        private void Receive()
        {
            while (_status)
            {
                try
                {
                    if (_tcpClient.Available > 0)
                    {
                        byte[] arr = new byte[2048];
                        _tcpClient.GetStream().Read(arr, 0, arr.Length);
                        msg += Encoding.UTF8.GetString(arr);
                        msg = msg.Replace("\0", string.Empty);

                        if (Messages.KeepAlive.IsKeepAlive(msg))
                        {
                            msg = msg.Substring(1);
                            ResetTimer();
                        }
                        else
                        {
                            int specialByteIndex;
                            string currentMsg = msgParser.Parse(ref msg, out specialByteIndex);
                            if (specialByteIndex == -1 || string.IsNullOrEmpty(currentMsg))
                            {
                                continue;
                            }
                            ResetTimer();
                            Console.WriteLine(currentMsg);
                            var type = _xmlRootReader.GetMessageType(currentMsg);
                            //Deserialization of the message
                            var serializer = new XmlSerializer(type);
                            Messages.GameMaster.IGameMasterHandler message = null;
                            using (var stream = new StringReader(currentMsg))
                            {
                                message = (IGameMasterHandler)serializer.Deserialize(stream);
                            }
                            message.HandleOnGameMaster(this);
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Lost connection: " + e);
                    _status = false;
                    //Disconnect();
                }

                Thread.Sleep(30);
            }
        }

        private void timerHandler(object sender, EventArgs e)
        {
            _timer.Stop();
            Console.WriteLine("Lost connection");
            Disconnect();
        }
        private void KeepAlive()
        {
            Socket server = _tcpClient.Client;
            while (_status)
            {
                Thread.Sleep(_keepAliveInterval);
                try
                {
                    byte[] tmp = new byte[1];
                    tmp[0] = (byte)(char)23;
                    _messageList.Add(Char.ToString((char)23));
                    //server.Send(tmp, 0, 0);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Keep alive error");
                    // 10035 == WSAEWOULDBLOCK
                    if (!e.NativeErrorCode.Equals(10035))
                    {
                        Console.WriteLine("Lost Connection");
                        _status = false;
                        //Disconnect();
                    }
                }
                finally
                {
                }
            }
            
        }


        private void ResetTimer()
        {
            _timer.Stop();
            _timer.Start();
        }

       
    }
}
