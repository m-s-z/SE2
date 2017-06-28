using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using Player;
using System.Net.Sockets;
using System.Net;

namespace Player.Test
{
    [TestClass]
    public class ConnectionTest
    {
        Connection connection;
        const string IPADDRESS = "127.0.0.1";
        const int PORT = 8000;
        const string gameName = "gameName";
        static string[] parameters = new string[7] { "testname", "--address", "234.234.234.234", "--port", "400", "--conf", "somepath" };
        Messages.ParametersReader inputParameters = new Messages.ParametersReader(parameters);
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CheckingIfConnectionClassHandlesProperlyErrorsThrownDuringTcpClientConnectMethod()
        {
            //  Arrange
            var tcpClient = A.Fake<ITcpClient>();
            inputParameters.ReadPlayerInputSettings();
            A.CallTo(() => tcpClient.Connect(A<string>.Ignored, A<int>.Ignored)).Throws(new FakeException());
            connection = new Connection(IPADDRESS, 80, gameName,inputParameters);
            //  Act
            connection.Connect();

            //  Assert
        }
    }
}
