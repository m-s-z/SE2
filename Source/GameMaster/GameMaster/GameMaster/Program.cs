using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Messages;
using Messages.XmlHandling;
namespace GameMaster
{
    class Program
    {
        private static Messages.ParametersReader _parametersReader;
        static void Main(string[] args)
        {
            foreach(var s in args)
            {
                Console.WriteLine("args: {0}", s);
            }
            _parametersReader = new Messages.ParametersReader(args);
            Tuple<string, int> endPoint = _parametersReader.Validate();
            _parametersReader.ReadGameMasterInputSettings();
            var gameState = new GameState(_parametersReader);
            Console.WriteLine(_parametersReader.ConfigurationFilePath);
            Connection connection = new Connection(endPoint.Item1, endPoint.Item2, _parametersReader);
            try
            {
                connection.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} has encountered exception: {1} while trying to connect", nameof(Program), e.Message);
                Console.WriteLine("{0} will now exit", nameof(Program));
            }
            
            var gameMessage = new Xsd2.RegisterGame(new Xsd2.GameInfo(connection.GameState));
            var serializer = new Serializer(gameMessage);
            string gameMsg = serializer.Serialize();
            Console.WriteLine(gameMsg);
            connection.SendMessage(gameMsg);

            Task.WaitAll();
            Cleanup();
            Console.ReadLine();
        }

        static void Cleanup()
        {
            Task.WaitAll();
            Console.WriteLine("{0} will now exit", nameof(Program));
        }

    }
}

