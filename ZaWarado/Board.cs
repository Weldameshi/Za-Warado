using System;

/// <summary>
/// empty dev class, remove when card class is complete
/// </summary>
internal class Card { }

public class Board
{
    internal struct Coord
    {
        internal int x;
        internal int y;

        internal Coord(int x, int y) { this.x = x; this.y = y; }
    }

    //Card[,] board = null;
    Dictionary<Coord, Card> board = null;

    /// <summary>
    /// Get the instance of card at the given location on the board
    /// </summary>
    /// <param name="x">X location of the desired card</param>
    /// <param name="y">Y location of the desired card</param>
    /// <returns>The card instance found at the provided location</returns>
    public Card this[int x, int y]
    {
        get
        {
            if (board is null)
                return null;
            if (x < 0 || x >= board.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), x, "provided value cannot be less than zero, or greater than the last index of the array");
            if (y < 0 || y >= board.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), y, "provided value cannot be less than zero, or greater than the last index of the array");

            return board[x, y];
        }
    }

    /// <summary>
    /// Create a square board of the given dimension
    /// </summary>
    /// <param name="squareBoardDimensions">Square boarddimension</param>
    public Board(int squareBoardDimensions) => board = new board[squareBoardDimensions, squareBoardDimensions];
    /// <summary>
    /// Create a rectangular board of the given dimensions
    /// </summary>
    /// <param name="horizontalBoardDimension">Horizontal board dimension</param>
    /// <param name="verticalBoardDimension">Vertical board dimension</param>
    public Board(int horizontalBoardDimension, int verticalBoardDimension) => board = new bool[horizontalBoardDimension, verticalBoardDimension];

    /// <summary>
    /// Add a card to the board in the requested location
    /// </summary>
    /// <param name="card">Card to add to the board</param>
    /// <param name="x">X location to add the card at</param>
    /// <param name="y">Y location to add the card at</param>
    public void AddCard(Card card, int x, int y)
    {
        if (board is null)
            return;
        if (x < 0 || x >= board.GetLength(0))
            throw new ArgumentOutOfRangeException(nameof(x), x, "provided value cannot be less than zero, or greater than the last index of the array");
        if (y < 0 || y >= board.GetLength(1))
            throw new ArgumentOutOfRangeException(nameof(y), y, "provided value cannot be less than zero, or greater than the last index of the array");

        board[x, y] = card;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Card[,] GetCardAndNeighbors(int x, int y)
    {
        if (board is null)
            return;
        if (x < 0 || x >= board.GetLength(0))
            throw new ArgumentOutOfRangeException(nameof(x), x, "provided value cannot be less than zero, or greater than the last index of the array");
        if (y < 0 || y >= board.GetLength(1))
            throw new ArgumentOutOfRangeException(nameof(y), y, "provided value cannot be less than zero, or greater than the last index of the array");

        Card[,] neighbors = new Card[3, 3];
        


    }
}
