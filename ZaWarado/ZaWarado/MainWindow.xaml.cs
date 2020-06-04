using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZaWarado.Models;

namespace ZaWarado
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Style btnStyle;
        Grid boardDisplay;
        Button currentCardButton;
        Card currentCard;
        List<Button> tempButtons = new List<Button>();
        Game game = new Game();
        bool placmentButtonsOn = false;

        public MainWindow()
        {
            WindowDisplay.Initialize();
            InitializeComponent();
            btnStyle = FindResource("ButtonStyle") as Style;
            game.StartGame();
            game.StartTurn();
            DisplayHand();
            SetUpBoard();
        }

        private void DisplayHand()
        {
            Hand.Children.Clear();
            int handSize = game.PlayerHand.Count;
            for (int i = 0; i < handSize; i++)
            {
                Button newBtn = new Button();
                newBtn.Name = "Card_" + i.ToString();
                newBtn.MaxHeight = 200;
                newBtn.MaxWidth = 120;
                newBtn.MinHeight = 60;
                newBtn.MinWidth = 120;
                newBtn.Style = btnStyle;
                newBtn.Click += HandClick;
                var brush = new ImageBrush();
                brush.ImageSource = game.PlayerHand[i].imageFile;
                newBtn.Background = brush;
                Hand.Children.Add(newBtn);
            }
        }

        private void HandClick(object sender, RoutedEventArgs e)
        {

            currentCardButton = sender as Button;
            string name = currentCardButton.Name.Split('_')[1];
            currentCard = game.PlayerHand[int.Parse(name)];
            if (game.Board.GameBoard.Count == 0)
            {
                PlaceCard(7, 7);
            }
            else if (!placmentButtonsOn)
            {
                foreach (var item in game.Board.GameBoard)
                {
                    Board.Coord itemCoord = item.Key;
                    Dictionary<Board.Coord, Card> itemNeighbors = game.Board.GetCardAndNeighbors(item.Key.x, item.Key.y);

                    if (itemNeighbors[(new Board.Coord(itemCoord.x, itemCoord.y - 1))] is null)
                    {
                        //do true logic (no card is placed above this card]
                        CreateTempButton(item.Key.x, item.Key.y - 1);
                    }
                    if (itemNeighbors[(new Board.Coord(itemCoord.x, itemCoord.y + 1))] is null)
                    {
                        //do true logic (no card is placed above this card]
                        CreateTempButton(item.Key.x, item.Key.y + 1);
                    }
                    if (itemNeighbors[(new Board.Coord(itemCoord.x + 1, itemCoord.y))] is null)
                    {
                        //do true logic (no card is placed above this card]
                        CreateTempButton(item.Key.x + 1, item.Key.y);
                    }
                    if (itemNeighbors[(new Board.Coord(itemCoord.x - 1, itemCoord.y))] is null)
                    {
                        //do true logic (no card is placed above this card]
                        CreateTempButton(item.Key.x - 1, item.Key.y);
                    }

                    //... 3 more times
                    placmentButtonsOn = true;
                }
            }

        }
        private void CreateTempButton(int x, int y)
        {
            bool tempButtonAlreadyExits = false;
            foreach (Button b in tempButtons)
            {
                if (Grid.GetRow(b) == y && Grid.GetColumn(b) == x)
                {
                    tempButtonAlreadyExits = true;
                }
            }
            if (!tempButtonAlreadyExits)
            {
                Button newBtn = new Button();
                newBtn.Tag = "";
                newBtn.Style = btnStyle;
                var brush = new SolidColorBrush(Color.FromArgb(125, 255, 165, 0));
                newBtn.Background = brush;
                boardDisplay.Children.Add(newBtn);
                Grid.SetColumn(newBtn, x);
                Grid.SetRow(newBtn, y);
                newBtn.Click += PlaceHolderClick;
                tempButtons.Add(newBtn);
            }

        }
        private void PlaceCard(int x, int y)
        {
            placmentButtonsOn = false;
            Hand.Children.Remove(currentCardButton);
            game.PlaceCard(currentCard, x, y);
            game.PlayerHand.Remove(currentCard);
            DisplayHand();
            WorldScore.Content = game.WorldScore;
            RefreshBoard();
        }

        private void RefreshBoard()
        {
            boardDisplay.Children.Clear();
            foreach (var card in game.Board.GameBoard)
            {
                Button newBtn = new Button();
                newBtn.MaxHeight = 200;
                newBtn.MaxWidth = 120;
                newBtn.MinHeight = 20;
                newBtn.MinWidth = 20;
                newBtn.Style = btnStyle;
                newBtn.Click += HandClick;
                var brush = new ImageBrush();
                brush.ImageSource = card.Value.imageFile;
                newBtn.Background = brush;
                newBtn.IsEnabled = false;
                boardDisplay.Children.Add(newBtn);
                Grid.SetColumn(newBtn, card.Key.x);
                Grid.SetRow(newBtn, card.Key.y);
            }
        }
        private void EndTurnClick(object sender, RoutedEventArgs e)
        {
            game.EndTurn();
            boardDisplay.Children.Clear();
            game.StartTurn();
            DisplayHand();
        }
        private void PlaceHolderClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            PlaceCard(Grid.GetColumn(b), Grid.GetRow(b));
            currentCardButton = null;
            foreach (Button btn in tempButtons)
            {
                boardDisplay.Children.Remove(btn);
            }
            tempButtons.Clear();
        }
        private void SetUpBoard()
        {
            boardDisplay = new Grid();
            GameArea.Children.Add(boardDisplay);
            Grid.SetRow(boardDisplay, 1);
            Grid.SetColumn(boardDisplay, 1);
            for (int i = 0; i < 15; i++)
            {
                boardDisplay.ColumnDefinitions.Add(new ColumnDefinition());
                boardDisplay.RowDefinitions.Add(new RowDefinition());
            }
            var brush1 = new ImageBrush();
            brush1.ImageSource = new BitmapImage(new Uri("../../Assets/Images/Mars.jpg", UriKind.Relative));
            boardDisplay.Background = brush1;
            WorldScore.Style = btnStyle;
            var brush2 = new SolidColorBrush(Color.FromArgb(125, 255, 165, 0));
            WorldScore.Background = brush2;
            EndTurn.Style = btnStyle;
            EndTurn.Background = brush2;
            RestartButton.Style = btnStyle;
            RestartButton.Background = brush2;
            Color background = Color.FromRgb(166, 56, 46);
            GameArea.Background = new SolidColorBrush(background);

            Color solidLineColor = Color.FromRgb(64, 6, 1);
            Label solidLine = new Label();
            solidLine.Background = new SolidColorBrush(solidLineColor);
            GameArea.Children.Add(solidLine);
            Grid.SetRow(solidLine, 4);
            Grid.SetColumnSpan(solidLine, 3);
            Grid.SetColumn(solidLine, 0);
            solidLine.IsEnabled = false;
            Label solidLine2 = new Label();
            solidLine2.Background = new SolidColorBrush(solidLineColor);
            GameArea.Children.Add(solidLine2);
            Grid.SetRow(solidLine2, 2);
            Grid.SetColumnSpan(solidLine2, 3);
            Grid.SetColumn(solidLine2, 0);
        }
        public void Restart()
        {
            game.StartGame();
            game.StartTurn();
            DisplayHand();
            SetUpBoard();
            WorldScore.Content = game.WorldScore;
        }
        private void RestartClick(object sender, RoutedEventArgs e)
        {
            Restart();
        }

        //Set button background code

        //var brush = new ImageBrush();
        //brush.ImageSource = new BitmapImage(new Uri("Images/ContentImage.png", UriKind.Relative));
        //button1.Background = brush;

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            WindowDisplay.ShowMainMenu();
        }
    }
}
