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
    public partial class Game : ICommunicationServerHandler, IPlayerHandler
    {
        public void HandleOnCommunicationServer(Messages.CommunicationServer.IConnection server, IReceiver receiver)
        {
            var serializer = new Messages.XmlHandling.Serializer(this);
            List<IReceiver> receivers = new List<IReceiver>();
            foreach (Player p in this.Players)
            {
                receivers.Add(server.Receivers.Find(r => r.Id == p.id));
            }
            var rec = server.Receivers.Find(r => r.Id == this.playerId);
            string msg = serializer.Serialize();

            rec.SendMessage(msg);
        }

        public void HandleOnPlayer(Messages.PlayerInterfaces.IConnection connection)
        {
            var msg = connection.Logic.AnswerForGameMessage(connection, this);
            var serializer = new Messages.XmlHandling.Serializer(msg);
            connection.SendMessage(serializer.Serialize());
        }
    }
}
