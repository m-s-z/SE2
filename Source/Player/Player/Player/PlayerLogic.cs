using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xsd2;

namespace Player
{
    public class PlayerLogic : Messages.PlayerInterfaces.IPlayerLogic
    {
        private Messages.PlayerInterfaces.IConnection _connection;
        private ulong _id;
        private ulong _gameId;
        private Guid _guid;
        private int _lastActionReminder; // 0 - move, 1 - discover, 2 - pick up, 3 - test, 4 - place
        private Piece _pieceTaken;
        private TeamColour _colour;
        private PlayerType _type;
        private bool _hasStartedOnTop;
        private int _boardWidth;
        private int _tasksHeight;
        private int _goalsHeight;
        private int _currentX, _currentY;
        private List<List<Field>> _board;
        private int _decisionNumber;
        private MoveType _moveToGoalArea;
        private MoveType _moveFromGoalArea;

        private int _minBoardY, _maxBoardY;
        private int _moveDiscoverCount = 0;

        private bool _goalSearching;
        private MoveType _activeDirection;
        private MoveType _previousMoveDirection;
        #region GettersAndSetters

        public int DecisionNumber
        {
            get
            {
                return _decisionNumber;
            }

            set
            {
                _decisionNumber = value;
            }
        }

        public List<List<Field>> Board { get { return _board; } set { _board = value; } }
        public int CurrentX { get { return _currentX; } set { _currentX = value; } }
        public int CurrentY { get { return _currentY; } set { _currentY = value; } }
        public int TasksHeight { get { return _tasksHeight; } set { _tasksHeight = value; } }
        public int GoalsHeight { get { return _goalsHeight; } set { _goalsHeight = value; } }
        public int BoardWidth { get { return _boardWidth; } set { _boardWidth = value; } }
        public bool HasStartedOnTop { get { return _hasStartedOnTop; } set { _hasStartedOnTop = value; } }

        #endregion

        public PlayerLogic(Messages.PlayerInterfaces.IConnection connection)
        {
            _connection = connection;
        }
        public PlayerLogic()
        {
        }

        public List<List<Field>> CreateInitialBoard(int goalsHeight, int tasksHeight, int boardWidth)
        {
            List<List<Field>> board = new List<List<Field>>();
            for (int i = 0; i < boardWidth; i++)
            {
                board.Add(new List<Field>());
                for (int j = 0; j < tasksHeight + 2 * goalsHeight; j++)
                {
                    if (j >= goalsHeight && j < goalsHeight + tasksHeight)
                    {
                        TaskField tF = new TaskField();
                        tF.x = (uint)i;
                        tF.y = (uint)j;
                        tF.pieceIdSpecified = false;
                        tF.distanceToPiece = -3;
                        board[i].Add(tF);
                    }
                    else
                    {
                        GoalField gF = new GoalField();
                        gF.x = (uint)i;
                        gF.y = (uint)j;
                        gF.type = GoalFieldType.unknown;
                        gF.team = (i < goalsHeight) ? TeamColour.blue : TeamColour.red;
                        board[i].Add(gF);
                    }
                }
            }
            return board;
        }

        public GameMessage AnswerForGameMessage(Messages.PlayerInterfaces.IConnection connection, Game gameMessage)
        {

            _gameId = connection.GameId;
            _id = gameMessage.playerId;
            _guid = connection.Guid;
            _colour = gameMessage.Players.First<Xsd2.Player>(p => p.id == _id).team;
            _type = gameMessage.Players.First<Xsd2.Player>(p => p.id == _id).type;
            _boardWidth = (int)gameMessage.Board.width;
            _tasksHeight = (int)gameMessage.Board.tasksHeight;
            _goalsHeight = (int)gameMessage.Board.goalsHeight;
            _currentX = (int)gameMessage.PlayerLocation.x;
            _currentY = (int)gameMessage.PlayerLocation.y;
            _hasStartedOnTop = _currentY >= _goalsHeight + _tasksHeight;
            _goalSearching = false;
            _board = CreateInitialBoard(_goalsHeight, _tasksHeight, _boardWidth);
            return CreateFirstMove(connection);

        }

        public GameMessage CreateFirstMove(Messages.PlayerInterfaces.IConnection connection)
        {
            if (_hasStartedOnTop)
            {
                _moveFromGoalArea = MoveType.down;
                _moveToGoalArea = MoveType.up;
                _minBoardY = _goalsHeight;
                _maxBoardY = _goalsHeight * 2 + _tasksHeight;
            }
            else
            {
                _moveFromGoalArea = MoveType.up;
                _moveToGoalArea = MoveType.down;
                _minBoardY = 0;
                _maxBoardY = _goalsHeight + _tasksHeight;
            }
            _lastActionReminder = 0;
            return new Move(connection.Guid, _moveFromGoalArea);
        }

