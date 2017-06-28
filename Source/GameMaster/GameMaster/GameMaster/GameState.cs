using Messages.GameMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using Xsd2;
using System.Timers;

namespace GameMaster
{
    public class GameState : Messages.GameMaster.IGameState
    {
        private Timer _spawnPieceTimer;
        private ulong _gameId;
        private bool _gameStarted;
        private bool _gameFinished;
        private string _name;
        private List<IPlayer> _redPlayers;
        private List<IPlayer> _bluePlayers;
        private ulong _redTeamMax;
        private ulong _blueTeamMax;
        private int _redGoalsFinished;
        private int _blueGoalsFinished;
        private int _redNumberOfGoals;
        private int _blueNumberOfGoals;
        private List<PieceInfo> _pieces = new List<PieceInfo>();
        private List<List<Field>> _boardList;

        private Configuration.GoalField[] _goals;
        private double _shamProbability;
        private uint _placingNewPiecesFrequency;
        private uint _initialNumberOfPiecesField;
        private int _boardWidth;
        private int _taskAreaLength;
        private int _goalAreaLength;
        private ulong _pieceIdCounter;
        private Configuration.GameMasterSettingsActionCosts _actionCosts;
        public int RedNumberOfGoals { get { return _redNumberOfGoals; } }
        public int BlueNumberOfGoals { get { return _blueNumberOfGoals; } }
        public int RedGoalsFinished { get { return _redGoalsFinished; } set { _redGoalsFinished = value; } }
        public int BlueGoalsFinished { get { return _blueGoalsFinished; } set { _blueGoalsFinished = value; } }
        public List<PieceInfo> Pieces { get { return _pieces; } }
        public int BoardWidth { get { return _boardWidth; } }
        public int TaskAreaLength { get { return _taskAreaLength; } }
        public int GoalAreaLength { get { return _goalAreaLength; } }
        public string Name { get { return _name; } set { _name = value; } }
        public ulong GameId { get { return _gameId; } set { _gameId = value; } }
        public List<IPlayer> RedPlayers { get { return _redPlayers; } set { _redPlayers = value; } }
        public List<IPlayer> BluePlayers { get { return _bluePlayers; } set { _bluePlayers = value; } }
        public ulong RedTeamMax { get { return _redTeamMax; } set { _redTeamMax = value; } }
        public ulong BlueTeamMax { get { return _blueTeamMax; } set { _blueTeamMax = value; } }
        public bool GameStarted { get { return _gameStarted; } set { _gameStarted = value; } }
        public bool GameFinished { get { return _gameFinished; } set { _gameFinished = value; } }
        public Configuration.GameMasterSettingsActionCosts ActionCosts { get { return _actionCosts; } set { _actionCosts = value; } }
        public uint InitialNumberOfPiecesField { get { return _initialNumberOfPiecesField; } set { _initialNumberOfPiecesField = value; } }
        public List<List<Field>> BoardList { get { return _boardList; } set { _boardList = value; } }
        public GameState(Messages.ParametersReader reader)
        {

            _pieceIdCounter = 0;
            _redPlayers = new List<IPlayer>();
            _bluePlayers = new List<IPlayer>();
            _name = reader.GameName;
            var settings = reader.ReadGameMasterSettings;
            _redTeamMax = _blueTeamMax = ulong.Parse(settings.GameDefinition.NumberOfPlayersPerTeam);
            _goals = settings.GameDefinition.Goals;
            _shamProbability = settings.GameDefinition.ShamProbability;
            _placingNewPiecesFrequency = settings.GameDefinition.PlacingNewPiecesFrequency;
            _initialNumberOfPiecesField = settings.GameDefinition.InitialNumberOfPieces;
            _boardWidth = Convert.ToInt16(settings.GameDefinition.BoardWidth);
            _taskAreaLength = Convert.ToInt16(settings.GameDefinition.TaskAreaLength);
            _goalAreaLength = Convert.ToInt16(settings.GameDefinition.GoalAreaLength);
            _actionCosts = settings.ActionCosts;
            foreach (var g in _goals)
            {
                if (g.team == Configuration.TeamColour.red)
                {
                    _redNumberOfGoals++;
                }
                else
                {
                    _blueNumberOfGoals++;
                }

            }

            _spawnPieceTimer = new Timer(_placingNewPiecesFrequency);
            _spawnPieceTimer.Elapsed += new ElapsedEventHandler(SpawnPiece);
        }
        public GameState(string[] parameters)
        {
            _gameFinished = false;
            _redPlayers = new List<IPlayer>();
            _bluePlayers = new List<IPlayer>();
            _name = parameters[0];
            _redTeamMax = _blueTeamMax = ulong.Parse(parameters[1]);
        }
        public GameState(string[] parameters, object placeholder)
        {
            _gameFinished = false;
            _redPlayers = new List<IPlayer>();
            _bluePlayers = new List<IPlayer>();
            _name = parameters[0];
            _redTeamMax = _blueTeamMax = ulong.Parse(parameters[1]);
            int.TryParse(parameters[2], out _taskAreaLength);
            int.TryParse(parameters[3], out _goalAreaLength);
        }
        public void StartSpawningPieces()
        {
            _spawnPieceTimer.Start();
        }
        public void AddPlayer(IPlayer player)
        {
            var teamColor = player.TeamColour;
            if (teamColor == Xsd2.TeamColour.red && (ulong)RedPlayers.Count < this.RedTeamMax)
            {
                this.RedPlayers.Add(player);
            }
            else if (teamColor == Xsd2.TeamColour.blue && (ulong)BluePlayers.Count < this.BlueTeamMax)
            {
                this.BluePlayers.Add(player);
            }
        }

