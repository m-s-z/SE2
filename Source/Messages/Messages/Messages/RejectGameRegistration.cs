using Messages.GameMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xsd2
{
    public partial class RejectGameRegistration : IGameMasterHandler
    {
        public void HandleOnGameMaster(Messages.GameMaster.IConnection connection)
        {            

            connection.Disconnect();
        }
    }
}