        public void SetReceivedData(TaskField[] taskFields, GoalField[] goalFields, bool gameFinished, Xsd2.Piece[] pieces, Location location)
        {
            _connection.GameFinished = gameFinished;
            if (taskFields != null)
            {
                foreach (var taskField in taskFields)
                {
                    _board[(int)taskField.x][(int)taskField.y] = taskField;
                }
            }
            if (goalFields != null)
            {
                foreach (var goalField in goalFields)
                {
                    _board[(int)goalField.x][(int)goalField.y] = goalField;
                }
            }

            //TO DO:Go through list of fields, get ids of pieces to get their locations, then assign location of pieces to list of PieceInfo
            //foreach (var field in _board)
            //{//}
            if (pieces != null)
            {
                _pieceTaken = pieces.FirstOrDefault(p => p.playerId == _id);
            }
            if (location != null)
            {
                _currentX = (int)location.x;
                _currentY = (int)location.y;
            }
        }

        private bool IsInTheGoalField()
        {
            return _hasStartedOnTop ? (_currentY >= _goalsHeight + _tasksHeight) : (_currentY < _goalsHeight);
        }


        public GameMessage ChooseNextMessage(Messages.PlayerInterfaces.IConnection connection, Data gameMessage)
        {
            if (IsInTheGoalField())
            {
                if (_lastActionReminder == 4)
                {
                    if (((GoalField)_board[_currentX][_currentY]).type == GoalFieldType.goal)
                    {
                        _goalSearching = false;
                        _lastActionReminder = 0;
                        _previousMoveDirection = _moveFromGoalArea;
                        return new Move(_guid, _moveFromGoalArea);
                    }
                    else
                    {
                        _lastActionReminder = 2;
                        return new PickUpPiece(_guid, _gameId);
                    }
                }
                else
                {
                    if (_pieceTaken != null)
                    {
                        if (_goalSearching)
                        {
                            if (((GoalField)_board[_currentX][_currentY]).type == GoalFieldType.unknown)
                            {
                                _lastActionReminder = 4;
                                _pieceTaken = null;
                                return new PlacePiece(_guid, _gameId);  
                            }
                            else
                            {
                                return NextMove();
                            }
                        }
                        else
                        {
                            return NextMove();
                        }
                    }
                    else
                    {
                        _lastActionReminder = 0;
                        _previousMoveDirection = _moveFromGoalArea;
                        return new Move(_guid, _moveFromGoalArea);
                    }
                }
            }
            else
            {
                if (_lastActionReminder == 3)
                {
                    // for now it is always normal piece, later we have to add checking whether it is a sham 
                    _lastActionReminder = 0;
                    _previousMoveDirection = _moveToGoalArea;
                    _moveDiscoverCount++;
                    return new Move(_guid, _moveToGoalArea);
                }
                else
                {
                    if (_pieceTaken != null)
                    {
                        if (_pieceTaken.type == PieceType.unknown)
                        {
                            _lastActionReminder = 3;
                            return new TestPiece(_guid, _gameId);
                        }
                        else
                        {
                            _lastActionReminder = 0;
                            _previousMoveDirection = _moveToGoalArea;
                            _moveDiscoverCount++;

                            return new Move(_guid, _moveToGoalArea);
                        }
                    }
                    else
                    {
                        if (((TaskField)_board[_currentX][_currentY]).distanceToPiece == 0)
                        {
                            _lastActionReminder = 2;
                            return new PickUpPiece(_guid, _gameId);
                        }
                        else
                        {
                            if (_moveDiscoverCount >= 2)
                            {
                                _lastActionReminder = 1;
                                _moveDiscoverCount = 0;
                                return new Discover(_guid,_gameId);
                            }
                            else
                            {
                                return NextMove();
                            }

                        }
                    }
                }
            }
        }

