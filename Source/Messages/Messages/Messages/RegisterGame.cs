using Messages.CommunicationServer;
using Messages.XmlHandling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xsd2;
namespace Xsd2
{
    public partial class RegisterGame : ICommunicationServerHandler
    {
        public RegisterGame()
        {
  
        }
        public RegisterGame(GameInfo gameInfo)
        {
            this.NewGameInfo = gameInfo;
        }
        public void HandleOnCommunicationServer(Messages.CommunicationServer.IConnection server, Messages.CommunicationServer.IReceiver receiver)
        {
            var game = server.CreateGame(receiver, this);
            if(game == null)
            {
                var msg = new RejectGameRegistration();
                var serializer = new Messages.XmlHandling.Serializer(msg);
                receiver.SendMessage(serializer.Serialize());
            }
            else
            {
                var msg = new ConfirmGameRegistration(game.GameId);
                var serializer = new Messages.XmlHandling.Serializer(msg);
                receiver.SendMessage(serializer.Serialize());
            }
          
            
        }
    }
}