        public void RemovePlayer(ulong id)
        {
            try
            {
                RedPlayers.RemoveAll(c => c.Id == id);
                BluePlayers.RemoveAll(c => c.Id == id);
            }
            catch (Exception)
            {
                Console.WriteLine("RedPlayers count : {0}", RedPlayers.Count);
                Console.WriteLine("BluePlayers count : {0}", BluePlayers.Count);
            }
        }
        //just example algorithm for calculating manhattan distance
        //whenever piece is added to the board, a new PieceInfo should be created with location of the piece
        //also when a piece is added or placed outside of taskarea, manhattan distanced should be calculated again
        public void CalculateManhattanDistance()
        {
            for (int i = 0; i < _boardList.Count; i++)
            {
                for (int j = 0; j < _boardList[i].Count; j++)
                {
                    if (_boardList[i][j] is Xsd2.TaskField)
                    {
                        uint min = int.MaxValue;
                        foreach (PieceInfo piece in _pieces)
                        {
                            if (piece.Piece.playerId == 0)
                            {
                                uint distance = (uint)(Math.Abs((int)piece.Location.x - (int)_boardList[i][j].x)
                                    + Math.Abs((int)piece.Location.y - (int)_boardList[i][j].y));
                                if (distance < min)
                                {
                                    min = distance;
                                }
                            }
                        }
                        if(min == int.MaxValue)
                        {
                            (_boardList[i][j] as Xsd2.TaskField).distanceToPiece = -1;
                        }
                        else
                        {
                            (_boardList[i][j] as Xsd2.TaskField).distanceToPiece = (int)min;
                        }
                    }
                }
            }
        }
        public void CreateGame()
        {

            _boardList = SetInitialBoard(_goalAreaLength, _taskAreaLength, _boardWidth);
            SetInitialGoals(_goals);
            SetInitialPieces(_initialNumberOfPiecesField, ref _pieceIdCounter, _boardList, _goalAreaLength, _taskAreaLength);
            CalculateManhattanDistance();
            foreach (var fieldList in _boardList)
            {
                foreach (var field in fieldList)
                {
                    if (field is GoalField)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write("%");
                    }
                }
                Console.WriteLine();
            }
        }

        public void SetPlayerLocations()
        {
            int blueTeamRow = _goalAreaLength - 1;
            for (int i = 0, j = 0; i < _redPlayers.Count; i++, j++)
            {
                _redPlayers[i].Location = new Location() { x = (uint)j, y = (uint)blueTeamRow };
                if (j >= _boardWidth)
                {
                    j = 0;
                    blueTeamRow--;
                }
            }
            int redTeamRow = _goalAreaLength + _taskAreaLength;
            for (int i = 0, j = 0; i < _bluePlayers.Count; i++, j++)
            {
                _bluePlayers[i].Location = new Location() { x = (uint)j, y = (uint)redTeamRow };
                if (j >= _boardWidth)
                {
                    j = 0;
                    redTeamRow++;
                }
            }
        }
        //for tests now
        public void AddPiece(Xsd2.Piece piece, Xsd2.Location location)
        {
            _pieces.Add(new PieceInfo(location, piece));
        }

