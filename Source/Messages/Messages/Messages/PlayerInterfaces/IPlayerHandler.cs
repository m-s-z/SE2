using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.PlayerInterfaces
{
    public interface IPlayerHandler
    {
        void HandleOnPlayer(Messages.PlayerInterfaces.IConnection connection);
    }
}
