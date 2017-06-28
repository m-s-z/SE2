using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace CommunicationServer.Test
{
    [TestClass]
    public class ConfirmJoiningGameTest
    {
        [TestMethod]
        public void CommunicationServerSendsExactlyConfirmJoiningGameMessageToPlayer()
        {
            var server = A.Fake<CommunicationServer.Connection>();
            var gm = A.Fake<CommunicationServer.Receiver>();
            server.Receivers = new List<Messages.CommunicationServer.IReceiver>();
            server.Receivers.Add(gm);
            gm.Id = 1;
            server.CreateGame(gm, new Xsd2.RegisterGame(new Xsd2.GameInfo() { redTeamPlayers = 1, blueTeamPlayers = 1, gameName = "gameName" }));
            gm.IsGameMaster = true;

            var msg = new Xsd2.ConfirmJoiningGame();
            msg.playerId = 1;
            msg.HandleOnCommunicationServer(server, gm);

            var xmlSerializer = new XmlSerializer(typeof(Xsd2.ConfirmJoiningGame));
            string message = server.Receivers[0].MessageList[0];
            message = message.Substring(0, message.Length - 1);
            using (var reader = new StringReader(message))
            {
                var obj = xmlSerializer.Deserialize(reader) as Xsd2.ConfirmJoiningGame;
                Assert.AreNotEqual(null, obj);
            }
        }
        [TestMethod]
        public void CommunicationServerSendsConfirmJoiningGameMessageToCorrectPlayer()
        {
            var server = new CommunicationServer.Connection(8001);
            var playerOne = A.Fake<CommunicationServer.Receiver>();
            //var gm= FakeItEasy.A.Fake<Messages.GameMaster.IConnection>(o => o.WithArgumentsForConstructor(
            //new object[] { "127.0.0.1", 8001, new string[] { "placeholder" } }));
            var gm = A.Fake<CommunicationServer.Receiver>();
            server.Receivers = new List<Messages.CommunicationServer.IReceiver>();
            server.CreateGame(gm, new Xsd2.RegisterGame(new Xsd2.GameInfo() {redTeamPlayers = 1, blueTeamPlayers = 1, gameName = "gameName" }));
            server.Receivers.Add(playerOne);
            server.Receivers.Add(gm);
            playerOne.Id = 1;
            gm.Id = 2;
            server.Receivers[1].IsGameMaster = true;
            
            var msg = new Xsd2.ConfirmJoiningGame();
            msg.playerId = 1;
            msg.HandleOnCommunicationServer(server, gm);

            Assert.AreEqual(1, playerOne.MessageList.Count);
        }

    }
}
