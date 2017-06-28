using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Messages.Test
{
    [TestClass]
    public class ParametersReaderTest
    {
        string[] parameters = new string[7] { "17-EN-04-gm", "--address", "234.234.234.234", "--port", "400", "--conf", "somepath" };
        [TestMethod]
        public void ParameterReaderPropertlyReadsTheCommandLineArguments()
        {

            var parameterReader = new Messages.ParametersReader(parameters);
            Assert.AreEqual("234.234.234.234", parameterReader.Adress);
            Assert.AreEqual("400", parameterReader.Port);
            Assert.AreEqual("somepath", parameterReader.ConfigurationFilePath);
        }

        [TestMethod]
        public void ParameterReaderPropertlyValidatesAddressAndPort()
        {
            var parameterReader = new Messages.ParametersReader(parameters);
            Tuple<string, int> testTouple = parameterReader.Validate();
            Assert.AreEqual("234.234.234.234", testTouple.Item1);
            Assert.AreEqual(400, testTouple.Item2);
        }

        [TestMethod]
        public void ParameterReaderPropertlyValidatesInvalidAddress()
        {
            parameters[2] = "dasdasdqweqwdas";
            var parameterReader = new Messages.ParametersReader(parameters);
            Tuple<string, int> testTouple = parameterReader.Validate();
            Assert.AreEqual("127.0.0.1", testTouple.Item1);
        }

        [TestMethod]
        public void ParameterReaderPropertlyValidatesInvalidPort()
        {
            parameters[4] = "dasdasdqweqwdas";
            var parameterReader = new Messages.ParametersReader(parameters);
            Tuple<string, int> testTouple = parameterReader.Validate();
            Assert.AreEqual(8000, testTouple.Item2);
        
        }
        /// <summary>
        /// PROPER PATH
        /// </summary>
        [TestMethod]
        public void ParameterReaderPropertlyRefersToPathGivenByUserForGameMaster()
        {
            parameters[6] = "..//..//..//..//..//Messages//Messages//Messages.Test//MessagesTestXmlInput//GameMasterSettingsTest.xml";
            var parameterReader = new Messages.ParametersReader(parameters);
            parameterReader.ReadGameMasterInputSettings();
            Assert.AreEqual("Initial test game", parameterReader.ReadGameMasterSettings.GameDefinition.GameName);
        }

        [TestMethod]
        public void ParameterReaderPropertlyRefersToPathGivenByUserForCommunicationsServer()
        {
            parameters[6] = "..//..//..//..//..//Messages//Messages//Messages.Test//MessagesTestXmlInput//CommunicationServerSettingsTest.xml";
            var parameterReader = new Messages.ParametersReader(parameters);
            parameterReader.ReadCommunicationServerInputSettings();
            Assert.AreEqual(1000, (int)parameterReader.ReadCommunicationServerSettings.KeepAliveInterval);
        }

        [TestMethod]
        public void ParameterReaderPropertlyRefersToPathGivenByUserForPlayer()
        {
            parameters[6] = "..//..//..//..//..//Messages//Messages//Messages.Test//MessagesTestXmlInput//PlayerSettingsTest.xml";
            var parameterReader = new Messages.ParametersReader(parameters);
            parameterReader.ReadPlayerInputSettings();
            Assert.AreEqual(1000, (int)parameterReader.ReadPlayerSettings.KeepAliveInterval);
        }

        /// <summary>
        ///  FAKE PATH
        /// </summary>
        [TestMethod]
        public void ParameterReaderPropertlyRefersToFakePathGivenByUserForGameMaster()
        {
            parameters[6] = "somepath";
            var parameterReader = new Messages.ParametersReader(parameters);
            parameterReader.ReadGameMasterInputSettings();
            Assert.AreEqual("Initial game", parameterReader.ReadGameMasterSettings.GameDefinition.GameName);
        }

        [TestMethod]
        public void ParameterReaderPropertlyRefersToFakePathGivenByUserForCommunicationsServer()
        {
            parameters[6] = "somepath";
            var parameterReader = new Messages.ParametersReader(parameters);
            parameterReader.ReadCommunicationServerInputSettings();
            Assert.AreEqual(500, (int)parameterReader.ReadCommunicationServerSettings.KeepAliveInterval);
        }

        [TestMethod]
        public void ParameterReaderPropertlyRefersToFakePathGivenByUserForPlayer()
        {
            parameters[6] = "somepath";
            var parameterReader = new Messages.ParametersReader(parameters);
            parameterReader.ReadPlayerInputSettings();
            Assert.AreEqual(500, (int)parameterReader.ReadPlayerSettings.KeepAliveInterval);
        }
        /// <summary>
        /// wrong file
        /// </summary>
        [TestMethod]
        public void ParameterReaderPropertlyRefersToWrongFilePathGivenByUserForGameMaster()
        {
            parameters[6] = "..//..//..//..//..//Messages//Messages//Messages.Test//MessagesTestXmlInput//PlayerSettingsTest.xml";
            var parameterReader = new Messages.ParametersReader(parameters);
            parameterReader.ReadGameMasterInputSettings();
            Assert.AreEqual("Initial game", parameterReader.ReadGameMasterSettings.GameDefinition.GameName);
        }

        [TestMethod]
        public void ParameterReaderPropertlyRefersToWrongFilePathGivenByUserForCommunicationsServer()
        {
            parameters[6] = "..//..//..//..//..//Messages//Messages//Messages.Test//MessagesTestXmlInput//PlayerSettingsTest.xml";
            var parameterReader = new Messages.ParametersReader(parameters);
            parameterReader.ReadCommunicationServerInputSettings();
            Assert.AreEqual(500, (int)parameterReader.ReadCommunicationServerSettings.KeepAliveInterval);
        }

        [TestMethod]
        public void ParameterReaderPropertlyRefersToWrongFilePathGivenByUserForPlayer()
        {
            parameters[6] = "..//..//..//..//..//Messages//Messages//Messages.Test//MessagesTestXmlInput//GameMasterSettingsTest.xml";
            var parameterReader = new Messages.ParametersReader(parameters);
            parameterReader.ReadPlayerInputSettings();
            Assert.AreEqual(500, (int)parameterReader.ReadPlayerSettings.KeepAliveInterval);
        }
    }
}