        private GameMessage NextMove()
        {
            _lastActionReminder = 0;
            if (IsInTheGoalField())
            {
                if (_goalSearching)
                {
                    if (_activeDirection == MoveType.down)
                    {
                        if (CanGoDown())
                        {
                            _previousMoveDirection = MoveType.down;
                            return new Move(_guid, MoveType.down);
                        }
                        else
                        {
                            if (CanGoRight())
                            {
                                _activeDirection = MoveType.up;
                                _previousMoveDirection = MoveType.up;
                                return new Move(_guid, MoveType.right);
                            }
                            else
                            {
                                Console.WriteLine("No fields to go :(");
                            }
                        }
                    }
                    else
                    {
                        if (CanGoUp())
                        {
                            _previousMoveDirection = MoveType.up;
                            return new Move(_guid, MoveType.up);
                        }
                        else
                        {
                            if (CanGoRight())
                            {
                                _activeDirection = MoveType.down;
                                _previousMoveDirection = MoveType.right;
                                return new Move(_guid, MoveType.right);
                            }
                            else
                            {
                                Console.WriteLine("No fields to go :/");
                            }
                        }
                    }
                }
                else
                {
                    if (IsPlayerInUpperLeftCorner())
                    {
                        _activeDirection = MoveType.down;
                        _goalSearching = true;
                        _lastActionReminder = 4;
                        _pieceTaken = null;
                        return new PlacePiece(_guid, _gameId);
                    }
                    else
                    {
                        if (CanGoUp())
                        {
                            _previousMoveDirection = MoveType.up;
                            return new Move(_guid, MoveType.up);
                        }
                        else
                        {
                            if (CanGoLeft())
                            {
                                _previousMoveDirection = MoveType.left;
                                return new Move(_guid, MoveType.left);
                            }
                            else
                            {
                                Console.WriteLine("Cannot go further - we are in the corner :)");
                            }
                        }
                    }
                }
            }
            else
            {
                if (((TaskField)_board[_currentX][_currentY]).distanceToPiece == -1)
                {
                    return new Discover(_guid, _gameId);
                }
                else
                {
                    int minMD = ((TaskField)_board[_currentX][_currentY]).distanceToPiece;
                    MoveType tmpDirection = MoveType.up;
                    if ((!IsOutsideBorder(_currentX, _currentY + 1)) &&
                        ((TaskField)_board[_currentX][_currentY + 1]).distanceToPiece < minMD &&
                        ((TaskField)_board[_currentX][_currentY + 1]).distanceToPiece > -1)
                    {
                        minMD = ((TaskField)_board[_currentX][_currentY + 1]).distanceToPiece;
                        tmpDirection = MoveType.up;
                    }
                    if ((!IsOutsideBorder(_currentX, _currentY - 1)) &&
                        ((TaskField)_board[_currentX][_currentY - 1]).distanceToPiece < minMD &&
                        ((TaskField)_board[_currentX][_currentY - 1]).distanceToPiece > -1)
                    {
                        minMD = ((TaskField)_board[_currentX][_currentY - 1]).distanceToPiece;
                        tmpDirection = MoveType.down;
                    }
                    if ((!IsOutsideBorder(_currentX - 1, _currentY)) &&
                        ((TaskField)_board[_currentX - 1][_currentY]).distanceToPiece < minMD &&
                        ((TaskField)_board[_currentX - 1][_currentY]).distanceToPiece > -1)
                    {
                        minMD = ((TaskField)_board[_currentX - 1][_currentY]).distanceToPiece;
                        tmpDirection = MoveType.left;
                    }
                    if ((!IsOutsideBorder(_currentX + 1, _currentY)) &&
                        ((TaskField)_board[_currentX + 1][_currentY]).distanceToPiece < minMD &&
                        ((TaskField)_board[_currentX + 1][_currentY]).distanceToPiece > -1)
                    {
                        minMD = ((TaskField)_board[_currentX + 1][_currentY]).distanceToPiece;
                        tmpDirection = MoveType.right;
                    }
                    if (((TaskField)_board[_currentX][_currentY]).distanceToPiece > minMD)
                    {
                        _previousMoveDirection = tmpDirection;
                        _moveDiscoverCount++;
                        return new Move(_guid, tmpDirection);
                    }
                    else
                    {
                        _moveDiscoverCount++;
                        return new Move(_guid, _previousMoveDirection);
                    }
                }
            }
            return new Discover(_guid, _gameId);
        }

        public bool CanGoDown()
        {
            if (_hasStartedOnTop)
            {
                return _currentY > _goalsHeight + _tasksHeight;
            }
            else
            {
                return _currentY > 0;
            }
        }

        public bool CanGoUp()
        {
            if (_hasStartedOnTop)
            {
                return _currentY < 2 * _goalsHeight + _tasksHeight - 1;
            }
            else
            {
                return _currentY < _goalsHeight - 1;
            }
        }

        public bool CanGoRight()
        {
            return _currentX < _boardWidth;
        }

        public bool CanGoLeft()
        {
            return _currentX > 0;
        }

        public bool IsPlayerInUpperLeftCorner()
        {

            if (_hasStartedOnTop)
            {
           //     Console.WriteLine("IsInTop");
           //     Console.WriteLine("Current y: " + _currentY+ ", x: "+ _currentX);
                return (_currentY == 2 * _goalsHeight + _tasksHeight - 1 && _currentX == 0);
            }
            else
            {
         //       Console.WriteLine("Is at the bottom");
        //        Console.WriteLine("Current y: " + _currentY + ", x: " + _currentX);
                return (_currentY == _goalsHeight - 1 && _currentX == 0);
            }
        }

        public bool IsOutsideBorder(int x, int y)
        {
            return (x < 0 || x >= _boardWidth || y < _goalsHeight || y >= _goalsHeight + _tasksHeight);
        }
    }
}
