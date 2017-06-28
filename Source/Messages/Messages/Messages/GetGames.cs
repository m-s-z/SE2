using Messages.CommunicationServer;
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
    public partial class GetGames : ICommunicationServerHandler
    {
        public void HandleOnCommunicationServer(IConnection server, IReceiver receiver)
        {
            var msg = new RegisteredGames();
            var count = (int)server.GameCount;
            msg.GameInfo = new GameInfo[count];
            int i = 0;
            foreach(var pair in server.Games)
            {
                msg.GameInfo[i] = new GameInfo(pair.Value);
                i++;
            }
            var serializer = new Messages.XmlHandling.Serializer(msg);
            receiver.SendMessage(serializer.Serialize());
        }
    }
}
