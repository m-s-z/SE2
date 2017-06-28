using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Collections.Generic;

namespace CommunicationServer.Test
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void CheckIfGameMessagesWasBroadcasted()
        {
            var server = A.Fake<CommunicationServer.Connection>();
            server.Receivers = new List<Messages.CommunicationServer.IReceiver>();
            int maxClients = 5;
            for(int i = 0; i < maxClients; i++)
            {
                server.Receivers.Add(A.Fake<CommunicationServer.Receiver>());
                server.Receivers[i].Id = (ulong)i;
            }
            var msg = new Xsd2.Game();
            msg.Players = new Xsd2.Player[maxClients];
            for(int i = 0; i < maxClients; i++)
            {
                msg.Players[i] = new Xsd2.Player() { id = (ulong)i };
            }

            msg.HandleOnCommunicationServer(server, null);

            foreach(var rec in server.Receivers)
            {
                Assert.AreEqual(1, rec.MessageList.Count);
            }
        }
    }
}
