using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Serialization;
using Messages.PlayerInterfaces;
using Messages.XmlHandling;
using Messages.MessageParsing;

namespace Player
{
    public class Connection : IConnection
    {
        protected MessageParser msgParser = new MessageParser();
        protected string msg = "";
        private const int DISCONNECTTIMERINTERVAL = 300000;
        private int ARBITRATYKEEPALIVEINTERVAL = 1000;
        private uint _retryJoinGameInterval;
        private int _keepAliveInterval;
        protected System.Timers.Timer _timer;
        protected string _address = "127.0.0.1";
        protected int _port = 8000;
        protected ITcpClient _tcpClient;
        protected Guid _guid;
        protected ulong _gameId;
        protected List<string> _messageList = new List<string>();
        protected string _gameName;
        protected bool _gameFinished = false;
        /*  
         *  <summary>
         * there are 3 differeny stages
         * 0 - disconnected
         * 1 - connected and ready to send a message
         * 2 - connected and waiting for an answer from game master
         * </summary>
         */
        protected Xsd2.TeamColour _team;
        protected Xsd2.PlayerType _role;
        protected int _status = 0;
        protected Task _receivingThread, _sendingThread, _keepAliveThread;
        protected XmlRootReader _xmlRootReader = new XmlRootReader();
        protected IPlayerLogic _logic;
        public List<string> MessageList
        {
            get { return _messageList; }
        }
        public bool GameFinished
        {
            get { return _gameFinished; }
            set { _gameFinished = value; }
        }
        public string GameName
        {
            get { return _gameName; }
            set { _gameName = value; }
        }
        public Guid Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }
        public ulong GameId
        {
            get { return _gameId; }
            set { _gameId = value; }
        }
        public Xsd2.TeamColour Team
        {
            get { return _team; }
            set { _team = value; }
        }
        public Xsd2.PlayerType Role
        {
            get { return _role; }
            set { _role = value; }
        }
        public IPlayerLogic Logic
        {
            get { return _logic; }
            set { _logic = value; }
        }
        public Connection(string ipAddress, int port, string gameName, Messages.ParametersReader inputParameters)
        {
            _logic = new PlayerLogic(this);
            _address = ipAddress;
            _port = port;
            _gameName = gameName;
            if (inputParameters != null)
                _keepAliveInterval = (int)inputParameters.ReadPlayerSettings.KeepAliveInterval;
            else _keepAliveInterval = ARBITRATYKEEPALIVEINTERVAL;
            _retryJoinGameInterval = inputParameters.ReadPlayerSettings.RetryJoinGameInterval;
            Enum.TryParse(inputParameters.Team, out _team);
            Enum.TryParse(inputParameters.Role, out _role);
        }
        public Connection(string ipAddress, int port, string gameName, string[] args, object testPlaceholder)
        {
            _logic = new PlayerLogic(this);
            _address = ipAddress;
            _port = port;
            _gameName = gameName;
            
            _keepAliveInterval = ARBITRATYKEEPALIVEINTERVAL;;
            uint.TryParse(args[0], out _retryJoinGameInterval);
            Enum.TryParse(args[1], out _team);
            Enum.TryParse(args[2], out _role);
        }
        
        public void Connect()
        {
            _tcpClient = new TcpClientAdapter(new TcpClient());
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
            _status = 1;
            _tcpClient.ReceiveBufferSize = 1024;
            _tcpClient.SendBufferSize = 1024;
            _timer = new System.Timers.Timer();
            _timer.Interval = DISCONNECTTIMERINTERVAL; //the timer is set to 5000ms (5s)
            _timer.Elapsed += new ElapsedEventHandler(this.timerHandler);
            InitiateThreads();
        }

        public void Disconnect()
        {
            Console.WriteLine("Player disconnected");
            _status = 0;
            Task.WaitAll();
            if(_tcpClient.Client.Connected)
            {
                _tcpClient.Client.Disconnect(false);
            }
            
            _tcpClient.Close();
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

        public void SendMessage(string message)
        {
            if (message.Length != 1)
            {
                message += (char)23;
            }
            _messageList.Add(message);
        }

        private void Receive()
        {
            while(_status != 0 )
            {
                if (_tcpClient.Available > 0)
                {
                    try
                    {
                        byte[] arr = new byte[2048];
                        //string msg = binFormatter.Deserialize(_tcpClient.GetStream()) as string;
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
                            IPlayerHandler message = null;
                            using (var stream = new StringReader(currentMsg))
                            {
                                message = (IPlayerHandler)serializer.Deserialize(stream);
                            }
                            message.HandleOnPlayer(this);
                        }
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("Lost connection");
                    }
                }
                Thread.Sleep(30);
            }
        }

        private void Send()
        {
            while(_status != 0)
            {
                if(_messageList.Count > 0)
                {
                    string m = _messageList[0];
                    byte[] arr = Encoding.UTF8.GetBytes(m);

                    try
                    {
                        _tcpClient.GetStream().Write(arr, 0, arr.Length);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("{0} has disconnected while serializing message {1}", nameof(Connect), m);
                        //Disconnect();
                    }
                    _messageList.Remove(m);
                    _status = 2;
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
            while (_status != 0)
            {
                Thread.Sleep(_keepAliveInterval);
                try
                {
                    byte[] tmp = new byte[1];
                    tmp[0] = (byte)(char)23;
                    SendMessage(char.ToString((char)23));
                    //server.Send(tmp, 0, 0);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Keep alive error");
                    // 10035 == WSAEWOULDBLOCK
                    if (!e.NativeErrorCode.Equals(10035))
                    {
                        _status = 0;
                        //Disconnect();
                    }
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
