using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages.PlayerInterfaces;
using System.Xml.Serialization;
using System.IO;
using Messages.XmlHandling;
using Xsd2;
namespace Xsd2
{
    public partial class RegisteredGames : IPlayerHandler
    {
        public void HandleOnPlayer(IConnection connection)
        {
            var gameInfo = this.GameInfo;
            //XmlSerializer xmlSerializer = null;
            Messages.CommunicationServer.ICommunicationServerHandler msg = null;
            if(gameInfo != null)
            {
                msg = new JoinGame(connection.GameName, connection.Team, connection.Role);
            }
            else
            {
                msg = new GetGames();
            }
            var serializer = new Messages.XmlHandling.Serializer(msg);
            connection.SendMessage(serializer.Serialize());

        }
    }
}
