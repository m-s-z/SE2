using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    interface ITcpClient
    {
        int ReceiveBufferSize { get; set; }
        int SendBufferSize { get; set; }
        Socket Client { get; set; }
        int Available { get; }

        void Connect(string endpoint, int port);

        Stream GetStream();
        void Close();
    }
}
