﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.GameMaster
{
    public interface IGameMasterHandler
    {
        void HandleOnGameMaster(Messages.GameMaster.IConnection connection);
    }
}
