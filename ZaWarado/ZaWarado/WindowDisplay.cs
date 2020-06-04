﻿using System;
using System.Windows;

namespace ZaWarado
{
    public static class WindowDisplay
    {
        public static Window Current { get; } = null;
        public static bool ResumeExistingGame { get; set; } = false;

        static bool initialized = false;

        private static Window mainMenu = new StartWindow();
        private static Window gameMenu = new MainWindow();

        public static void Initialize()
        {
            if (!initialized)
            {
                gameMenu.Closed += ClosedGameWindow;
                initialized = true;
            }
        }

        static void ClosedGameWindow(object sender, EventArgs args)
        {
            gameMenu = new MainWindow();
            gameMenu.Closed += ClosedGameWindow;
        }

        public static void ShowMainMenu()
        {
            if (!mainMenu.IsActive) mainMenu.Activate();
            mainMenu.Show();
            gameMenu.Hide();
        }

        public static void RestartGame()
        {
            if (gameMenu.IsActive) gameMenu.Hide();
            gameMenu = new MainWindow();
            gameMenu.Activate();
            gameMenu.Show();
        }

        public static void ShowGame()
        {
            if (ResumeExistingGame)
            {
                if (!gameMenu.IsActive) mainMenu.Activate();
                gameMenu.Show();
                mainMenu.Hide();
            }
            else RestartGame();
        }
    }
}
