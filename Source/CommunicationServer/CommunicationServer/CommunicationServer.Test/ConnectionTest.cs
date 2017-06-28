using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;

namespace CommunicationServer.Test
{
    [TestClass]
    public class ConnectionTest
    {
        [TestMethod]
        public void ListenerStarted()
        {
            var connection = new Connection(80);
            try
            {
                connection.StartServer();
            }
            catch 
            {
                Assert.Fail();
            }
            finally
            {
                connection.StopServer();
            }
        }

        [TestMethod]
        public void ServerProperlyStopped()
        {
            try
            {
                var connection = new Connection(80);
                connection.StartServer();
                connection.StopServer();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ErrorCode);
                Assert.Fail();
            }
        }

    }
}
