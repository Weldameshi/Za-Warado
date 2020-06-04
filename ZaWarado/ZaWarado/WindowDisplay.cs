using System.Windows;

namespace ZaWarado
{
    public static class WindowDisplay
    {
        public static Window Current { get; } = null;

        private static Window mainMenu = new StartWindow();
        private static Window gameMenu = new MainWindow();

        public static void ShowMainMenu()
        {
            if (!mainMenu.IsActive) mainMenu.Activate();
            mainMenu.Show();
            gameMenu.Hide();
        }

        public static void ShowGame()
        {
            if (!gameMenu.IsActive) mainMenu.Activate();
            gameMenu.Show();
            mainMenu.Hide();
        }
    }
}
