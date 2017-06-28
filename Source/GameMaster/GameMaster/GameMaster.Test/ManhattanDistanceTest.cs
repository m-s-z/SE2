using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GameMaster.Test
{
    [TestClass]
    public class ManhattanDistanceTest
    {
        private string[] _parameters = { "gameName", "2" };
        private int _taskAreaLength = 3;
        private int _taskAreWidth = 3;
        // UNDER CONSTRUCTION 
        [TestMethod]
        public void GameMasterIsCorrectlyCalculatingTheManhattanDistance()
        {
            GameMaster.GameState _gameState = new GameState(_parameters);
            List<List<Xsd2.Field>> taskArea = new List<List<Xsd2.Field>>();
            for(int i = 0; i < _taskAreaLength; i++)
            {
                taskArea.Add(new List<Xsd2.Field>());
                for(int j = 0; j < _taskAreWidth; j++)
                {
                    taskArea[i].Add(new Xsd2.TaskField() { x = (uint)i + 1, y = (uint)j + 1 });
                }
            }
            _gameState.AddPiece(new Xsd2.Piece(), new Xsd2.Location() { x = 1, y = 1 });
            _gameState.BoardList = taskArea;
            _gameState.CalculateManhattanDistance();

            Assert.AreEqual(4, (taskArea[2][2] as Xsd2.TaskField).distanceToPiece);

        }
        [TestMethod]
        public void GameMasterIsCorrectlyCalculatingTheManhattanDistanceForTwoPieces()
        {
            GameMaster.GameState _gameState = new GameState(_parameters);
            List<List<Xsd2.Field>> taskArea = new List<List<Xsd2.Field>>();
            for (int i = 0; i < _taskAreaLength; i++)
            {
                taskArea.Add(new List<Xsd2.Field>());
                for (int j = 0; j < _taskAreWidth; j++)
                {
                    taskArea[i].Add(new Xsd2.TaskField() { x = (uint)i + 1, y = (uint)j + 1 });
                }
            }
            _gameState.AddPiece(new Xsd2.Piece(), new Xsd2.Location() { x = 1, y = 1 });
            _gameState.AddPiece(new Xsd2.Piece(), new Xsd2.Location() { x = 1, y = 3 });
            _gameState.BoardList = taskArea;
            _gameState.CalculateManhattanDistance();

            Assert.AreEqual(2, (taskArea[2][2] as Xsd2.TaskField).distanceToPiece);
            Assert.AreEqual(2, (taskArea[2][0] as Xsd2.TaskField).distanceToPiece);
            Assert.AreEqual(1, (taskArea[0][1] as Xsd2.TaskField).distanceToPiece);

        }
    }
}
