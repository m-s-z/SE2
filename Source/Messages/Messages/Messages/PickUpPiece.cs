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
using Messages;

namespace Xsd2
{
    public partial class PickUpPiece : ICommunicationServerHandler, IGameMasterHandler
    {
        public PickUpPiece() { }
        public PickUpPiece(Guid _guid, ulong _gameId)
        {
            this.gameId = _gameId;
            this.playerGuid = _guid.ToString();
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
            var msg = new Data();
       
            IPlayer player = connection.GameState.BluePlayers.FirstOrDefault(p => p.Guid.ToString() == this.playerGuid);
            if (player == null)
            {
                player = connection.GameState.RedPlayers.FirstOrDefault(p => p.Guid.ToString() == this.playerGuid);
            }
            msg.playerId = player.Id;
            Piece[] pickedUpPiece = new Piece[1];
            foreach (PieceInfo p in connection.GameState.Pieces)
            {
                if (player.Location.x == p.Location.x && player.Location.y == p.Location.y)
                {
                    p.Piece.playerId = player.Id;
                    p.Piece.playerIdSpecified = true;
                    pickedUpPiece[0] = new Piece() { type = PieceType.unknown, playerId = player.Id, playerIdSpecified = true, id = p.Piece.id };
                    msg.Pieces = pickedUpPiece;
                    connection.GameState.CalculateManhattanDistance();
                   
                }
            }
            msg.gameFinished = connection.GameState.GameFinished;
            var serializer = new Messages.XmlHandling.Serializer(msg);
            connection.DelayMessage(serializer, connection.GameState.ActionCosts.PickUpDelay);
            
        }
    }
}
