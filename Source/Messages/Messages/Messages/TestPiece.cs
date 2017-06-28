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
    public partial class TestPiece : ICommunicationServerHandler, IGameMasterHandler
    {

        public TestPiece() { }

        public TestPiece(Guid _guid, ulong _gameId)
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
            if (player == null)
            {
                player = connection.GameState.RedPlayers.FirstOrDefault(p => p.Guid.ToString() == this.playerGuid);
            }
            msg.playerId = player.Id;
            Piece[] checkedPiece = new Piece[1];

            foreach (PieceInfo p in connection.GameState.Pieces)
            {
                if (player.Id == p.Piece.playerId)
                {
                    checkedPiece[0] = new Piece (){ type=p.Piece.type, playerId = player.Id, playerIdSpecified = true, id = p.Piece.id };
                    msg.Pieces = checkedPiece;
                }
            }
            msg.gameFinished = connection.GameState.GameFinished;

            var serializer = new Messages.XmlHandling.Serializer(msg);
            connection.DelayMessage(serializer, connection.GameState.ActionCosts.TestDelay);
        }

        
    }
}
