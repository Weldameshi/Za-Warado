using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ZaWarado.Models;

namespace UnitTesting
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void CreateBoardShouldAssertTrueIfBoardCretedSuccessfully()
        {
            Board board = new Board();
            Assert.IsTrue(board != null);
        }

        [TestMethod]
        public void CreateABoardAndPlaceTenCardsShouldAssertTrueIfCardsSuccessfullyPlaced()
        {
            Board board = new Board();
            Card card = new Card(Card.Type.WIND);

            board.AddCard(card, 0, 0);
            board.AddCard(card, 0, 1);
            board.AddCard(card, 0, 2);
            board.AddCard(card, 1, 0);
            board.AddCard(card, 1, 1);
            board.AddCard(card, 2, 0);
            board.AddCard(card, 3, 0);
            board.AddCard(card, 2, 1);
            board.AddCard(card, 3, 1);
            board.AddCard(card, 0, 3);

            Assert.IsTrue(board[0, 0] != null);
            Assert.IsTrue(board[0, 1] != null);
            Assert.IsTrue(board[0, 2] != null);
            Assert.IsTrue(board[1, 0] != null);
            Assert.IsTrue(board[1, 1] != null);
            Assert.IsTrue(board[2, 0] != null);
            Assert.IsTrue(board[3, 0] != null);
            Assert.IsTrue(board[2, 1] != null);
            Assert.IsTrue(board[3, 1] != null);
            Assert.IsTrue(board[0, 3] != null);
        }

        [TestMethod]
        public void CreateABoardAndPlaceThreeCardsInTheSamePlaceShouldAssertTrueIfTheCardTypeIsWater()
        {
            Board board = new Board();
            Card waterCard = new Card(Card.Type.WATER);
            Card mountainCard = new Card(Card.Type.MOUNTAIN);
            Card windCard = new Card(Card.Type.WIND);

            board.AddCard(waterCard, 0, 0);
            board.AddCard(mountainCard, 0, 0);
            board.AddCard(windCard, 0, 0);

            Assert.IsTrue(board[0, 0].type == Card.Type.WATER);
        }

        [TestMethod]
        public void CreateABoardAndGetCardsInLocationsThatDoNotYetExistShouldAssertTrueIfReturnedCardsAreNull()
        {
            Board board = new Board();
            Card testAgainst = null;

            Assert.IsTrue(board[0, 0] == testAgainst);
            Assert.IsTrue(board[10, 10] == testAgainst);
            Assert.IsTrue(board[1000, 1000] == testAgainst);
        }

        //[TestMethod]
        //public void CreateABoardAndGetANeighborThatHasNotYetBenCreatedShouldAssertTrueIfAnArrayOfNullCardsIsReturned()
        //{
        //    Board board = new Board();
        //    Dictionary<Board.Coord, Card> returned = board.GetCardAndNeighbors(0, 0);
        //    Card[,] testAgainst = new Card[,] { { null, null, null }, { null, null, null }, { null, null, null } };

        //    bool arraysIdentical = false;

        //    for (int x = 0; x < 3; x++)
        //        for (int y = 0; y < 3; y++)
        //            arraysIdentical = returned[x, y] == testAgainst[x, y];

        //    Assert.IsTrue(arraysIdentical);
        //}

        //[TestMethod]
        //public void CreateABoardAndGetANeighborThatHasBeenCreatedShouldAssertTrueIfArrayReturnedHasOneNonNullCardInTheCenter()
        //{
        //    Board board = new Board();
        //    Card centralPlacedCard = new Card(Card.Type.FIRE);
        //    Card[,] returned;
        //    Card[,] testAgainst = new Card[,] { { null, null, null }, { null, centralPlacedCard, null }, { null, null, null } };

        //    board.AddCard(centralPlacedCard, 0, 0);
        //    returned = board.GetCardAndNeighbors(0, 0);

        //    bool arraysIdentical = false;

        //    for (int x = 0; x < 3; x++)
        //        for (int y = 0; y < 3; y++)
        //            arraysIdentical = returned[x, y] == testAgainst[x, y];

        //    Assert.IsTrue(arraysIdentical);
        //}
    }
}
