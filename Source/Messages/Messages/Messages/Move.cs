using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages.CommunicationServer;
using Messages.GameMaster;
using System.Xml.Serialization;
using System.IO;
using Xsd2;
namespace Xsd2
{
    public partial class Move : ICommunicationServerHandler, IGameMasterHandler
    {

        public Move()
        {
        }

        public Move(Guid guid, MoveType _direction)
        {
            this.playerGuid = guid.ToString();
            this.direction = _direction;
            this.directionSpecified = true;
        }


        public void HandleOnCommunicationServer(Messages.CommunicationServer.IConnection server, IReceiver receiver)
        {
            var serializer = new Messages.XmlHandling.Serializer(this);
            ulong gameId = receiver.GameId;
            var game = server.Games[gameId];
            this.gameId = gameId;
            game.GameMaster.SendMessage(serializer.Serialize());
        }

        public void HandleOnGameMaster(Messages.GameMaster.IConnection connection)
        {
            IPlayer player = connection.GameState.BluePlayers.FirstOrDefault(p => p.Guid.ToString() == this.playerGuid);
            if (player == null)
            {
                player = connection.GameState.RedPlayers.FirstOrDefault(p => p.Guid.ToString() == this.playerGuid);
            }
            //TO DO:location of piece carried by player should also change
            var location = Validate(connection, player);
            var piece = connection.GameState.Pieces.FirstOrDefault(p => p.Piece.playerId == player.Id && p.Piece.playerIdSpecified == true);
            if(piece != null)
            {
                piece.Location = location;
            }
            var msg = new Data();
            msg.PlayerLocation = player.Location = location;
            msg.playerId = player.Id;
            msg.gameFinished = connection.GameState.GameFinished;
            var serializer = new Messages.XmlHandling.Serializer(msg);
            Task delay = new Task(()=> {
                System.Threading.Thread.Sleep((int)connection.GameState.ActionCosts.MoveDelay);
                connection.SendMessage(serializer.Serialize());
                Console.WriteLine("I have waited for: " + (int)connection.GameState.ActionCosts.MoveDelay);
            });
            delay.Start();
            
        }

        private Location Validate(Messages.GameMaster.IConnection connection, IPlayer player)
        {
            var gameState = connection.GameState;
            var newLocation = GetNewPlayerLocation(player);

            if(player.TeamColour == TeamColour.red)
            {
                if(newLocation.y >= (gameState.GoalAreaLength + gameState.TaskAreaLength) || newLocation.y < 0)
                {
                    return player.Location;
                }
            }
            if(player.TeamColour == TeamColour.blue)
            {
                if(newLocation.y >= (gameState.GoalAreaLength * 2 + gameState.TaskAreaLength) || 
                    newLocation.y < (gameState.GoalAreaLength))
                {
                    return player.Location;
                }
            }
            if(newLocation.x >= gameState.BoardWidth || newLocation.x < 0)
            {
                return player.Location;
            }
            foreach(var p in gameState.RedPlayers)
            {
                if(p.Location.x == newLocation.x && p.Location.y == newLocation.y
                    && p.Id != player.Id)
                {
                    return player.Location;
                }
            }
            foreach(var p in gameState.BluePlayers)
            {
                if (p.Location.x == newLocation.x && p.Location.y == newLocation.y
                    && p.Id != player.Id)
                {
                    return player.Location;
                }
            }
            return newLocation;
        }

        private Xsd2.Location GetNewPlayerLocation(IPlayer player)
        {
            int xMove = 0;
            int yMove = 0;
            if (this.direction == MoveType.left)
            {
                xMove = -1;
            }
            if (this.direction == MoveType.right)
            {
                xMove = 1;
            }
            if (this.direction == MoveType.up)
            {
                yMove = 1;
            }
            if (this.direction == MoveType.down)
            {
                yMove = -1;
            }

            return new Location() { x = (uint)(player.Location.x + xMove), y = (uint)(player.Location.y + yMove) };
        }
    }
}
