using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;

namespace Player.Test
{
    [TestClass]
    public class ConfirmJoiningGameTest
    {
        private string[] playerParameters = new string[] { "30000", "red", "member" };
        private string gameName = "";
        [TestMethod]
        public void PlayerGetsProperGuid()
        {
            var player = FakeItEasy.A.Fake<Player.Connection>(o => o.WithArgumentsForConstructor(
                new object[] { "127.0.0.1", 8002, gameName, playerParameters, new object() }));

            var msg = new Xsd2.ConfirmJoiningGame();
            var guid = new Guid();
            msg.privateGuid = guid.ToString();

            msg.HandleOnPlayer(player);

            Assert.AreEqual(guid, player.Guid);
        }
    }
}
