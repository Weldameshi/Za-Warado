using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ZaWarado.Models;


namespace UnitTesting
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void CreateGameShouldAssertTrueIfGameCreatedSuccessfully()
        {
            Game game = new Game();
            Assert.IsNotNull(game);
        }

        [TestMethod]
        public void StartGameToSeePlayerHandSize()
        {
            Game game = new Game();
            game.StartGame();
            Assert.AreEqual(game.PlayerHand.Count, 5);
        }

        [TestMethod]
        public void EndTurnToCheckTurnNumberAndPlayerDiscards()
        {
            Game game = new Game();
            game.StartGame();
            game.StartTurn();
            game.EndTurn();

            Assert.AreEqual(game.PlayerHand.Count, 0);
        }

        [TestMethod]
        public void EndGameToSeeOutcome()
        {
            int xPosition = 0;
            Game game = new Game();
            game.StartGame();
            for (int i = 0; i < 10; i++)
            {
                game.StartTurn();
                for (int j = 0; j < game.PlayerHand.Count; j++)
                {
                    game.PlaceCard(game.PlayerHand[j], xPosition++, 0);
                }
                game.EndTurn();
            }
            Console.WriteLine(game.IsWon);
        }
    }
}
