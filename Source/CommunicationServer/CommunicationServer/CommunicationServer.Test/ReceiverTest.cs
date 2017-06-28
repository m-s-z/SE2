using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using Player;

namespace CommunicationServer.Test
{

    [TestClass]
    public class ReceiverTest
    {


        [TestMethod]
        public void ReceiverStartedProperly()
        {
            var client = new TcpClientAdapter(new TcpClient());
            var server = new Connection(80);
            var receiver = new Receiver(client, server);
            try
            {
                receiver.Start();
            }
            catch
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void ReceiverDisconnectedProperly()
        {
            var client = new TcpClientAdapter(new TcpClient());
            var server = new Connection(80);
            var receiver = new Receiver(client, server);
            try
            {
                receiver.Start();
                receiver.Disconnect();
            }
            catch (AggregateException e)
            {
                Assert.Fail();
            }

        }


        [TestMethod]
        public void AddingToTheMessageList()
        {
            var client = new TcpClientAdapter(new TcpClient());
            var server = new Connection(80);
            var receiver = new Receiver(client, server);
         
            receiver.SendMessage("test message");
            Assert.IsTrue(receiver.MessageList.Count > 0);
        }
    }
}
