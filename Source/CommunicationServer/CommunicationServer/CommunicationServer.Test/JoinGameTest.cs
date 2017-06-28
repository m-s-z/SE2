using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Xml.Serialization;
using System.IO;

namespace CommunicationServer.Test
{
    [TestClass]
    public class JoinGameTest
    {
        private string gameName = "gameName";
        [TestMethod]
        public void PlayerCountIncreaseAFterGettingJoinGameMessage()
        {
            var server = A.Fake<CommunicationServer.Connection>();
            var receiver = A.Fake<CommunicationServer.Receiver>();


            var msg = new Xsd2.JoinGame();
            msg.HandleOnCommunicationServer(server, receiver);

            Assert.AreEqual((ulong)1, server.PlayerCount);
        }

        [TestMethod]
        public void CommunicationServerPassesMessageToGameMaster()
        {
            var server = A.Fake<CommunicationServer.Connection>();
            var receiver = A.Fake<CommunicationServer.Receiver>();
            var game = A.Fake<Messages.CommunicationServer.IGameState>();
            game.GameMaster = A.Fake<CommunicationServer.Receiver>();
            game.GameName = gameName;
            server.Games.Add(1, game);

            var msg = new Xsd2.JoinGame(gameName, Xsd2.TeamColour.blue, Xsd2.PlayerType.member);
            msg.HandleOnCommunicationServer(server, receiver);

            Assert.AreEqual(1, game.GameMaster.MessageList.Count);
        }

        [TestMethod]
        public void CommunicationServerPassesExactlyJoinGameMessageToGameMaster()
        {
            var server = A.Fake<CommunicationServer.Connection>();
            var receiver = A.Fake<CommunicationServer.Receiver>();
            var game = A.Fake<Messages.CommunicationServer.IGameState>();
            game.GameMaster = A.Fake<CommunicationServer.Receiver>();
            game.GameName = gameName;
            server.Games.Add(1, game);


            var msg = new Xsd2.JoinGame(gameName, Xsd2.TeamColour.blue, Xsd2.PlayerType.leader);
            msg.HandleOnCommunicationServer(server, receiver);

            var xmlSerializer = new XmlSerializer(typeof(Xsd2.JoinGame));
            //removing special character from message string
            string message = game.GameMaster.MessageList[0];
            message = message.Substring(0, message.Length - 1);
            using (var reader = new StringReader(message))
            {
                var obj = xmlSerializer.Deserialize(reader) as Xsd2.JoinGame;
                Assert.AreNotEqual(null, obj);
            }
        }
    }
}
