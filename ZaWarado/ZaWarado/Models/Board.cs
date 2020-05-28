using System;
using System.Collections.Generic;

namespace ZaWarado.Models
{
    public class Board
    {
        /// <summary>
        /// Internal data structure used to track the location of valid cards
        /// </summary>
        public struct Coord : IComparable<Coord>
        {
            public readonly int x;
            public readonly int y;

            public Coord(int x, int y) { this.x = x; this.y = y; }

            public int CompareTo(Coord other)
            {
                int diff = 0;

                if (x < other.x || y < other.y) diff = -1;
                else if (x == other.x && y == other.y) diff = 0;
                else if (x > other.x || y > other.y) diff = 1;

                return diff;
            }
        }

        private Dictionary<Coord, Card> board = null;
        /// <summary>
        /// The underlying storage mechanism that this board class uses to manage the Cards it holds
        /// </summary>
        internal Dictionary<Coord, Card> GameBoard { get => board; }

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

                Coord loc = new Coord(x, y);
                if (board.ContainsKey(loc))
                    return board[loc];
                else return null;
            }
        }

        /// <summary>
        /// Create a square board of the given dimension
        /// </summary>
        /// <param name="squareBoardDimensions">Square boarddimension</param>
        public Board() => board = new Dictionary<Coord, Card>();

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

            Coord loc = new Coord(x, y);
            if (board.ContainsKey(loc))
                return;

            board[loc] = card;
        }

        ///// <summary>
        ///// Return a card and it's parallel neighbors.
        ///// </summary>
        ///// <param name="x">Central card X location</param>
        ///// <param name="y">Central card Y location</param>
        ///// <returns>A collection of neighboring cards stored in a dictionary</returns>
        public Dictionary<Coord, Card> GetCardAndNeighbors(int x, int y)
        {
            if (board is null) return null;

            Dictionary<Coord, Card> @return = new Dictionary<Coord, Card>();
            Coord loc = new Coord(x, y);

            @return.Add(loc, board.ContainsKey(loc) ? board[loc] : null); loc = new Coord(x, y + 1);
            @return.Add(loc, board.ContainsKey(loc) ? board[loc] : null); loc = new Coord(x, y - 1);
            @return.Add(loc, board.ContainsKey(loc) ? board[loc] : null); loc = new Coord(x + 1, y);
            @return.Add(loc, board.ContainsKey(loc) ? board[loc] : null); loc = new Coord(x - 1, y);
            @return.Add(loc, board.ContainsKey(loc) ? board[loc] : null);

            return @return;
        }
    }
}