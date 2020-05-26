using System;
using System.Collections.Generic;

namespace ZaWarado.Models
{
    public class Board
    {
        /// <summary>
        /// Internal data structure used to track the location of valid cards
        /// </summary>
        internal struct Coord : IComparable<Coord>
        {
            internal int x;
            internal int y;

            internal Coord(int x, int y) { this.x = x; this.y = y; }

            public int CompareTo(Coord other)
            {
                int diff = 0;

                if (x < other.x || y < other.y) diff = -1;
                else if (x == other.x && y == other.y) diff = 0;
                else if (x > other.x || y > other.y) diff = 1;

                return diff;
            }
        }

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

        /// <summary>
        /// Return a card and it's parallel neighbors.
        /// </summary>
        /// <param name="x">Central card X location</param>
        /// <param name="y">Central card Y location</param>
        /// <returns>A collection of non-null neighbors where applicable</returns>
        public Card[,] GetCardAndNeighbors(int x, int y)
        {
            if (board is null)
                return null;

            Card[,] neighbors = new Card[3, 3];
            Coord loc = new Coord(x, y);

            bool temporaryAdd = false;
            Coord temporaryAddCoord = default;

            if (!board.ContainsKey(loc))
            {
                temporaryAdd = true;
                temporaryAddCoord = loc;
                board.Add(loc, null);
            }

            /* X: 0 1 2 - Y: 0 0 0
             * X: 0 1 2 - Y: 1 1 1
             * X: 0 1 2 - Y: 2 2 2
             */

            neighbors[1, 1] = board[loc]; Coord next = new Coord(x, y - 1);
            neighbors[1, 0] = board.ContainsKey(next) ? board[next] : null; next = new Coord(x, y + 1);
            neighbors[1, 2] = board.ContainsKey(next) ? board[next] : null; next = new Coord(x - 1, y);
            neighbors[0, 1] = board.ContainsKey(next) ? board[next] : null; next = new Coord(x + 1, y);
            neighbors[2, 1] = board.ContainsKey(next) ? board[next] : null;

            if (temporaryAdd) board.Remove(temporaryAddCoord);

            return neighbors;
        }
    }
}