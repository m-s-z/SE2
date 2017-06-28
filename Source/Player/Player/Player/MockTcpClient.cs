using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    public class MockTcpClient : ITcpClient
    {
        public int Available
        {
            get
            {
                return 0;
            }
        }

        public Socket Client
        {
            get
            {
                return null;
            }

            set
            {
                value = null;
            }
        }

        public int ReceiveBufferSize
        {
            get
            {
                return 0;
            }

            set
            {
                value = 0;
            }
        }

        public int SendBufferSize
        {
            get
            {
                return 0;
            }

            set
            {
                value = 0;
            }
        }

        public void Close()
        {
            return;
        }

        public void Connect(string endpoint, int port)
        {
            return;
        }

        public Stream GetStream()
        {
            return null;
        }
    }
}
