using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Messages.PlayerInterfaces;
using FakeItEasy;
using Xsd2;
using System.Collections.Generic;

namespace Player.Test
{
    [TestClass]
    public class PlayerLogicTest
    {
        private string[] playerParameters = new string[] { "30000", "red", "member" };
        private int boardWidth = 2;
        private int goalsHeight = 1;
        private int tasksHeight = 4;

        [TestMethod]
        public void PlayerLogicCreatesBoardWithCorrectDimensions()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);

            Assert.AreEqual(boardWidth, logic.Board.Count);
            Assert.AreEqual(2*goalsHeight+tasksHeight, logic.Board[0].Count);
        }

        [TestMethod]
        public void PlayerLogicCreatesBoardWithCorrectGoalsFieldHeight()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);

            for (int i = 0; i < boardWidth; i++)
                for (int j = 0; j < tasksHeight + 2 * goalsHeight; j++)
                    if (!(j >= goalsHeight && j < goalsHeight + tasksHeight))
                        Assert.IsInstanceOfType(logic.Board[i][j], typeof(Xsd2.GoalField));
        }

        [TestMethod]
        public void PlayerLogicCreatesBoardWithCorrectTasksFieldHeight()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);

            for (int i = 0; i < boardWidth; i++)
                for (int j = 0; j < tasksHeight + 2 * goalsHeight; j++)
                    if (j >= goalsHeight && j < goalsHeight + tasksHeight)
                        Assert.IsInstanceOfType(logic.Board[i][j], typeof(TaskField));
        }

        [TestMethod]
        public void PlayerLogicCreatesBoardWithUnknownTypeGoalFields()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);

            for (int i = 0; i < boardWidth; i++)
                for (int j = 0; j < tasksHeight + 2 * goalsHeight; j++)
                    if (!(j >= goalsHeight && j < goalsHeight + tasksHeight))
                        Assert.AreEqual(GoalFieldType.unknown, (logic.Board[i][j] as GoalField).type);
        }

        [TestMethod]
        public void PlayerLogicCreatesBoardWithNeededDistanceToPiece()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);

            for (int i = 0; i < boardWidth; i++)
                for (int j = 0; j < tasksHeight + 2 * goalsHeight; j++)
                    if (j >= goalsHeight && j < goalsHeight + tasksHeight)
                        Assert.AreEqual(-3, (logic.Board[i][j] as TaskField).distanceToPiece);
        }

        [TestMethod]
        public void PlayerLogicCanGoUpReturnsFalseAtVeryTop()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);
            logic.GoalsHeight = goalsHeight;
            logic.TasksHeight = tasksHeight;
            logic.CurrentY = goalsHeight*2+tasksHeight-1;
            Assert.IsFalse(logic.CanGoUp());
        }

        [TestMethod]
        public void PlayerLogicCanGoUpReturnsFalseWhenTryingToEnterAnotherTeamGoalField()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);
            logic.GoalsHeight = goalsHeight;
            logic.TasksHeight = tasksHeight;
            logic.CurrentY = goalsHeight + tasksHeight - 1;
            Assert.IsFalse(logic.CanGoUp());
        }

        [TestMethod]
        public void PlayerLogicCanGoDownReturnsFalseWhenTruingToEnterAnotherTeamGoalField()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);
            logic.HasStartedOnTop = true;
            logic.GoalsHeight = goalsHeight;
            logic.TasksHeight = tasksHeight;
            logic.CurrentY = goalsHeight;
            Assert.IsFalse(logic.CanGoDown());
        }


        [TestMethod]
        public void PlayerLogicCanGoDownReturnsFalseAtTheBottom()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);
            logic.GoalsHeight = goalsHeight;
            logic.TasksHeight = tasksHeight;
            logic.CurrentY = 0;
            Assert.IsFalse(logic.CanGoDown());
        }

        [TestMethod]
        public void PlayerLogicCanGoDownReturnsTrueInMiddle()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);
            logic.GoalsHeight = goalsHeight;
            logic.TasksHeight = tasksHeight;
            logic.CurrentY = goalsHeight;
            Assert.IsTrue(logic.CanGoDown());
        }

        [TestMethod]
        public void PlayerLogicCanGoUpReturnsTrueInMiddle()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);
            logic.GoalsHeight = goalsHeight;
            logic.TasksHeight = tasksHeight;
            logic.CurrentY = tasksHeight-1;
            Assert.IsTrue(logic.CanGoUp());
        }

        [TestMethod]
        public void PlayerLogicCanGoLeftReturnsFalseOnLeftBorder()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);
            logic.BoardWidth = boardWidth;
            logic.CurrentX = 0;
            Assert.IsFalse(logic.CanGoLeft());
        }

        [TestMethod]
        public void PlayerLogicCanGoLeftReturnsTrueInTheMiddle()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);
            logic.BoardWidth = boardWidth;
            logic.CurrentX = boardWidth-1;
            Assert.IsTrue(logic.CanGoLeft());
        }

        [TestMethod]
        public void PlayerLogicIsOutsideBorderReturnsFalseInMiddle()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);
            logic.BoardWidth = boardWidth;
            logic.GoalsHeight = goalsHeight;
            logic.TasksHeight = tasksHeight;
            Assert.IsFalse(logic.IsOutsideBorder( boardWidth-1, tasksHeight));
        }

        [TestMethod]
        public void PlayerLogicIsOutsideBorderReturnsTrueOutSide()
        {
            PlayerLogic logic = new PlayerLogic();
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);
            logic.BoardWidth = boardWidth;
            logic.GoalsHeight = goalsHeight;
            logic.TasksHeight = tasksHeight;
            Assert.IsTrue(logic.IsOutsideBorder(boardWidth+1, tasksHeight));
        }


        [TestMethod]
        public void PlayerLogicMakesCorrectFirstMoveTowardsTasksField()
        {
            var connection = A.Fake<Connection>(o => o.WithArgumentsForConstructor(
                new object[] { "127.0.0.1", 8002, "testGame", playerParameters, new object() }));
            PlayerLogic logic = new PlayerLogic(connection);
            logic.Board = logic.CreateInitialBoard(goalsHeight, tasksHeight, boardWidth);

            Game gameMessage = new Game();
            gameMessage.playerId = 0;
            List<Xsd2.Player> players = new List<Xsd2.Player>();
            Xsd2.Player player = new Xsd2.Player();
            players.Add(new Xsd2.Player(0, TeamColour.blue, PlayerType.member));
            gameMessage.Players = players.ToArray();
            gameMessage.Board = new GameBoard();
            gameMessage.Board.width = (uint)boardWidth;
            gameMessage.Board.tasksHeight = (uint)tasksHeight;
            gameMessage.Board.goalsHeight = (uint)goalsHeight;
            gameMessage.PlayerLocation = new Location();
            gameMessage.PlayerLocation.x = 0;
            gameMessage.PlayerLocation.y = 0;

            GameMessage msg = logic.AnswerForGameMessage(connection, gameMessage);
            // Since we create agent in the bottom, direction is up)
            Assert.AreEqual(MoveType.up, (msg as Move).direction);
        }
    }
}
