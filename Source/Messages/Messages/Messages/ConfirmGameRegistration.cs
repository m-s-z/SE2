using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages.GameMaster;

namespace Xsd2
{
    public partial class ConfirmGameRegistration : IGameMasterHandler
    {
        public ConfirmGameRegistration() { }
        public ConfirmGameRegistration(ulong gameId)
        {
            this.gameId = gameId;
        }
        // For now it does nothing
        public void HandleOnGameMaster(IConnection connection)
        {
            connection.GameState.CreateGame();
        }
    }
}
