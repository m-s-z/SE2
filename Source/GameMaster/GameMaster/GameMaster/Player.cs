using Messages.GameMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xsd2;

namespace GameMaster
{
    public class Player : IPlayer
    {
        public Guid Guid { get; set; }
        public ulong Id { get; set; }
        public Xsd2.Location Location { get; set; }
        public PlayerType PlayerType
        {
            get;
            set;
        }

        public TeamColour TeamColour
        {
            get;
            set;
        }
        public Player(Guid guid, ulong id, PlayerType type, TeamColour team)
        {
            this.Guid = guid;
            this.Id = id;
            this.PlayerType = type;
            this.TeamColour = team;
        }
    }
}
