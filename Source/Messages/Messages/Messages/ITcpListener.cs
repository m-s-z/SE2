using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    interface ITcpListener
    {
        void Start();
        void Stop();

        TcpClient EndAcceptTcpClient(IAsyncResult a);

        void BeginAcceptTcpClient(AsyncCallback a, object state);
    }
}
