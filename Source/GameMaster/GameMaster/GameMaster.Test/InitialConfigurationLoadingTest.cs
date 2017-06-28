using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GameMaster.Test
{
    [TestClass]
    public class InitialConfigurationLoadingTest
    {
        private string[] _parameters = { "gameName", "2" };
        private int _goalAreaLength = 1;
        private int _taskAreaLength = 4;
        private int _boardWidth = 2;
        [TestMethod]
        public void SetInitialBoardIsSettingTheBoardDimentionsCorrectly()
        {  
            GameMaster.GameState _gamestate = new GameState(_parameters);
            _gamestate.BoardList = _gamestate.SetInitialBoard(_goalAreaLength, _taskAreaLength, _boardWidth);
            Assert.AreEqual(_boardWidth, _gamestate.BoardList.Count);
            Assert.AreEqual(_goalAreaLength * 2 + _taskAreaLength, _gamestate.BoardList[0].Count);
        }

        [TestMethod]
        public void SetInitialBoardIsSettingTheGoalTilesCorrectly()
        {
            GameMaster.GameState _gamestate = new GameState(_parameters);
            _gamestate.BoardList = _gamestate.SetInitialBoard(_goalAreaLength, _taskAreaLength, _boardWidth);
            for (int i = 0; i < _boardWidth; i++)
                for (int j = 0; j < _taskAreaLength + 2 * _goalAreaLength; j++)
                    if (!(j >= _goalAreaLength && j < _goalAreaLength + _taskAreaLength))
                        Assert.IsInstanceOfType(_gamestate.BoardList[i][j], typeof(Xsd2.GoalField));
        }
        [TestMethod]
        public void GameMasterIsSettingTheTaskTilesCorrectly()
        {
            GameMaster.GameState _gamestate = new GameState(_parameters);
            _gamestate.BoardList = _gamestate.SetInitialBoard(_goalAreaLength, _taskAreaLength, _boardWidth);
            for (int i = 0; i < _boardWidth; i++)
                for (int j = 0; j < _taskAreaLength + 2 * _goalAreaLength; j++)
                    if (j >= _goalAreaLength && j < _goalAreaLength + _taskAreaLength)
                        Assert.IsInstanceOfType(_gamestate.BoardList[i][j], typeof(Xsd2.TaskField));
        }
        [TestMethod]
        public void SetInitialGoalsPlacesGoalFieldCorrectly()
        {
            GameMaster.GameState _gamestate = new GameState(_parameters);
            _gamestate.BoardList = _gamestate.SetInitialBoard(_goalAreaLength, _taskAreaLength, _boardWidth);
            List<Configuration.GoalField> goalsList = new List<Configuration.GoalField>();
            Configuration.GoalField gf1 = new Configuration.GoalField();
            gf1.x = 0;
            gf1.y = 0;
            gf1.team = Configuration.TeamColour.blue;
            gf1.type = Configuration.GoalFieldType.goal;
            Configuration.GoalField gf2 = new Configuration.GoalField();
            gf2.x = 0;
            gf2.y = (uint)(_taskAreaLength+_goalAreaLength);
            gf2.team = Configuration.TeamColour.red;
            gf2.type = Configuration.GoalFieldType.goal;
            goalsList.Add(gf1);
            goalsList.Add(gf2);

            Configuration.GoalField[] goals = goalsList.ToArray();
            _gamestate.SetInitialGoals(goals);

            Assert.AreEqual(Xsd2.GoalFieldType.goal, (_gamestate.BoardList[0][0] as Xsd2.GoalField).type  );
            Assert.AreEqual(Xsd2.GoalFieldType.goal, (_gamestate.BoardList[0][_taskAreaLength + _goalAreaLength] as Xsd2.GoalField).type );
        }

        [TestMethod]
        public void SetInitialGoalsPlacesGoalFieldCorrectly2()
        {
            _goalAreaLength = 2;
            _taskAreaLength = 5;
            _boardWidth = 4;

        GameMaster.GameState _gamestate = new GameState(_parameters);
            _gamestate.BoardList = _gamestate.SetInitialBoard(_goalAreaLength, _taskAreaLength, _boardWidth);
            List<Configuration.GoalField> goalsList = new List<Configuration.GoalField>();
            Configuration.GoalField gf1 = new Configuration.GoalField();
            gf1.x = 1;
            gf1.y = 1;
            gf1.team = Configuration.TeamColour.blue;
            gf1.type = Configuration.GoalFieldType.goal;
            Configuration.GoalField gf2 = new Configuration.GoalField();
            gf2.x = 3;
            gf2.y = (uint)(_taskAreaLength + _goalAreaLength + 1);
            gf2.team = Configuration.TeamColour.red;
            gf2.type = Configuration.GoalFieldType.goal;
            goalsList.Add(gf1);
            goalsList.Add(gf2);

            Configuration.GoalField[] goals = goalsList.ToArray();
            _gamestate.SetInitialGoals(goals);

            Assert.AreEqual(Xsd2.GoalFieldType.goal, (_gamestate.BoardList[1][1] as Xsd2.GoalField).type);
            Assert.AreEqual(Xsd2.GoalFieldType.goal, (_gamestate.BoardList[3][_taskAreaLength + _goalAreaLength + 1] as Xsd2.GoalField).type);
        }

        [TestMethod]
        public void SetInitialPiecesPlacesTheCorrectAmountOfPiecesTest()
        {
            uint numberOfPieces = 1;
            ulong pieceIdCounter = 0;
            GameMaster.GameState _gamestate = new GameState(_parameters);
            _gamestate.BoardList = _gamestate.SetInitialBoard(_goalAreaLength, _taskAreaLength, _boardWidth);
            _gamestate.SetInitialPieces(numberOfPieces, ref pieceIdCounter, _gamestate.BoardList, _goalAreaLength, _taskAreaLength);

            int actualPiecesOnTheBoard = 0;
            foreach (var l in _gamestate.BoardList)
            {
                foreach (Xsd2.Field f in l)
                {
                    if (f is Xsd2.TaskField)
                    {
                        if ((f as Xsd2.TaskField).pieceIdSpecified == true)
                            actualPiecesOnTheBoard++;
                    }
                }
            }
            Assert.AreEqual(1,actualPiecesOnTheBoard);
           
        }

        //ANOTHER TEST FOR INITIAL PLACING OF THE PLAYERS.
    }
}
