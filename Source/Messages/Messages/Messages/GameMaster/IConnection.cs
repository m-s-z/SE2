using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xsd2;

namespace Messages
{
    namespace GameMaster
    {
        public interface IConnection
        {
            void Connect();
            void Disconnect();
            void InitiateThreads();
            void SendMessage(string message);
            List<string> MessageList { get; }
            void AddPlayer(Guid guid, ulong id, Xsd2.TeamColour team, Xsd2.PlayerType type);
            IGameState GameState { get; set; }
            void DelayMessage(Messages.XmlHandling.Serializer serializer, uint delay);
           // Messages.ParametersReader Parameters { get; set; }
        }
    }
}
