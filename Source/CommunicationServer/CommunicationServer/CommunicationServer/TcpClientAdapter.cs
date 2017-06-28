using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Messages.CommunicationServer;

namespace Player
{
    public class TcpClientAdapter : ITcpClient
    {
        protected TcpClient _wrappedClient;
        public TcpClientAdapter(TcpClient client)
        {
            _wrappedClient = client;
        }

        public Socket Client { get { return _wrappedClient.Client; } set { _wrappedClient.Client = value; } }
        public int ReceiveBufferSize { get { return _wrappedClient.ReceiveBufferSize; } set { _wrappedClient.ReceiveBufferSize = value; } }
        public int SendBufferSize { get { return _wrappedClient.SendBufferSize; } set { _wrappedClient.SendBufferSize = value; } }

        public int Available { get { return _wrappedClient.Available; } }

        public void Connect(string endpoint, int port)
        {
            _wrappedClient.Connect(endpoint, port);
        }

        public void Close()
        {
            _wrappedClient.Close();
        }
        public Stream GetStream()
        {
            return _wrappedClient.GetStream();
        }

    }
}
