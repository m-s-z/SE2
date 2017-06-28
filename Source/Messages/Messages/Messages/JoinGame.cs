using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages.CommunicationServer;
using Messages.GameMaster;
using System.Xml.Serialization;
using System.IO;
using Messages.XmlHandling;

namespace Xsd2
{
    public partial class JoinGame : ICommunicationServerHandler, IGameMasterHandler
    {
        public JoinGame() { }
        public JoinGame(string gameName, Xsd2.TeamColour team, Xsd2.PlayerType type)
        {
            this.gameName = gameName;
            this.preferredRole = type;
            this.preferredTeam = team;
        }
        public void HandleOnCommunicationServer(Messages.CommunicationServer.IConnection server, IReceiver receiver)
        {
            lock (server.PlayerCountLock)
            {
                server.PlayerCount++;
                receiver.Id = server.PlayerCount;
            }
            this.playerId = receiver.Id;
            this.playerIdSpecified = true;
            var gameList = server.Games.Where(g => g.Value.GameName == this.gameName).ToList();
            if(gameList.Count == 1)
            {
                var game = gameList[0].Value;
                var serializer = new Messages.XmlHandling.Serializer(this);
                game.GameMaster.SendMessage(serializer.Serialize());
            }
            //else send reject message when defined in specification     
        }


        public void HandleOnGameMaster(Messages.GameMaster.IConnection connection)
        {
            var guid = Guid.NewGuid();
            var gameState = connection.GameState;
            var msg = new ConfirmJoiningGame(gameState.GameId, guid, this.playerId, new Player(this.playerId, this.preferredTeam, this.preferredRole));
            connection.AddPlayer(guid, msg.playerId, this.preferredTeam, this.preferredRole);

            var serializer = new Messages.XmlHandling.Serializer(msg);
            connection.SendMessage(serializer.Serialize());
            if(((ulong) gameState.RedPlayers.Count == gameState.RedTeamMax)
                &&
              ((ulong) gameState.BluePlayers.Count == gameState.BlueTeamMax))
            {
                gameState.GameStarted = true;
                var gameMsg = new Game();
                gameMsg.Players = new Player[gameState.RedTeamMax + gameState.BlueTeamMax];
                int index = 0;
                for(int i = 0; i < (int) gameState.RedTeamMax; i++, index++)
                {
                    var p = gameState.RedPlayers[i];
                    gameMsg.Players[index] = new Player(p.Id, p.TeamColour, p.PlayerType);
                }
                for (int i = 0; i < (int) gameState.BlueTeamMax; i++, index++)
                {
                    var p = gameState.BluePlayers[i];
                    gameMsg.Players[index] = new Player(p.Id, p.TeamColour, p.PlayerType);
                }
                connection.GameState.SetPlayerLocations();
                Console.WriteLine("Sending game msg");
                gameMsg.Board = new GameBoard();
                gameMsg.Board.goalsHeight = (uint)connection.GameState.GoalAreaLength;
                gameMsg.Board.tasksHeight = (uint)connection.GameState.TaskAreaLength;
                gameMsg.Board.width = (uint)connection.GameState.BoardWidth;

                foreach(var player in connection.GameState.RedPlayers)
                {
                    gameMsg.playerId = player.Id;
                    gameMsg.PlayerLocation = player.Location;
                    serializer = new Serializer(gameMsg);
                    connection.SendMessage(serializer.Serialize());
                }
                foreach (var player in connection.GameState.BluePlayers)
                {
                    gameMsg.playerId = player.Id;
                    gameMsg.PlayerLocation = player.Location;
                    serializer = new Serializer(gameMsg);
                    connection.SendMessage(serializer.Serialize());
                }

                connection.GameState.StartSpawningPieces();
            }
        }
    }
}
