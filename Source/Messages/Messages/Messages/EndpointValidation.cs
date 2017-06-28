using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Messages
{
    public class EndpointValidation
    {

        public static Tuple<String,int> validate ( string ipAddress,string port)
        { 
            int numericPort;

            try
            {
                numericPort = Convert.ToInt32(port);
                if (!Regex.Match(ipAddress, @"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)").Success)
                    throw new Exception("ip did not match");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                numericPort = 8000;
                ipAddress = "127.0.0.1";
                Console.WriteLine("Setting default ip address at: 127.0.0.1 and port to: 8000");
            }
            return new Tuple<string,int>(ipAddress,numericPort);
        }
    }
}
