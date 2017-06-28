using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    namespace CommunicationServer
    {
        public interface IReceiver
        {
            bool IsGameMaster{ get;set; }
            void SendMessage(string message);
            ITcpClient GetClient();
            bool Status { get; set; }
            List<string> MessageList{ get; }
            ulong Id { get; set; }
            ulong GameId { get; set; }
            string GameName { get; set; }
        }
    }
}
