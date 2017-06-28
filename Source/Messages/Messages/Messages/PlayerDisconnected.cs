using Messages.CommunicationServer;
using Messages.GameMaster;
using Messages.PlayerInterfaces;
using Messages.XmlHandling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xsd2
{
    public partial class PlayerDisconnected : IGameMasterHandler, IPlayerHandler, ICommunicationServerHandler
    {
        public PlayerDisconnected() { }
        public PlayerDisconnected(ulong playerId)
        {
            this.playerId = playerId;
        }
        public void HandleOnCommunicationServer(Messages.CommunicationServer.IConnection server, IReceiver receiver)
        {
            //gameid == 0 -> player is not assigned to any game
            if(receiver.GameId == 0)
            {
                server.Receivers.Remove(receiver);
                return;
            }
            var serializer = new Messages.XmlHandling.Serializer(this);
            string msg = serializer.Serialize();
            ulong gameId = receiver.GameId;
            var game = server.Games[gameId];
            game.Players.Remove(receiver);
            server.Receivers.Remove(receiver);
            try
            {
                foreach(var player in game.Players)
                {
                    if(player.Status == true)
                    player.SendMessage(msg);
                }
                game.GameMaster.SendMessage(msg);
                if (game.Players.Count == 0)
                {
                    server.Games.Remove(game.GameId);
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Error while notifying other clients");
            }
            Console.WriteLine("Games: {0}", game.Players.Count);
        }

        public void HandleOnGameMaster(Messages.GameMaster.IConnection connection)
        {
            try
            {
                Console.WriteLine("Player id #{0} disconnected", this.playerId);
                connection.GameState.RemovePlayer(this.playerId);
                if (connection.GameState.GameStarted == true &&
                    connection.GameState.RedPlayers.Count == 0 &&
                    connection.GameState.BluePlayers.Count == 0)
                {
                    connection.Disconnect();
                    Console.WriteLine("All players disconnected");
                }
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid msg: {0}", this);
            }
            Thread.Sleep(2000);
        }

        public void HandleOnPlayer(Messages.PlayerInterfaces.IConnection connection)
        {
            Console.WriteLine("Player id #{0} disconnected", this.playerId);
            Thread.Sleep(2000);
        }
    }
}
