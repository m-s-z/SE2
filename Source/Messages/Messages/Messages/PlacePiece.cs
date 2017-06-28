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
    public partial class PlacePiece : ICommunicationServerHandler, IGameMasterHandler
    {

        public PlacePiece() { }

        public PlacePiece(Guid _guid, ulong _gameId)
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

            Piece[] placedPiece = new Piece[1];
            GoalField[] goalFields = new GoalField[1];
            //we pick piece which shares the location with player.
            PieceInfo myPiece = connection.GameState.Pieces.First(p => p.Location.x == player.Location.x 
                                                                    && p.Location.y == player.Location.y 
                                                                    && p.Piece.playerId == player.Id 
                                                                    && p.Piece.playerIdSpecified == true);
            if (myPiece.Piece.type != PieceType.sham)
            { 
                //if piece is on place we are standing,
                foreach (PieceInfo p in connection.GameState.Pieces)
                {
                    if (player.Location.x == p.Location.x && player.Location.y == p.Location.y)
                    {
                        if (player.Id != p.Piece.playerId)
                        {
                            placedPiece[0] = new Piece() { type = myPiece.Piece.type, playerId = player.Id, playerIdSpecified = true, id = myPiece.Piece.id };
                        }   
                    }
                }
                // this is overriding the lines before, we need to think through this process.
                placedPiece[0] = new Piece() { type = myPiece.Piece.type, id = myPiece.Piece.id };
            
                foreach (List<Xsd2.Field> flist in connection.GameState.BoardList)
                {
                    foreach (var f in flist)
                    {
                        if (f is GoalField)
                        {
                            if (f.x == player.Location.x && f.y == player.Location.y)
                            {
                                goalFields[0] = new GoalField() { playerId = player.Id, type = ((GoalField)f).type, playerIdSpecified = true, x = f.x, y = f.y };
                                Console.WriteLine(((GoalField)f).type);
                                Console.WriteLine("f.x: " + f.x + ", f.y: "+ f.y);
                                Console.WriteLine(" x " + player.Location.x + " y "+ player.Location.y);
                                //testing whether piece is not sham.
                                if (((GoalField)f).type == GoalFieldType.goal && myPiece.Piece.type == PieceType.normal)
                                {
                                    if (player.TeamColour == TeamColour.red)
                                        connection.GameState.RedGoalsFinished++;
                                    else
                                        connection.GameState.BlueGoalsFinished++;
                                }
                            }
                        }
                    }
                }
                // why do we send here piece?
                msg.Pieces = placedPiece; // we are still or not holding it.
                msg.GoalFields = goalFields;
                if (connection.GameState.GameFinished != true)
                {
                    connection.GameState.GameFinished = CheckIfEnding(connection);
                }
                //edit
                msg.gameFinished = connection.GameState.GameFinished;
                var serializer = new Messages.XmlHandling.Serializer(msg);
                connection.DelayMessage(serializer, connection.GameState.ActionCosts.PlacingDelay);
            }
            else
            {
                connection.GameState.Pieces.First(p => p.Location.x == player.Location.x
                                                                    && p.Location.y == player.Location.y
                                                                    && p.Piece.playerId == player.Id
                                                                    && p.Piece.playerIdSpecified == true).Piece.playerIdSpecified=false;
                //we are sending the keep alive back
                var serializer = new Messages.XmlHandling.Serializer(char.ToString((char)23));
            }
        }

        private bool CheckIfEnding (Messages.GameMaster.IConnection connection)
        {
            //Console.WriteLine("i red goal values : " + connection.GameState.RedGoalsFinished + " blue goals finished " + connection.GameState.BlueGoalsFinished);
            //Console.WriteLine("i red goal TO BE DONE : " + connection.GameState.RedNumberOfGoals + " blue goals TO BE DONE " + connection.GameState.BlueNumberOfGoals);
            if (connection.GameState.RedGoalsFinished == connection.GameState.RedNumberOfGoals)
                return true;
            if (connection.GameState.BlueGoalsFinished == connection.GameState.BlueNumberOfGoals)
                return true;
            return false;
        }
    }
}
