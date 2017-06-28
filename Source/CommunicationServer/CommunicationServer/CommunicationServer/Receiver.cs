using Messages;
using Player;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Messages.CommunicationServer;
using System.Timers;
using Messages.XmlHandling;
using Messages.MessageParsing;
namespace CommunicationServer
{
    /*naaajs*/
    public class Receiver : Messages.CommunicationServer.IReceiver
    {
        protected MessageParser msgParser = new MessageParser();
        protected string msg = "";
        protected System.Timers.Timer _timer;
        private const int DISCONNECTTIMERINTERVAL = 300000;
        protected ITcpClient _client;
        protected Connection _server;
        protected Task _receivingThread, _sendingThread;
        protected List<string> _messageList;
        protected bool _status = false;  //conncection status true -> connected. 
        protected Guid _guid;
        protected bool _isGameMaster;
        protected BinaryFormatter _formatter;
        protected XmlRootReader _xmlRootReader = new XmlRootReader();
        protected ulong _id;
        protected ulong _gameId;
        protected string _gameName;
        public ITcpClient GetClient()
        {
            return _client;
        }
        public bool IsGameMaster
        {
            get { return _isGameMaster; }
            set { _isGameMaster = value; }
        }
        public ulong Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public ulong GameId
        {
            get { return _gameId; }
            set { _gameId = value; }
        }
        public string GameName
        {
            get { return _gameName; }
            set { _gameName = value; }
        }
        public List<string> MessageList
        {
            get { return _messageList; }
        }

        public bool Status
        {
            get { return _status; }
            set { _status = value; }
        }

        //Creating new class receving the messages from particular client conncected to the designated conncection server
        public Receiver(ITcpClient client, Connection server)
        {
            _guid = Guid.NewGuid();
            _messageList = new List<string>();
            _status = true;
            _server = server;
            _client = client;
            _client.ReceiveBufferSize = 1024;
            _client.SendBufferSize = 1024;
            _isGameMaster = false;
            _formatter = new BinaryFormatter();
            _timer = new System.Timers.Timer();
            _timer.Interval = DISCONNECTTIMERINTERVAL; //the timer is set to 5000ms (5s)
            _timer.Elapsed += new ElapsedEventHandler(this.timerHandler);
        }

        public void Start()
        {
            _receivingThread = new Task(() => Receive());
            _receivingThread.Start();

            _sendingThread = new Task(() => Send());
            _sendingThread.Start();

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
                        _client.GetStream().Write(arr, 0, arr.Length);
                    }
                    catch(SocketException ex)
                    {
                        Console.WriteLine("Lost connection to {0}", _guid);
                        //Disconnect();
                    }
                    finally
                    {
                        _messageList.Remove(m);
                    }
                }
                Thread.Sleep(30);
            }
        }

        // Message handling.
        private void Receive()
        {
            while (_status)
            {
                if (_client.Available > 0)
                {
                    try
                    {
                        //Extracting the root information from the message.
                        byte[] arr = new byte[2048];
                        _client.GetStream().Read(arr, 0, arr.Length);
                        msg += Encoding.UTF8.GetString(arr);
                        msg = msg.Replace("\0", string.Empty);
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("Lost connection to {0}", _guid);
                        _status = false;
                        //Disconnect();
                    }
                    if(Messages.KeepAlive.IsKeepAlive(msg))
                    {
                        ResetTimer();
                        msg = msg.Substring(1);
                        try
                        {
                            byte[] tmp = new byte[1];
                            tmp[0] = (byte)(char)23;
                            _messageList.Add(char.ToString((char)23));
                        }
                        catch (SocketException e)
                        {
                            Console.WriteLine("We encountered an exception");
                        }
                    }
                    else
                    {
                        int specialByteIndex;
                        string currentMsg = msgParser.Parse(ref msg, out specialByteIndex);
                        if(specialByteIndex == -1 || string.IsNullOrEmpty(currentMsg))
                        {
                            continue;
                        }
                        ResetTimer();
                        Console.WriteLine(currentMsg);
                        var type = _xmlRootReader.GetMessageType(currentMsg);
                        //Deserialization of the message
                        var serializer = new XmlSerializer(type);
                        Messages.CommunicationServer.ICommunicationServerHandler message = null;
                        using (var stream = new StringReader(currentMsg))
                        {
                            message = (Messages.CommunicationServer.ICommunicationServerHandler)serializer.Deserialize(stream);
                        }
                        message.HandleOnCommunicationServer(_server, this);                
                    }
                }
                Thread.Sleep(30);
            }
        }

        private void timerHandler(object sender, EventArgs e)
        {
            _timer.Stop();
            System.Console.WriteLine("[{0}]client with guid {1} disconnected", _guid, _guid);
            Disconnect();
        }

        public void Disconnect()
        {
            _status = false;
            _receivingThread?.Wait();
            _sendingThread?.Wait();
            _receivingThread = null;
            _sendingThread = null;
            if(_client.Client.Connected)
            {
                _client.Client.Disconnect(false);
            }
            
            _client.Close();
        }

        private void ResetTimer()
        {
            _timer.Stop();
            _timer.Start();
        }
    }
}