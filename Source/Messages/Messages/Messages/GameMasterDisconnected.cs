using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages.CommunicationServer;
using Messages.PlayerInterfaces;
using System.Xml.Serialization;
using System.IO;
using Messages.XmlHandling;

namespace Xsd2
{
    public partial class GameMasterDisconnected : ICommunicationServerHandler, IPlayerHandler
    {
        public GameMasterDisconnected() { }
        public GameMasterDisconnected(ulong gameId)
        {
            this.gameId = gameId;
        }
        // Handling is not done
        public void HandleOnCommunicationServer(Messages.CommunicationServer.IConnection server, IReceiver receiver)
        {
            var serializer = new Messages.XmlHandling.Serializer(this);
            string msg = serializer.Serialize();
            try
            {
                ulong gameId = receiver.GameId;
                var game = server.Games[gameId];

                foreach(var rec in game.Players)
                    {
                    rec.SendMessage(msg);
                }
                server.Receivers.Remove(receiver);
                server.Games.Remove(gameId);
            }catch(Exception)
            {
                Console.WriteLine("Error while notifying other clients, gm disconnected");
            }            
        }

        public void HandleOnPlayer(Messages.PlayerInterfaces.IConnection connection)
        {
            Console.WriteLine("Game Master disconnected, disconnecting the player");
            connection.Disconnect();
        }
    }
}
