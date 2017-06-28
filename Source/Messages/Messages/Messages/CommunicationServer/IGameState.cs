using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.CommunicationServer
{
    public interface IGameState
    {
        List<IReceiver> Players { get; set; }
        IReceiver GameMaster { get; set; }
        ulong GameId { get; set; }
        string GameName { get; set; }
        ulong RedTeamCount { get; set; }
        ulong RedTeamMax { get; set; }
        ulong BlueTeamCount { get; set; }
        ulong BlueTeamMax { get; set; }
        object RedTeamLock { get; }
        object BlueTeamLock { get; }
    }
}
