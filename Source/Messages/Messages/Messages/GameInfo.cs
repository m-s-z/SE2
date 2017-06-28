using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xsd2
{
    public partial class GameInfo
    {
        public GameInfo() { }
        public GameInfo(Messages.CommunicationServer.IGameState gameState)
        {
            this.blueTeamPlayers = gameState.BlueTeamMax;
            this.redTeamPlayers = gameState.RedTeamMax;
            this.gameName = gameState.GameName;
        }
        public GameInfo(Messages.GameMaster.IGameState gameState)
        {
            this.blueTeamPlayers = gameState.BlueTeamMax;
            this.redTeamPlayers = gameState.RedTeamMax;
            this.gameName = gameState.Name;
        }
    }
}
