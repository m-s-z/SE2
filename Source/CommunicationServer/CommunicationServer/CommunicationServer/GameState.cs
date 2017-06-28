using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages.CommunicationServer;

namespace CommunicationServer
{
    public class GameState : Messages.CommunicationServer.IGameState
    {
        private ulong _gameId;
        private IReceiver _gameMaster;
        private List<IReceiver> _players;
        private string _gameName;
        private ulong _redTeamMax;
        private ulong _redTeamCount;
        private ulong _blueTeamMax;
        private ulong _blueTeamCount;
        private object _redTeamLock = new object();
        private object _blueTeamLock = new object();
        public ulong GameId
        {
            get
            {
                return _gameId;
            }

            set
            {
                _gameId = value;
            }
        }

        public IReceiver GameMaster
        {
            get
            {
                return _gameMaster;
            }

            set
            {
                _gameMaster = value;
            }
        }

        public List<IReceiver> Players
        {
            get
            {
                return _players;
            }

            set
            {
                _players = value;
            }
        }

        public string GameName
        {
            get
            {
                return _gameName;
            }

            set
            {
                _gameName = value;
            }
        }

        public ulong RedTeamCount
        {
            get
            {
                return _redTeamCount;
            }

            set
            {
                _redTeamCount = value;
            }
        }

        public ulong RedTeamMax
        {
            get
            {
                return _redTeamMax;
            }

            set
            {
                _redTeamMax = value;
            }
        }

        public ulong BlueTeamCount
        {
            get
            {
                return _blueTeamCount;
            }

            set
            {
                _blueTeamCount = value;
            }
        }

        public ulong BlueTeamMax
        {
            get
            {
                return _blueTeamMax;
            }

            set
            {
                _blueTeamMax = value;
            }
        }

        public object RedTeamLock
        {
            get
            {
                return _redTeamLock;
            }
        }

        public object BlueTeamLock
        {
            get
            {
                return _blueTeamLock;
            }
        }

        public GameState(IReceiver gameMaster, ulong gameId, Xsd2.RegisterGame msg)
        {
            _players = new List<IReceiver>();
            _gameId = gameId;
            _gameMaster = gameMaster;
            _blueTeamMax = msg.NewGameInfo.blueTeamPlayers;
            _redTeamMax = msg.NewGameInfo.redTeamPlayers;
            _blueTeamCount = 0;
            _redTeamCount = 0;
            _gameName = msg.NewGameInfo.gameName;
        }
    }
}
