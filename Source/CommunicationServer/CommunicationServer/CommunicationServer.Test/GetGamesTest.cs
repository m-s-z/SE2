using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;

namespace CommunicationServer.Test
{
    [TestClass]
    public class GetGamesTest
    {
        [TestMethod]
        public void CreatesMessageWithGame()
        {
            var server = A.Fake<CommunicationServer.Connection>();
            var receiver = A.Fake<CommunicationServer.Receiver>();
            
            var msg = new Xsd2.GetGames();
            msg.HandleOnCommunicationServer(server, receiver);

            Assert.AreEqual(1, receiver.MessageList.Count);
        }
    }
}
