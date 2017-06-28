using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Configuration;

namespace Messages
{
    public class ParametersReader
    {
        private string _port;
        private string _adress;
        private string _configurationFilePath;
        private string _gameName;
        private string _team;
        private string _role;
        private Configuration.GameMasterSettings _readGameMasterSettings;
        private Configuration.PlayerSettings _readPlayerSettings;
        private Configuration.CommunicationServerSettings _readCommunicationServerSettings;


        #region Getters
        public string Port
        {
            get
            {
                return _port;
            }

        }

        public string Adress
        {
            get
            {
                return _adress;
            }

        }

        public string ConfigurationFilePath
        {
            get
            {
                return _configurationFilePath;
            }

        }

        public string GameName
        {
            get
            {
                return _gameName;
            }

        }

        public string Team
        {
            get
            {
                return _team;
            }

        }

        public string Role
        {
            get
            {
                return _role;
            }

        }

        public GameMasterSettings ReadGameMasterSettings
        {
            get
            {
                return _readGameMasterSettings;
            }

        }

        public PlayerSettings ReadPlayerSettings
        {
            get
            {
                return _readPlayerSettings;
            }

        }

        public CommunicationServerSettings ReadCommunicationServerSettings
        {
            get
            {
                return _readCommunicationServerSettings;
            }

        }


        #endregion

        public ParametersReader(string[] args)
        {
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].Equals("--address"))
                {
                    if (!CheckIfParameterValue(args[i+1]))
                    {
                        Console.WriteLine("Value of {0} is wrong", args[i]);
                        return;
                    }
                    _adress = args[i + 1];
                    Console.WriteLine("IP adress: {0}", args[i + 1]);
                }
                if (args[i].Equals("--port"))
                {
                    if (!CheckIfParameterValue(args[i + 1]))
                    {
                        Console.WriteLine("Value of {0} is wrong", args[i]);
                        return;
                    }
                    _port = args[i + 1];
                    Console.WriteLine("Port is: {0}", args[i + 1]);
                }
                if (args[i].Equals("--conf"))
                {
                    if (!CheckIfParameterValue(args[i + 1]))
                    {
                        Console.WriteLine("Value of {0} is wrong", args[i]);
                        return;
                    }
                    _configurationFilePath = args[i + 1];
                    Console.WriteLine("Path to config is: {0}", args[i + 1]);
                }
                if (args[i].Equals("--game"))
                {
                    if (!CheckIfParameterValue(args[i + 1]))
                    {
                        Console.WriteLine("Value of {0} is wrong", args[i]);
                        return;
                    }
                    _gameName = args[i + 1];
                    Console.WriteLine("Game name is: {0}", args[i + 1]);
                }
                if (args[i].Equals("--team"))
                {
                    if (!CheckIfParameterValue(args[i + 1]))
                    {
                        Console.WriteLine("Value of {0} is wrong", args[i]);
                        return;
                    }
                    _team = args[i + 1];
                    Console.WriteLine("Team color is: {0}", args[i + 1]);
                }
                if (args[i].Equals("--role"))
                {
                    if (!CheckIfParameterValue(args[i + 1]))
                    {
                        Console.WriteLine("Value of {0} is wrong", args[i]);
                        return;
                    }
                    _role = args[i + 1];
                    Console.WriteLine("Prefered role is: {0}", args[i + 1]);
                }
            }

        }

        private static bool CheckIfParameterValue(string arg)
        {
            if (arg[0].Equals('-'))
            {
                Console.WriteLine("Problem with argument {0}", arg);
                return false;
            }
            if (arg == null)
            {
                return false;
            }
            return true;
        }

        public Tuple<String, int> Validate()
        {
            int numericPort;

            try
            {
                numericPort = Convert.ToInt32(Port);
                if (!Regex.Match(Adress, @"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)").Success)
                    throw new Exception("ip did not match");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _port = "8000";
                _adress = "127.0.0.1";
                Console.WriteLine("Setting default ip address at: 127.0.0.1 and port to: 8000");
            }
            return new Tuple<string, int>(Adress, Convert.ToInt32(Port));
        }

        public void ReadGameMasterInputSettings()
        {
            string PATH_DEFAULT = "..//..//..//..//..//GameMaster//GameMaster//GameMaster//GameMasterSettingsDefault.xml";
            var serializer = new XmlSerializer(typeof(Configuration.GameMasterSettings));
            StreamReader file;
            if (ConfigurationFilePath == null)
            {
                _configurationFilePath = PATH_DEFAULT;
            }
            try
            {
                file = new StreamReader(ConfigurationFilePath);
                _readGameMasterSettings = (Configuration.GameMasterSettings)serializer.Deserialize(file);
                _gameName = _readGameMasterSettings.GameDefinition.GameName;
            }
            catch(Exception e)
            {
                Console.WriteLine("Exepction {0} occured while reading file", e.ToString());
                file = new StreamReader(PATH_DEFAULT);
                _readGameMasterSettings = (Configuration.GameMasterSettings)serializer.Deserialize(file);
                _gameName = _readGameMasterSettings.GameDefinition.GameName;
            }
        }

        public void ReadPlayerInputSettings()
        {
            string PATH_DEFAULT = "..//..//..//..//..//Player//Player//Player//PlayerSettingsDefault.xml";
            var serializer = new XmlSerializer(typeof(Configuration.PlayerSettings));
            StreamReader file;
            if (ConfigurationFilePath == null)
            {
                _configurationFilePath = PATH_DEFAULT;
            }
            try
            {
                file = new StreamReader(ConfigurationFilePath);
                _readPlayerSettings = (Configuration.PlayerSettings)serializer.Deserialize(file);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exepction {0} occured while reading file", e.ToString());
                file = new StreamReader(PATH_DEFAULT);
                _readPlayerSettings = (Configuration.PlayerSettings)serializer.Deserialize(file);
            }
        }

        public void ReadCommunicationServerInputSettings()
        {
            string PATH_DEFAULT = "..//..//..//..//..//CommunicationServer//CommunicationServer//CommunicationServer//CommunicationServerSettingsDefault.xml";
            var serializer = new XmlSerializer(typeof(Configuration.CommunicationServerSettings));
            StreamReader file;
            if (ConfigurationFilePath == null)
            {
                _configurationFilePath = PATH_DEFAULT;
            }
            try
            {
                file = new StreamReader(ConfigurationFilePath);
                _readCommunicationServerSettings = (Configuration.CommunicationServerSettings)serializer.Deserialize(file);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exepction {0} occured while reading file", e.ToString());
                file = new StreamReader(PATH_DEFAULT);
                _readCommunicationServerSettings = (Configuration.CommunicationServerSettings)serializer.Deserialize(file);
            }
        }
    }
}
