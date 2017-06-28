using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    namespace CommunicationServer
    {
        public interface IConnection
        {
            void StartServer();
            void StopServer();
            Dictionary<ulong, IGameState> Games { get; set; }
            List<IReceiver> Receivers { get; set; }
            ulong PlayerCount { get; set; }
            object PlayerCountLock { get; }
            ulong GameCount { get; set; }
            object GameCountLock { get; }
            IGameState CreateGame(IReceiver gameMaster, Xsd2.RegisterGame msg);
        }
    }
}