        public void SpawnPiece(object sender, ElapsedEventArgs e)
        {
            if(GameFinished)
            {
                _spawnPieceTimer.Stop();
                _spawnPieceTimer.Elapsed -= new ElapsedEventHandler(SpawnPiece);
            }
            else
            {
                Random rnd = new Random();

                int x = rnd.Next(_boardWidth);
                int y = rnd.Next(_goalAreaLength, _goalAreaLength + _taskAreaLength);
                var currentField = ((TaskField)_boardList[x][y]);
                //if theres no piece in this location
                if (_pieces.FirstOrDefault(p => p.Location.x == x && p.Location.y == y) == null)
                {
                    _pieceIdCounter++;
                    currentField.pieceId = _pieceIdCounter;
                    currentField.pieceIdSpecified = true;
                    var piece = new Xsd2.Piece() { id = _pieceIdCounter };
                    piece.type = (rnd.NextDouble() <= _shamProbability) ? PieceType.sham : PieceType.normal;
                    AddPiece(piece, new Location() { x = (uint)x, y = (uint)y });
                }
            }
        }

        public void CreateNextPiece(ref ulong pieceIdCounter, int goalAreaLength, int taskAreaLength, List<List<Field>> boardList)
        {
            int boardWidth = boardList.Count;
            Random rnd = new Random();
            while (true)
            {

                int x = rnd.Next(boardWidth);
                int y = rnd.Next(goalAreaLength, goalAreaLength + taskAreaLength);
                if (((TaskField)boardList[x][y]).pieceIdSpecified == false)
                {
                    ((TaskField)boardList[x][y]).pieceId = pieceIdCounter++;
                    ((TaskField)boardList[x][y]).pieceIdSpecified = true;
                    var piece = new Xsd2.Piece() { id = pieceIdCounter };
                    piece.type = (rnd.NextDouble() <= _shamProbability) ? PieceType.sham : PieceType.normal;
                    AddPiece(piece, new Location() { x = (uint)x, y = (uint)y });
                    break;
                }
            }

        }


        public void SetInitialPieces(uint initialNumberOfPiecesField, ref ulong pieceIdCounter, List<List<Field>> boardList, int goalAreaLength, int taskAreaLength)
        {
            for (int i = 0; i < initialNumberOfPiecesField; i++)
                CreateNextPiece(ref pieceIdCounter, goalAreaLength, taskAreaLength, boardList);
        }

        public void SetInitialGoals(Configuration.GoalField[] goals)
        {
            foreach (var r in goals)
            {
                try
                {
                    ((GoalField)_boardList[(int)r.x][(int)r.y]).type = GoalFieldType.goal;
                }
                catch (Exception e)
                {
                    Console.WriteLine("There is a mistake in the goal fields positioning. As the point x:" + r.x + " y:" + r.y + " is not in goal area");
                    throw new NullReferenceException();
                }
            }
            _redGoalsFinished = 0;
            _blueGoalsFinished = 0;
        }

        public List<List<Field>> SetInitialBoard(int goalAreaLength, int taskAreaLength, int boardWidth)
        {
            List<List<Field>> boardList = new List<List<Field>>();
            for (int i = 0; i < boardWidth; i++)
            {
                boardList.Add(new List<Field>());

                for (int j = 0; j < taskAreaLength + 2 * goalAreaLength; j++)
                {
                    if (j >= goalAreaLength && j < goalAreaLength + taskAreaLength)
                    {
                        TaskField tF = new TaskField();
                        tF.x = (uint)i;
                        tF.y = (uint)j;
                        tF.pieceIdSpecified = false;
                        boardList[i].Add(tF);
                    }
                    else
                    {
                        GoalField gF = new GoalField();
                        gF.x = (uint)i;
                        gF.y = (uint)j;
                        gF.type = GoalFieldType.nongoal;
                        gF.team = (i < goalAreaLength) ? TeamColour.blue : TeamColour.red;
                        boardList[i].Add(gF);
                    }
                }
            }
            return boardList;
        }
    }
}
