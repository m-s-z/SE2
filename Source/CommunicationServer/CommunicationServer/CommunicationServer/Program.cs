using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer
{
    class Program
    {
        private static Messages.ParametersReader _parametersReader;

        static void Main(string[] args)
        {
            _parametersReader = new Messages.ParametersReader(args);
            try {
                var connection = new Connection(Convert.ToInt32(_parametersReader.Port));
                connection.StartServer();
                Console.WriteLine("[Communiation server] accepting requests at {0}.", connection.GetEndpoint());
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} has encountered exception: {1} while trying to connect", nameof(Program), e.Message);
                Console.WriteLine("{0} will now exit", nameof(Program));
            }
            
        }
    }
}
