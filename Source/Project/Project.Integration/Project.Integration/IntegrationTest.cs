using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameMaster;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

namespace Project.Integration
{
    [TestClass]
    public class IntegrationTest
    {
        static int SLEEPTIME = 1000;
        const string IPADDRESS = "127.0.0.1";
        const int PORT = 8000;
        const string gameName = "gameName";
        private string[] parameters = new string[] { "gameName", "1" };
        private string[] playerParameters = new string[] { "30000", "red", "member" };

        //TOFIX
        [TestMethod]
        public void ChecksIfGameMasterCanConnectToCommunicationServer()
        {
            CommunicationServer.Connection serv = new CommunicationServer.Connection(PORT);
            serv.StartServer();
            GameMaster.Connection gameMaster = new GameMaster.Connection(IPADDRESS,PORT, parameters);
            gameMaster.Connect();
            Thread.Sleep(SLEEPTIME);

            // Checking if gamemaster connected
            Assert.AreEqual(1, serv.Receivers.Count);
            gameMaster.Disconnect();
            Thread.Sleep(SLEEPTIME);
            serv.StopServer();
            Thread.Sleep(SLEEPTIME);
            Task.WaitAll();
        }
        //TOFIX
        [TestMethod]
        public void ChecksIfPlayerAndGameMasterCanConnectToCommunicationServer()
        {
            CommunicationServer.Connection serv = new CommunicationServer.Connection(PORT + 1);
            serv.StartServer();
            GameMaster.Connection gameMaster = new GameMaster.Connection(IPADDRESS,PORT +1, parameters);
            gameMaster.Connect();
            var gameMessage = new Xsd2.RegisterGame(new Xsd2.GameInfo() { redTeamPlayers = 1, blueTeamPlayers = 1, gameName = gameName });
            var xmlSerializer = new XmlSerializer(typeof(Xsd2.RegisterGame));
            using (var writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, gameMessage);
                gameMaster.SendMessage(writer.ToString());
            }

            var player = FakeItEasy.A.Fake<Player.Connection>(o => o.WithArgumentsForConstructor(
            new object[] { "127.0.0.1", 8001, gameName, playerParameters, new object() }));
            player.Connect();
            Thread.Sleep(SLEEPTIME);

            int gmCount = 0;
            foreach(var reciever in serv.Receivers)
                if (reciever.IsGameMaster)
                    gmCount++;
            
            // Checking if there is only one gamemaster
            Assert.AreEqual(1, gmCount);
            // Checking if gamemaster and player properly connected
            Assert.AreEqual(2, serv.Receivers.Count);

            player.Disconnect();
            Thread.Sleep(SLEEPTIME);
            gameMaster.Disconnect();
            Thread.Sleep(SLEEPTIME);
            serv.StopServer();
            Thread.Sleep(SLEEPTIME);
            Task.WaitAll();
        }

        [TestMethod]
        public void ChecksIfNPlayersCanConnect()
        {   
            int n = 64;
            List<Player.Connection> players = new List<Player.Connection>();
            CommunicationServer.Connection serv = new CommunicationServer.Connection(8002);
            serv.StartServer();
            for(int i = 0; i < n; i++)
            {

                var player = FakeItEasy.A.Fake<Player.Connection>(o => o.WithArgumentsForConstructor(
                new object[] { "127.0.0.1", 8002, gameName, playerParameters, new object() }));
                player.Connect();
                players.Add(player);
            }
            Thread.Sleep(SLEEPTIME);

            // Checks if N players connected to the server
            Assert.AreEqual(n, serv.Receivers.Count);

            foreach (var player in players)
                player.Disconnect();
            Thread.Sleep(SLEEPTIME);
            serv.StopServer();
            Thread.Sleep(SLEEPTIME);
            Task.WaitAll();
        }

        // [TestMethod]
        // TODO Get it working ( Communication server doesn't delete reciver from the _recivers list )
        public void ChecksIfPlayerCanDisconnect()
        {
            CommunicationServer.Connection serv = new CommunicationServer.Connection(8003);
            serv.StartServer();
            
            Player.Connection player = new Player.Connection(IPADDRESS, PORT + 3, gameName, null);
            player.Connect();
            Thread.Sleep(SLEEPTIME+10000);
            player.Disconnect();

            Thread.Sleep(2000+10000);

            // Checks if no players still connected to the communication server
            Assert.AreEqual(0, serv.Receivers.Count);

            serv.StopServer();
            Thread.Sleep(SLEEPTIME);
            Task.WaitAll();
        }

        //[TestMethod]
        // TO FIX
        public void GameMasterDoesntStartGame()
        {
            CommunicationServer.Connection serv = new CommunicationServer.Connection(8004);
            serv.StartServer();
            GameMaster.Connection gameMaster = new GameMaster.Connection(IPADDRESS, PORT + 4, parameters);

            gameMaster.Connect();
            Thread.Sleep(SLEEPTIME);

            // Checks if IsGameMaster property is set to false 
            Assert.AreEqual(false, serv.Receivers[0].IsGameMaster);
            var gameMessage = new Xsd2.RegisterGame();
            var xmlSerializer = new XmlSerializer(typeof(Xsd2.RegisterGame));
            using (var writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, gameMessage);
                gameMaster.SendMessage(writer.ToString());
            }

            Thread.Sleep(SLEEPTIME);
            // Checks if IsGameMaster property is set to true 
            Assert.AreEqual(true, serv.Receivers[0].IsGameMaster);

            gameMaster.Disconnect();
            serv.StopServer();
            Task.WaitAll();
        }


        [TestMethod]
        public void CommunicationServerReceivesKeepAliveMessageFromGameMaster ()
        {
    
            CommunicationServer.Connection server = new CommunicationServer.Connection(8004);
            server.StartServer();
            GameMaster.Connection gameMaster = new GameMaster.Connection(IPADDRESS, PORT + 4, parameters);

            gameMaster.Connect();
            Thread.Sleep(4*SLEEPTIME);
            Assert.AreEqual(1, server.Receivers.Count);

            gameMaster.Disconnect();
            server.StopServer();
            Task.WaitAll();
        }

        //[TestMethod]
        public void KeepAliveFromGameMasterDoesNotInteractWithNormalMessages()
        {
   
            CommunicationServer.Connection server = new CommunicationServer.Connection(8004);
            server.StartServer();
            GameMaster.Connection gameMaster = new GameMaster.Connection(IPADDRESS, PORT + 4, parameters);
            gameMaster.Connect();
            Thread.Sleep(1500);
            var gameMessage = new Xsd2.RegisterGame(new Xsd2.GameInfo() { gameName = gameName, redTeamPlayers = 1, blueTeamPlayers = 1 });
            var xmlSerializer = new XmlSerializer(typeof(Xsd2.RegisterGame));
            using (var writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, gameMessage);
                gameMaster.SendMessage(writer.ToString());
            }

            Thread.Sleep(SLEEPTIME);
            // Checks if IsGameMaster property is set to true 
            Assert.AreEqual(true, server.Receivers[0].IsGameMaster);


            gameMaster.Disconnect();
            server.StopServer();
            Task.WaitAll();
        }
    }
}
