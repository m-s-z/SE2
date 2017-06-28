using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Player;
using System.Net;
using System.Text.RegularExpressions;
using Messages.CommunicationServer;

namespace CommunicationServer
{
    public class Connection : Messages.CommunicationServer.IConnection
    {
        protected bool _isStarted = false;
        protected TcpListener _listener;
        protected System.Net.IPAddress _ipAddress;
        protected List<IReceiver> _receivers = new List<IReceiver>();
        protected Task _clientCollector;
        protected int _port;
        protected ulong _playerCount = 0;
        protected object _playerCountLock = new object();
        protected ulong _gameCount = 0;
        protected object _gameCountLock = new object();
        protected Dictionary<ulong, IGameState> _games = new Dictionary<ulong, IGameState>();
        //Public

        public List<IReceiver> Receivers
        {
            get { return _receivers; }
            set { _receivers = value; }
        }
        public Dictionary<ulong, IGameState> Games
        {
            get { return _games; }
            set { _games = value; }
        }
        public ulong PlayerCount
        {
            get { return _playerCount; }
            set { _playerCount = value; }
        }

        public object PlayerCountLock
        {
            get { return _playerCountLock; }
        }
        public ulong GameCount
        {
            get { return _gameCount; }
            set { _gameCount = value; }
        }
        public object GameCountLock
        {
            get { return _gameCountLock; }
        }
       
        public Connection(int port)
        {
            _port = port;
        }

        public void StartServer()
        {
            if (!_isStarted)
            {
                Console.WriteLine("[Server] Server started!");

                _playerCount = 0;
                _gameCount = 0;
                _ipAddress = System.Net.IPAddress.Any;
                _listener = new TcpListener(_ipAddress, _port);
                _listener.Start();
                _isStarted = true;

                _clientCollector = new Task(() => Collect());
                _clientCollector.Start();

                WaitForConnection();
            }
        }

        public void StopServer()
        {
            if (_isStarted)
            {
                _isStarted = false;
                _listener.Stop();
                foreach (var rec in _receivers)
                {
                    rec.Status = false;
                }
                Task.WaitAll();
                Console.WriteLine("[Server] Server stopped!");
            }
        }

        public string GetEndpoint()
        {
            string externalIP = GetLocalIPAddress();
            return externalIP + ":" + _port.ToString() + " ";
        }

        //Private Methods
        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void ConnectionHandler(IAsyncResult ar)
        {
            try
            {
                if (_isStarted)
                {
                    lock (_receivers)
                    {
                        var newClient = new Receiver(new TcpClientAdapter(_listener?.EndAcceptTcpClient(ar)), this);
                        newClient.Start();
                        _receivers.Add(newClient);
                        Console.WriteLine("[Server] Client connected! ");
                    }

                    WaitForConnection();
                }
            }
            catch(SocketException e)
            {
                Console.WriteLine("Accepting clients finished");
            }
            
        }

        private void WaitForConnection()
        {
            _listener.BeginAcceptTcpClient(new AsyncCallback(ConnectionHandler), null);
        }

        private void Collect()
        {
            while(_isStarted)
            {
                int receiversCount = _receivers.Count;
                for ( int i = receiversCount-1; i >= 0; i-- )
                {
                    if (_receivers[i].Status == false)
                    {
                        ICommunicationServerHandler msg = null;
                        if(_receivers[i].IsGameMaster)
                        {
                            msg = new Xsd2.GameMasterDisconnected(_receivers[i].GameId);
                        }
                        else
                        {
                            msg = new Xsd2.PlayerDisconnected(_receivers[i].Id);
                        }
                        msg.HandleOnCommunicationServer(this, _receivers[i]);
                    }

                }
                //Timers should be checked every second
                Thread.Sleep(1000);
            }
        }

        public IGameState CreateGame(IReceiver gameMaster, Xsd2.RegisterGame msg)
        {
            var gameWithSameName = _games.FirstOrDefault(g => g.Value.GameName == msg.NewGameInfo.gameName).Value;
            if(gameWithSameName != null)
            {
                return null;
            }
            ulong gameId = 0;
            lock (_gameCountLock)
            {
                _gameCount++;
                gameId = _gameCount;
            }
            gameMaster.IsGameMaster = true;
            gameMaster.GameId = gameId;

            var game = new GameState(gameMaster, gameId, msg);
            _games.Add(gameId, game);

            return game;
        }
    }
}
