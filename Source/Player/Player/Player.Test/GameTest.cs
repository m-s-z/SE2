using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Xml.Serialization;
using System.IO;

namespace Player.Test
{
    [TestClass]
    public class GameTest
    {
        private string[] playerParameters = new string[] { "30000", "red", "member" };
        private string gameName = "";
        //[TestMethod]
        public void PlayerSendsGameMessage()
        {
            var player = FakeItEasy.A.Fake<Player.Connection>(o => o.WithArgumentsForConstructor(
                new object[] { "127.0.0.1", 8002, gameName, playerParameters, new object() }));
            var msg = new Xsd2.Game();
            
            var guid = new Guid();
            player.Guid = guid;
            msg.HandleOnPlayer(player);

            Assert.IsTrue(!string.IsNullOrEmpty(player.MessageList[0]));
        }
    }
}
