using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Xml.Serialization;
using System.IO;

namespace CommunicationServer.Test
{
    [TestClass]
    public class DiscoverTest
    {
        private string gameName = "gameName";
        [TestMethod]
        public void CommunicationServerSendDataMessageToGameServer()
        {
            var server = A.Fake<CommunicationServer.Connection>();
            var receiver = A.Fake<CommunicationServer.Receiver>();
            receiver.GameId = 1;
            var game = A.Fake<Messages.CommunicationServer.IGameState>();
            game.GameMaster = A.Fake<CommunicationServer.Receiver>();
            game.GameName = gameName;
            server.Games.Add(1, game);

            var msg = new Xsd2.Discover();
            msg.HandleOnCommunicationServer(server, receiver);

            Assert.AreEqual(1, game.GameMaster.MessageList.Count);
        }

    }

}
