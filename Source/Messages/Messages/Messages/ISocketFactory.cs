using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    interface ISocketFactory
    {
        ITcpClient CreateTcpClient();

        ITcpListener getTcpListener();

    }
}
