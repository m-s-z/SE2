using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.GameMaster
{
    public interface IPlayer
    {
        Guid Guid { get; set; }
        ulong Id { get; set; }
        Xsd2.TeamColour TeamColour { get; set; }
        Xsd2.PlayerType PlayerType { get; set; }
        Xsd2.Location Location { get; set; }
    }
}
