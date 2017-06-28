using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

namespace GameMaster.Test
{

    [TestClass]
    public class JoinGameTest
    {
        static string[] parameters = new string[7] { "testname", "--address", "234.234.234.234", "--port", "400", "--conf", "somepath" };
        string[] parametersGm = new string[] { "gameName", "3" };
        Messages.ParametersReader inputParameters = new Messages.ParametersReader(parameters);
        //[TestMethod]
        public void GameMasterSendsGameMessageWhenAllPlayersConnect()
        {
            var xmlSerializer = new XmlSerializer(typeof(Xsd2.Game));
            var gmConnection = A.Fake<GameMaster.Connection>(o => o.WithArgumentsForConstructor(
                new object[] { "127.0.0.1", 8000, parametersGm }));
            gmConnection.GameState.RedTeamMax = 1;
            gmConnection.GameState.BlueTeamMax = 1;
            var msgRed = new Xsd2.JoinGame();
            var msgBlue = new Xsd2.JoinGame();
            msgRed.preferredTeam = Xsd2.TeamColour.red;
            msgBlue.preferredTeam = Xsd2.TeamColour.blue;
            msgRed.HandleOnGameMaster(gmConnection);
            msgBlue.HandleOnGameMaster(gmConnection);
            //reading most recent message
            var msg = gmConnection.MessageList[gmConnection.MessageList.Count - 1];
            //removing keep alive character
            msg = msg.Substring(0, msg.Length - 1);
            using (var reader = new StringReader(msg))
            {
                var obj = xmlSerializer.Deserialize(reader) as Xsd2.Game;
                Assert.AreNotEqual(null, obj);
            }
        }

        public void GameMasterSendsConfirmJoinMessageWhenAPlayerConnects()
        {
            var xmlSerializer = new XmlSerializer(typeof(Xsd2.ConfirmJoiningGame));
            var gmConnection = A.Fake<GameMaster.Connection>();
            gmConnection.GameState.RedTeamMax = 1;
            gmConnection.GameState.BlueTeamMax = 1;
            var msgRed = new Xsd2.JoinGame();
            msgRed.preferredTeam = Xsd2.TeamColour.red;
            msgRed.HandleOnGameMaster(gmConnection);
            //reading most recent message
            var msg = gmConnection.MessageList[gmConnection.MessageList.Count - 1];
            using (var reader = new StringReader(msg))
            {
                var obj = xmlSerializer.Deserialize(reader) as Xsd2.ConfirmJoiningGame;
                Assert.AreNotEqual(null, obj);
            }
        }

        //[TestMethod]
        public void GameMasterSendsExactlyConfirmJoiningGameMessageToCommunicationServerAfterReceivingJoinGameMessage()
        {
            var gmConnection = A.Fake<Messages.GameMaster.IConnection>();

            gmConnection.GameState = A.Fake<Messages.GameMaster.IGameState>();
            gmConnection.GameState.BlueTeamMax = 2;
            gmConnection.GameState.RedTeamMax = 2;
          
            var msg = new Xsd2.JoinGame();
            msg.gameName = "";
            msg.HandleOnGameMaster(gmConnection);

            var xmlSerializer = new XmlSerializer(typeof(Xsd2.ConfirmJoiningGame));
            Thread.Sleep(3000);
            //removing special character from message string
            string message = gmConnection.MessageList[0];
            message = message.Substring(0, message.Length - 1);
            using (var reader = new StringReader(message))
            {
                var obj = xmlSerializer.Deserialize(reader) as Xsd2.ConfirmJoiningGame;
                Assert.AreNotEqual(null, obj);
            }
        }
        [TestMethod] //No RejectJoinGame yet
        public void PlayerTriesToConnectToFullGame()
        {
            var xmlSerializer = new XmlSerializer(typeof(Xsd2.RejectJoiningGame));
            var gmConnection = A.Fake<GameMaster.Connection>(o => o.WithArgumentsForConstructor(
                new object[] { "127.0.0.1", 8000, parametersGm }));
            gmConnection.GameState.RedTeamMax = 1;
            gmConnection.GameState.BlueTeamMax = 1;
            var msgRed = new Xsd2.JoinGame();
            var msgBlue = new Xsd2.JoinGame();
            var msgLate = new Xsd2.JoinGame();
            msgRed.preferredTeam = Xsd2.TeamColour.red;
            msgBlue.preferredTeam = Xsd2.TeamColour.blue;
            msgLate.preferredTeam = Xsd2.TeamColour.blue;
            msgRed.HandleOnGameMaster(gmConnection);
            msgBlue.HandleOnGameMaster(gmConnection);

            msgLate.HandleOnGameMaster(gmConnection);
            //reading most recent message
            var msg = gmConnection.MessageList[gmConnection.MessageList.Count - 1];
            msg = msg.Substring(0, msg.Length - 1);
            using (var reader = new StringReader(msg))
            {
                var obj = xmlSerializer.Deserialize(reader) as Xsd2.RejectJoiningGame;
                Assert.AreNotEqual(null, obj);
            }
        }
    }
}
