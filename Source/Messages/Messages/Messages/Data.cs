using Messages.CommunicationServer;
using Messages.PlayerInterfaces;
using Messages.XmlHandling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xsd2
{
    public partial class Data : ICommunicationServerHandler, IPlayerHandler
    {
        public void HandleOnCommunicationServer(Messages.CommunicationServer.IConnection connection, Messages.CommunicationServer.IReceiver receiver = null)
        {
            IReceiver rec = connection.Receivers.Find(p => p.Id == this.playerId);
            var serializer = new Messages.XmlHandling.Serializer(this);
            rec.SendMessage(serializer.Serialize());
        }

        public void HandleOnPlayer(Messages.PlayerInterfaces.IConnection connection)
        {
            if (gameFinished == true)
            {
                Console.WriteLine("Game is finished");
                return;
            }
            connection.Logic.SetReceivedData(this.TaskFields, this.GoalFields, this.gameFinished, this.Pieces, this.PlayerLocation);
            var msg = connection.Logic.ChooseNextMessage(connection, this);
            if (msg != null)
            {
                var serializer = new Messages.XmlHandling.Serializer(msg);
                connection.SendMessage(serializer.Serialize());
            }
        }
    }

}