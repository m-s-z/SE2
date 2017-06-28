using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Xml.Serialization;
using System.IO;

namespace CommunicationServer.Test
{
    [TestClass]
    public class RegisterGameTest
    {
        private string gameName = "gameName";
        [TestMethod]
        public void RegistersNewGameMaster()
        {
            var server = A.Fake<CommunicationServer.Connection>();
            var receiver = A.Fake<CommunicationServer.Receiver>();
            var msg = new Xsd2.RegisterGame(new Xsd2.GameInfo() { gameName = gameName, redTeamPlayers = 1, blueTeamPlayers = 1 });

            msg.HandleOnCommunicationServer(server, receiver);

            Assert.AreEqual(receiver, server.Games[1].GameMaster);
            Assert.AreEqual(true, receiver.IsGameMaster);
        }

        [TestMethod]
        public void DoesNotRegisterNewGameMaster()
        {
            var server = A.Fake<CommunicationServer.Connection>();
            var receiver = A.Fake<CommunicationServer.Receiver>();
            var game = A.Fake<Messages.CommunicationServer.IGameState>();
            game.GameName = gameName;
            server.Games.Add(1, game);
            var msg = new Xsd2.RegisterGame(new Xsd2.GameInfo() { blueTeamPlayers = 1, redTeamPlayers = 1, gameName = gameName });
            msg.HandleOnCommunicationServer(server, receiver);

            var xmlSerializer = new XmlSerializer(typeof(Xsd2.RejectGameRegistration));
            //removing special character from message string
            string message = receiver.MessageList[0];
            message = message.Substring(0, message.Length - 1);
            using (var reader = new StringReader(message))
            {
                var obj = xmlSerializer.Deserialize(reader) as Xsd2.RejectGameRegistration;
                Assert.AreNotEqual(null, obj);

            }
        }
    }
}
