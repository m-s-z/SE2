using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.CommunicationServer
{
    public interface ICommunicationServerHandler
    {
        void HandleOnCommunicationServer(Messages.CommunicationServer.IConnection server, Messages.CommunicationServer.IReceiver receiver);
    }
}
