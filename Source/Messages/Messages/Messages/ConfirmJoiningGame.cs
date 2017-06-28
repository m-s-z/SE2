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
using Messages;
namespace Xsd2
{
    public partial class ConfirmJoiningGame : ICommunicationServerHandler, IPlayerHandler
    {
        public ConfirmJoiningGame() { }
        public ConfirmJoiningGame(ulong gameId, Guid guid, ulong playerId, Player player)
        {
            this.gameId = gameId;
            this.privateGuid = guid.ToString();
            this.playerId = playerId;
            this.PlayerDefinition = player;
        }
        public void HandleOnCommunicationServer(Messages.CommunicationServer.IConnection server, IReceiver receiver)
        {
            var player = server.Receivers.Find(p => p.Id == this.playerId);
            if(player == null)
            {
                return;
            }
            player.GameId = receiver.GameId;
            var serializer = new Messages.XmlHandling.Serializer(this);
            player.SendMessage(serializer.Serialize());
            server.Games[receiver.GameId].Players.Add(player);
        }
    
        public void HandleOnPlayer(Messages.PlayerInterfaces.IConnection connection)
        {
            connection.Guid = new Guid(privateGuid);
            connection.GameId = gameId;
        }
    }
}
