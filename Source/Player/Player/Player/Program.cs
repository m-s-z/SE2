using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Messages.XmlHandling;

namespace Player
{
    public class Program
    {
        private static Messages.ParametersReader _parametersReader;
        static void Main(string[] args)
        {
            _parametersReader = new Messages.ParametersReader(args);
            Tuple<string, int> endPoint = _parametersReader.Validate();
            foreach(string s in args)
            {
                Console.WriteLine("Arg: {0}", s);
            }
            _parametersReader.ReadPlayerInputSettings();
            Connection connection = new Connection(endPoint.Item1, endPoint.Item2, _parametersReader.GameName, _parametersReader);
            try
            {    
                connection.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} has encountered exception: {1} while trying to connect", nameof(Program), e.Message);
                Console.WriteLine("{0} will now exit", nameof(Program));
                return;
            }
            var joinGameMessage = new Xsd2.GetGames();
            var serializer = new Serializer(joinGameMessage);
            string joinGameMsg = serializer.Serialize();
            Console.WriteLine(joinGameMsg);
            connection.SendMessage(joinGameMsg);

            Task.WaitAll();
            Console.ReadLine();
            connection.Disconnect();
            Cleanup();
        }

        static void Cleanup()
        {
            Task.WaitAll();
            Console.WriteLine("{0} will now exit", nameof(Program));
        }
    }
}
