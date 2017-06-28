using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace GameMaster.Test
{
    [TestClass]
    public class DiscoverTest
    {
        string[] parametersGm = new string[] { "gameName", "3" };
        string[] gameStateParams = new string[] { "gameName", "3", "3", "1" };
        [TestMethod]
        public void GameMasterSendsMessageToCommunicationServerAfterReceivingDiscover()
        {
            var gmConnection = A.Fake<GameMaster.Connection>(o => o.WithArgumentsForConstructor(
                new object[] { "127.0.0.1", 8000, gameStateParams, new object() }));
            gmConnection.GameState = new GameState(gameStateParams);

            var player = A.Fake<Messages.GameMaster.IPlayer>();
            player.TeamColour = Xsd2.TeamColour.blue;
            player.Location = new Xsd2.Location() { x = 1, y = 1 };
            gmConnection.GameState.BoardList = new List<List<Xsd2.Field>>();
            for(int i = 0; i < 2; i++)
            {
                gmConnection.GameState.BoardList.Add(new List<Xsd2.Field>());
                for(int j = 0; j < 2; j++)
                {
                    gmConnection.GameState.BoardList[i].Add(new Xsd2.TaskField() { x = (uint)i, y = (uint)j });
                }
            }
            gmConnection.GameState.AddPlayer(player);
            gmConnection.GameState.ActionCosts = new Configuration.GameMasterSettingsActionCosts();
            
            var msg = new Xsd2.Discover();
            msg.playerGuid = gmConnection.GameState.BluePlayers[0].Guid.ToString();
            msg.HandleOnGameMaster(gmConnection);
            Thread.Sleep(1000);
            Assert.AreEqual(1, gmConnection.MessageList.Count);
        }

        //[TestMethod]
        public void GameMasterSendsExactlyDataMessageToCommunicationServerAfterReceivingDiscover()
        {
            var gmConnection = A.Fake<GameMaster.Connection>(o => o.WithArgumentsForConstructor(
                new object[] { "127.0.0.1", 8000, parametersGm }));
            gmConnection.GameState.BluePlayers = new List<Messages.GameMaster.IPlayer>();
            var player = A.Fake<Messages.GameMaster.IPlayer>();
            player.TeamColour = Xsd2.TeamColour.blue;
            gmConnection.GameState.AddPlayer(player);

            var msg = new Xsd2.Discover();
            msg.playerGuid = gmConnection.GameState.BluePlayers[0].Guid.ToString();
            msg.HandleOnGameMaster(gmConnection);

            var xmlSerializer = new XmlSerializer(typeof(Xsd2.Data));
            //removing special character from message string
            string message = gmConnection.MessageList[0];
            message = message.Substring(0, message.Length - 1);
            using (var reader = new StringReader(message))
            {
                var obj = xmlSerializer.Deserialize(reader) as Xsd2.Data;
                Assert.AreNotEqual(null, obj);
            }
        }
    }
}
