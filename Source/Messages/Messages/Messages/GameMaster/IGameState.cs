using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xsd2;

namespace Messages.GameMaster
{
    public interface IGameState
    {
        string Name { get; set; }
        ulong GameId { get; set; }
        List<IPlayer> BluePlayers { get; set; }
        List<IPlayer> RedPlayers { get; set; }
        bool GameStarted { get; set; }
        bool GameFinished { get; set; }
        int RedGoalsFinished { get; set; }
        int BlueGoalsFinished { get; set; }
        int RedNumberOfGoals { get;  }
        int BlueNumberOfGoals { get;  }
        ulong BlueTeamMax { get; set; }
        ulong RedTeamMax { get; set; }
        uint InitialNumberOfPiecesField { get; set; }
        List<List<Field>> BoardList { get; set; }
        List<PieceInfo> Pieces { get; }
        int BoardWidth { get; }
        int TaskAreaLength { get; }
        int GoalAreaLength { get; }
        Configuration.GameMasterSettingsActionCosts ActionCosts { get; set; }
        void SetPlayerLocations();
        void CreateGame();
        void CalculateManhattanDistance();
        void AddPlayer(IPlayer player);
        void RemovePlayer(ulong id);
        void StartSpawningPieces();
    }
}
