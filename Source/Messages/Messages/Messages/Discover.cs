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
    public partial class Discover : ICommunicationServerHandler, IGameMasterHandler
    {
        public Discover() { }

        public Discover (Guid _guid, ulong _gameId )
        {
            this.gameId = _gameId;
            this.playerGuid = _guid.ToString();
        }

        public void HandleOnCommunicationServer(Messages.CommunicationServer.IConnection server, IReceiver receiver)
        {

            var serializer = new Messages.XmlHandling.Serializer(this);
            var gameId = receiver.GameId;
            var game = server.Games[gameId];
            this.gameId = gameId;
            game.GameMaster.SendMessage(serializer.Serialize());
        }

        public void HandleOnGameMaster(Messages.GameMaster.IConnection connection)
        {
            var msg = new Data();
            IPlayer player = connection.GameState.BluePlayers.FirstOrDefault(p => p.Guid.ToString() == this.playerGuid);
            if(player == null)
            {
                player = connection.GameState.RedPlayers.FirstOrDefault(p => p.Guid.ToString() == this.playerGuid);
            }
            msg.TaskFields = GetSurroundingFields(connection, player);
            msg.playerId = player.Id;
            msg.gameFinished = connection.GameState.GameFinished;
            var serializer = new Messages.XmlHandling.Serializer(msg);
            connection.DelayMessage(serializer, connection.GameState.ActionCosts.DiscoverDelay);
        }

        private TaskField[] GetSurroundingFields(Messages.GameMaster.IConnection connection, IPlayer player)
        {
            var gameState = connection.GameState;
            var location = player.Location;
            var taskFields = new List<TaskField>();
            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    if(location.x + i < 0 || location.x + i >= gameState.BoardWidth
                        || location.y + j < 0 || location.y + j >= (gameState.TaskAreaLength + gameState.GoalAreaLength * 2))
                    {
                        continue;
                    }
                    if(gameState.BoardList[(int)location.x + i][(int)location.y + j] is Xsd2.TaskField)
                    {
                        //Console.WriteLine(" my location x is: " + location.x + " y is: " + location.y);
                        taskFields.Add(gameState.BoardList[(int)location.x + i][(int)location.y + j] as TaskField);
                        
                    }
                }
            }
            
            return taskFields?.ToArray();
        }
    }
}
