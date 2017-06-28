using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xsd2
{
    public partial class Player
    {
        public Player() { }
        public Player(ulong id, TeamColour teamColour, PlayerType type)
        {
            this.id = id;
            this.team = teamColour;
            this.type = type;
        }
    }
}
