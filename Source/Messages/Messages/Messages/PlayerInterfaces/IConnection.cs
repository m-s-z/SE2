using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.PlayerInterfaces
{
    public interface IConnection
    {
        void Connect();
        void Disconnect();
        void InitiateThreads();
        void SendMessage(string message);
        List<string> MessageList { get; }
        Guid Guid { get; set; }
        string GameName { get; set; }
        bool GameFinished { get; set; }
        Xsd2.TeamColour Team { get; set; }
        Xsd2.PlayerType Role { get; set; }
        IPlayerLogic Logic { get; set; }
        ulong GameId { get; set; }
    }
}
