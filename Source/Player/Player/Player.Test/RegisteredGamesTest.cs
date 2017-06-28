using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Xml.Serialization;
using System.IO;

namespace Player.Test
{
    [TestClass]
    public class RegisteredGamesTest
    {
        private string[] playerParameters = new string[] { "30000", "red", "member" };
        private string gameName = "";
        [TestMethod]
        public void PlayerGetsJoinGame()
        {
            var player = FakeItEasy.A.Fake<Player.Connection>(o => o.WithArgumentsForConstructor(
                new object[] { "127.0.0.1", 8002, gameName, playerParameters, new object() }));
            var msg = new Xsd2.RegisteredGames();
            msg.GameInfo = new Xsd2.GameInfo[1];
            msg.GameInfo[0] = new Xsd2.GameInfo();

            msg.HandleOnPlayer(player);

            var xmlSerializer = new XmlSerializer(typeof(Xsd2.JoinGame));
            //removing special character from message string
            string message = player.MessageList[0];
            message = message.Substring(0, message.Length - 1);
            using (var reader = new StringReader(message))
            {
                var obj = xmlSerializer.Deserialize(reader) as Xsd2.JoinGame;
                Assert.AreNotEqual(null, obj);

            }
        }

        [TestMethod]
        public void PlayerGetsEmptyRegisteredGamesAndSendsGetGamesAgain()
        {
            var player = FakeItEasy.A.Fake<Player.Connection>(o => o.WithArgumentsForConstructor(
                new object[] { "127.0.0.1", 8002, gameName, playerParameters, new object() }));

            var msg = new Xsd2.RegisteredGames();

            msg.HandleOnPlayer(player);

            var xmlSerializer = new XmlSerializer(typeof(Xsd2.GetGames));
            //removing special character from message string
            string message = player.MessageList[0];
            message = message.Substring(0, message.Length - 1);
            using (var reader = new StringReader(message))
            {
                var obj = xmlSerializer.Deserialize(reader) as Xsd2.GetGames;
                Assert.AreNotEqual(null, obj);
            }

        }
    }
}
