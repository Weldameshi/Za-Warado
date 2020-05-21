using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaWarado.Models
{
    /// <summary>
    /// Represents the game as a whole. It determines what happens when a card
    /// is placed and keeps track of score.
    /// </summary>
    class Game
    {
        public static Game GameSingleton { get; set; }
        //private Board Board { get; set; }
        private int TurnNumber { get; set; }


        private int worldScore;

        public int WorldScore
        {
            get { return worldScore; }
            set { worldScore = value; }
        }
        private int hazardPoints;

        public int HazardPoints
        {
            get { return hazardPoints; }
            set { hazardPoints = Math.Max(0, value); }
        }

        private int bioPoints;

        public int BioPoints
        {
            get { return bioPoints; }
            set { bioPoints = Math.Max(0, value); }
        }

        private int heatPoints;

        public int HeatPoints
        {
            get { return heatPoints; }
            set { heatPoints = Math.Max(0, value); }
        }

        private int cyclePoints;

        public int CyclePoints
        {
            get { return cyclePoints; }
            set { cyclePoints = Math.Max(0, value); }
        }

        private int habitatPoints;

        public int HabitatPoints
        {
            get { return habitatPoints; }
            set { habitatPoints = Math.Max(0, value); }
        }

        private Dictionary<string, bool> ObtainedHabitats { get; set; }
        private List<Card> PlayerHand { get; set; }

        Stack<Card> Deck { get; set; }

        /// <summary>
        /// Starts a new game by resetting all the values to their default, setting up the deck, and starting the firstTurn.
        /// </summary>
        public void StartGame()
        {
            ResetGameValues();
            PopulateDeck();
            StartTurn();
        }

        /// <summary>
        /// Resets the games values to a default to start a new game.
        /// </summary>
        private void ResetGameValues()
        {
            GameSingleton = this;
            //Board = new Board();
            WorldScore = 0;
            TurnNumber = 1;
            HazardPoints = 10;
            HeatPoints = 20;
            CyclePoints = 0;
            HabitatPoints = 0;
            ObtainedHabitats = new Dictionary<string, bool>();
            Deck = new Stack<Card>();
            PlayerHand = new List<Card>();
        }

        /// <summary>
        /// Populates the deck 60 cards (10 of each type) randomly.
        /// </summary>
        private void PopulateDeck()
        {
            //Declares a new random to generate a card from.
            Random rng = new Random();

            //Used to keep track of how many of each card is created.
            //As of now, there can only be 10 of each card but I'm sure this will change.
            int[] generatedCards = new int[6];

            //For loop to generate a 60 cards. Again will likely be changed.
            for (int i = 0; i < 60; i++)
            {
                //Variables to decide if the generate number corresponds to a card type that doesn't have 10 cards yet.
                bool isValid = false;
                int generatedType;
                do
                {
                    generatedType = rng.Next(0, 6);
                    isValid = (generatedCards[generatedType] + 1) <= 10;
                } while (!isValid);

                //creates a new card passing in the Card.Type gotten back from the Enum.GetValues function.
                Card generatedCard = new Card((Card.Type)Enum.GetValues(typeof(Card.Type)).GetValue(generatedType));

                //Increments the generated cards counter up by one.
                generatedCards[generatedType] += 1;

                //Pushes the generated card onto the Deck.
                Deck.Push(generatedCard);
            }
        }

        /// <summary>
        /// Starts a new turn in which the player draws 5 cards from the deck.
        /// </summary>
        private void StartTurn()
        {
            //Call method to add 5 cards to players hand.
            DrawCards(5);
        }

        private void EndTurn()
        {
            DiscardHand();
            if (++TurnNumber > 10)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            TallyScore();
        }

        private void TallyScore()
        {
            int scoreFromBioPoints = (BioPoints - 20 > 0) ? (BioPoints - 20) : (0);
            int scoreFromHazardPoints = HazardPoints;
            int scoreFromHeatPoints = (HeatPoints < 5 || HeatPoints > 15) ? ((HeatPoints < 5) ? (0 - HeatPoints) : (0 - (HeatPoints - 15))) : (0);
            int scoreFromCyclePoints = (CyclePoints > 30) ? (30) : (CyclePoints);
            int scoreFromHabitatPoints = HabitatPoints;
            WorldScore = scoreFromBioPoints +
                         scoreFromHazardPoints +
                         scoreFromHeatPoints +
                         scoreFromCyclePoints +
                         scoreFromHabitatPoints;

            int numOfCollectedHabitats = ObtainedHabitats.Count(habitat => habitat.Value);
        }

        /// <summary>
        /// Discards the entire players hand. 
        /// </summary>
        private void DiscardHand()
        {
            PlayerHand = new List<Card>();
        }

        /// <summary>
        /// Method to draw any number of cards and add them to the player's hand.
        /// </summary>
        /// <param name="numOfCards"></param>
        private void DrawCards(int numOfCards)
        {
            for (int i = 0; i < numOfCards; i++)
            {
                PlayerHand.Add(Deck.Pop());
            }
        }

        /// <summary>
        /// Method to be called when the player places a card from their hand.
        /// </summary>
        /// <param name="placedCard">The card that the player has chosen to place</param>
        /// <param name="xPosition">The x position of the chosen spot to place the card.</param>
        /// <param name="yPosition">The y position of the chosen spot to place the card.</param>
        private void PlaceCard(Card placedCard, int xPosition, int yPosition)
        {
            //Gets the surrounding cards of the placed card.
            Card aboveCard = null; //Board.GetCard(xPosition, yPosition - 1);
            Card rightCard = null; //Board.GetCard(xPosition + 1, yPosition);
            Card belowCard = null; //Board.GetCard(xPosition, yPosition + 1);
            Card leftCard = null;//Board.GetCard(xPosition - 1, yPosition);

            //switch on the placed cards type to dictate how it will interact with the surrounding cards.
            switch (placedCard.type)
            {
                #region Placed Plant card
                case Card.Type.PLANT:
                    BioPoints += 1;
                    //Check if above placed position is a card and do appropriate action based on its type
                    if (aboveCard != null)
                    {
                        switch (aboveCard.type)
                        {
                            case Card.Type.PLANT:
                                break;
                            case Card.Type.WATER:
                                BioPoints += 2;
                                CheckObtainedHabitats("Kelp Forest", 3);
                                break;
                            case Card.Type.WIND:
                                DrawCards(1);
                                break;
                            case Card.Type.FIRE:
                                BioPoints += 1;
                                DrawCards(1);
                                break;
                            case Card.Type.MOUNTAIN:
                                CyclePoints += 2;
                                break;
                            case Card.Type.PLAINS:
                                HazardPoints -= 1;
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if right of placed position is a card and do appropriate action based on its type
                    if (rightCard != null)
                    {
                        switch (rightCard.type)
                        {
                            case Card.Type.PLANT:
                                break;
                            case Card.Type.WATER:
                                HazardPoints -= 3;
                                CheckObtainedHabitats("Wetlands", 3);
                                break;
                            case Card.Type.WIND:
                                if (true)//(Board.GetCard(xPosition - 1, yPosition) == null)
                                {
                                    //Place extra plant card at xPosition-1, yPosition;
                                }
                                else
                                {
                                    BioPoints -= 1;
                                }
                                break;
                            case Card.Type.FIRE:
                                BioPoints -= 2;
                                DiscardCard(Card.Type.PLANT);
                                break;
                            case Card.Type.MOUNTAIN:
                                switch (Card.Type.FIRE)//Board.GetCard(xPosition + 1, yPosition + 1).type)
                                {
                                    case Card.Type.WATER:
                                        BioPoints += 3;
                                        break;
                                    case Card.Type.FIRE:
                                    case Card.Type.WIND:
                                        BioPoints -= 1;
                                        break;
                                }
                                break;
                            case Card.Type.PLAINS:
                                CyclePoints += 2;
                                CheckObtainedHabitats("Plant Field", 6);
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if below placed position is a card and do appropriate action based on its type
                    if (belowCard != null)
                    {
                        switch (belowCard.type)
                        {
                            case Card.Type.PLANT:
                                break;
                            case Card.Type.WATER:
                                CyclePoints += 1;
                                DrawCards(1);
                                break;
                            case Card.Type.WIND:
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                CheckObtainedHabitats("Tree Mountain", 1);
                                break;
                            case Card.Type.PLAINS:
                                CheckObtainedHabitats("Underground Root Tunnels", 4);
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if left of placed position is a card and do appropriate action based on its type
                    if (leftCard != null)
                    {
                        switch (leftCard.type)
                        {
                            case Card.Type.PLANT:
                                break;
                            case Card.Type.WATER:
                                HazardPoints -= 3;
                                CheckObtainedHabitats("Wetlands", 3);
                                break;
                            case Card.Type.WIND:
                                if (true)//(Board.GetCard(xPosition + 1, yPosition) == null)
                                {
                                    //Place extra plant card at xPosition+1, yPosition;
                                }
                                else
                                {
                                    BioPoints -= 1;
                                }
                                break;
                            case Card.Type.FIRE:
                                BioPoints -= 2;
                                DiscardCard(Card.Type.PLANT);
                                break;
                            case Card.Type.MOUNTAIN:
                                switch (Card.Type.FIRE)//Board.GetCard(xPosition - 1, yPosition + 1).type)
                                {
                                    case Card.Type.WATER:
                                        BioPoints += 3;
                                        break;
                                    case Card.Type.FIRE:
                                    case Card.Type.WIND:
                                        BioPoints -= 1;
                                        break;
                                }
                                break;
                            case Card.Type.PLAINS:
                                CyclePoints += 2;
                                CheckObtainedHabitats("Plant Field", 6);
                                break;
                            default:
                                break;
                        }
                    }

                    break;
                #endregion
                #region Placed Water Card
                case Card.Type.WATER:
                    HeatPoints -= 1;
                    //Check if above placed position is a card and do appropriate action based on its type
                    if (aboveCard != null)
                    {
                        switch (aboveCard.type)
                        {
                            case Card.Type.PLANT:
                                CyclePoints += 1;
                                DrawCards(1);
                                break;
                            case Card.Type.WATER:
                                break;
                            case Card.Type.WIND:
                                HeatPoints -= 1;
                                CheckObtainedHabitats("Ocean Surface w/ Temperature-Regulating Air Currents", 2);
                                break;
                            case Card.Type.FIRE:
                                CyclePoints += 2;
                                break;
                            case Card.Type.MOUNTAIN:
                                HazardPoints -= 1;
                                BioPoints += 1;
                                CyclePoints += 1;
                                break;
                            case Card.Type.PLAINS:
                                HeatPoints += 1;
                                CyclePoints += 1;
                                CheckObtainedHabitats("Desert Oasis", 8);
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if right of placed position is a card and do appropriate action based on its type
                    if (rightCard != null)
                    {
                        switch (rightCard.type)
                        {
                            case Card.Type.PLANT:
                                HazardPoints -= 3;
                                CheckObtainedHabitats("Wetlands", 3);
                                break;
                            case Card.Type.WATER:
                                break;
                            case Card.Type.WIND:
                                CyclePoints += 2;
                                CheckObtainedHabitats("Ocean Currents", 1);
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                BioPoints += 1;
                                CheckObtainedHabitats("Lakes and Rivers", 4);
                                break;
                            case Card.Type.PLAINS:
                                CyclePoints += 3;
                                CheckObtainedHabitats("Canyon", 1);
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if below placed position is a card and do appropriate action based on its type
                    if (belowCard != null)
                    {
                        switch (belowCard.type)
                        {
                            case Card.Type.PLANT:
                                BioPoints += 2;
                                CheckObtainedHabitats("Kelp Forest", 3);
                                break;
                            case Card.Type.WATER:
                                break;
                            case Card.Type.WIND:
                                switch (Card.Type.PLANT)//Board.GetCard(xPosition, yPosition-2).type)
                                {
                                    case Card.Type.PLANT:
                                        HazardPoints -= 1;
                                        BioPoints += 2;
                                        CheckObtainedHabitats("Rainforest", 7);
                                        break;
                                    case Card.Type.WATER:
                                        HazardPoints += 3;
                                        DiscardCard(Card.Type.WATER);
                                        break;
                                    case Card.Type.FIRE:
                                        HeatPoints += 1;
                                        CyclePoints += 1;
                                        break;
                                    case Card.Type.MOUNTAIN:
                                        HeatPoints -= 3;
                                        CheckObtainedHabitats("Snow", 2);
                                        break;
                                    case Card.Type.PLAINS:
                                        CyclePoints += 4;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case Card.Type.FIRE:
                                HazardPoints += 2;
                                HeatPoints += 1;
                                CyclePoints += 3;
                                CheckObtainedHabitats("Volcanic Vents", 1);
                                break;
                            case Card.Type.MOUNTAIN:
                                CyclePoints += 2;
                                CheckObtainedHabitats("Waterfalls & Mountain Streams", 2);
                                break;
                            case Card.Type.PLAINS:
                                hazardPoints += 2;
                                CyclePoints -= 2;
                                DrawCards(2);
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if left of placed position is a card and do appropriate action based on its type
                    if (leftCard != null)
                    {
                        switch (leftCard.type)
                        {
                            case Card.Type.PLANT:
                                HazardPoints -= 3;
                                CheckObtainedHabitats("Wetlands", 3);
                                break;
                            case Card.Type.WATER:
                                break;
                            case Card.Type.WIND:
                                CyclePoints += 2;
                                CheckObtainedHabitats("Ocean Currents", 1);
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                BioPoints += 1;
                                CheckObtainedHabitats("Lakes and Rivers", 4);
                                break;
                            case Card.Type.PLAINS:
                                CyclePoints += 3;
                                CheckObtainedHabitats("Canyon", 1);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                #endregion
                #region Placed Wind Card
                case Card.Type.WIND:
                    CheckForCopies();
                    //Check if above placed position is a card and do appropriate action based on its type
                    if (aboveCard != null)
                    {
                        switch (aboveCard.type)
                        {
                            case Card.Type.PLANT:
                                //Nothing happens
                                break;
                            case Card.Type.WATER:
                                switch (Card.Type.PLANT)//Board.GetCard(xPosition, yPosition-1).type)
                                {
                                    case Card.Type.PLANT:
                                        HazardPoints -= 1;
                                        BioPoints += 2;
                                        CheckObtainedHabitats("Rainforest", 7);
                                        break;
                                    case Card.Type.WATER:
                                        HazardPoints += 3;
                                        DiscardCard(Card.Type.WATER);
                                        break;
                                    case Card.Type.FIRE:
                                        HeatPoints += 1;
                                        CyclePoints += 1;
                                        break;
                                    case Card.Type.MOUNTAIN:
                                        HeatPoints -= 3;
                                        CheckObtainedHabitats("Snow", 2);
                                        break;
                                    case Card.Type.PLAINS:
                                        CyclePoints += 4;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case Card.Type.WIND:
                                break;
                            case Card.Type.FIRE:
                                HazardPoints -= 1;
                                DrawCards(1);
                                break;
                            case Card.Type.MOUNTAIN:
                                HazardPoints += 1;
                                CyclePoints -= 2;
                                break;
                            case Card.Type.PLAINS:
                                //Nothing happens
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if right of placed position is a card and do appropriate action based on its type
                    if (rightCard != null)
                    {
                        switch (rightCard.type)
                        {
                            case Card.Type.PLANT:
                                if (true)//(Board.GetCard(xPosition + 2, yPosition) == null)
                                {
                                    //Place extra plant card at xPosition+2, yPosition;
                                }
                                else
                                {
                                    BioPoints -= 1;
                                }
                                break;
                            case Card.Type.WATER:
                                CyclePoints += 2;
                                CheckObtainedHabitats("Ocean Currents", 1);
                                break;
                            case Card.Type.WIND:
                                break;
                            case Card.Type.FIRE:
                                HazardPoints += 2;
                                HeatPoints += 2;
                                if (true)//Board.GetCard(xPosition-1, yPosition).type == Card.Type.PLANT)
                                {
                                    BioPoints -= 2;
                                    DiscardCard(Card.Type.PLANT);
                                }
                                break;
                            case Card.Type.MOUNTAIN:
                                CheckObtainedHabitats("Caves", 1);
                                break;
                            case Card.Type.PLAINS:
                                HeatPoints += 1;
                                CheckObtainedHabitats("Sand Dunes", 2);
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if below placed position is a card and do appropriate action based on its type
                    if (belowCard != null)
                    {
                        switch (belowCard.type)
                        {
                            case Card.Type.PLANT:
                                DrawCards(1);
                                break;
                            case Card.Type.WATER:
                                HeatPoints -= 1;
                                CheckObtainedHabitats("Ocean Surface w/ Temperature-Regulating Air Currents", 2);
                                break;
                            case Card.Type.WIND:
                                break;
                            case Card.Type.FIRE:
                                CyclePoints += 1;
                                CheckObtainedHabitats("Updraft", 2);
                                break;
                            case Card.Type.MOUNTAIN:
                                CyclePoints += 2;
                                break;
                            case Card.Type.PLAINS:
                                BioPoints += 1;
                                CyclePoints += 1;
                                CheckObtainedHabitats("Flower Field", 4);
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if left of placed position is a card and do appropriate action based on its type
                    if (leftCard != null)
                    {
                        switch (leftCard.type)
                        {
                            case Card.Type.PLANT:
                                if (true)//(Board.GetCard(xPosition - 2, yPosition) == null)
                                {
                                    //Place extra plant card at xPosition-2, yPosition;
                                }
                                else
                                {
                                    BioPoints -= 1;
                                }
                                break;
                            case Card.Type.WATER:
                                CyclePoints += 2;
                                CheckObtainedHabitats("Ocean Currents", 1);
                                break;
                            case Card.Type.WIND:
                                break;
                            case Card.Type.FIRE:
                                HazardPoints += 2;
                                HeatPoints += 2;
                                if (true)//Board.GetCard(xPosition+1, yPosition).type == Card.Type.PLANT)
                                {
                                    BioPoints -= 2;
                                    DiscardCard(Card.Type.PLANT);
                                }
                                break;
                            case Card.Type.MOUNTAIN:
                                CheckObtainedHabitats("Caves", 1);
                                break;
                            case Card.Type.PLAINS:
                                HeatPoints += 1;
                                CheckObtainedHabitats("Sand Dunes", 2);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                #endregion
                #region Placed Fire Card
                case Card.Type.FIRE:
                    HeatPoints += 1;
                    //Check if above placed position is a card and do appropriate action based on its type
                    if (aboveCard != null)
                    {
                        switch (aboveCard.type)
                        {
                            case Card.Type.PLANT:
                                //Nothing happens.
                                break;
                            case Card.Type.WATER:
                                HazardPoints += 2;
                                HeatPoints += 1;
                                CyclePoints += 3;
                                CheckObtainedHabitats("Volcanic Vents", 1);
                                break;
                            case Card.Type.WIND:
                                CyclePoints += 1;
                                CheckObtainedHabitats("Updraft", 2);
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                BioPoints += 1;
                                break;
                            case Card.Type.PLAINS:
                                HazardPoints += 3;
                                CyclePoints += 1;
                                DrawCards(1);
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if right of placed position is a card and do appropriate action based on its type
                    if (rightCard != null)
                    {
                        switch (rightCard.type)
                        {
                            case Card.Type.PLANT:
                                BioPoints -= 2;
                                DiscardCard(Card.Type.PLANT);
                                break;
                            case Card.Type.WATER:
                                //Nothing happens.
                                break;
                            case Card.Type.WIND:
                                HazardPoints += 2;
                                HeatPoints += 2;
                                if (true)//Board.GetCard(xPosition+2, yPosition).type == Card.Type.PLANT)
                                {
                                    BioPoints -= 2;
                                    DiscardCard(Card.Type.PLANT);
                                }
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                HeatPoints += 1;
                                HazardPoints += 1;
                                break;
                            case Card.Type.PLAINS:
                                HazardPoints += 1;
                                DiscardCard(Card.Type.PLANT);
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if below placed position is a card and do appropriate action based on its type
                    if (belowCard != null)
                    {
                        switch (belowCard.type)
                        {
                            case Card.Type.PLANT:
                                BioPoints += 1;
                                DrawCards(1);
                                break;
                            case Card.Type.WATER:
                                CyclePoints += 2;
                                break;
                            case Card.Type.WIND:
                                HazardPoints -= 1;
                                DrawCards(1);
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                HeatPoints += 2;
                                HazardPoints += 6;
                                CyclePoints -= 3;
                                break;
                            case Card.Type.PLAINS:
                                HeatPoints += 1;
                                CheckObtainedHabitats("Rocky Desert", 2);
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if left of placed position is a card and do appropriate action based on its type
                    if (leftCard != null)
                    {
                        switch (leftCard.type)
                        {
                            case Card.Type.PLANT:
                                BioPoints -= 2;
                                DiscardCard(Card.Type.PLANT);
                                break;
                            case Card.Type.WATER:
                                //Nothing happens.
                                break;
                            case Card.Type.WIND:
                                HazardPoints += 2;
                                HeatPoints += 2;
                                if (true)//Board.GetCard(xPosition-2, yPosition).type == Card.Type.PLANT)
                                {
                                    BioPoints -= 2;
                                    DiscardCard(Card.Type.PLANT);
                                }
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                HeatPoints += 1;
                                HazardPoints += 1;
                                break;
                            case Card.Type.PLAINS:
                                HazardPoints += 1;
                                DiscardCard(Card.Type.PLANT);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                #endregion
                #region Placed Mountain Card
                case Card.Type.MOUNTAIN:
                    CyclePoints += 1;
                    //Check if above placed position is a card and do appropriate action based on its type
                    if (aboveCard != null)
                    {
                        switch (aboveCard.type)
                        {
                            case Card.Type.PLANT:
                                CheckObtainedHabitats("Mountain")
                                break;
                            case Card.Type.WATER:
                                break;
                            case Card.Type.WIND:
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                break;
                            case Card.Type.PLAINS:
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if right of placed position is a card and do appropriate action based on its type
                    if (rightCard != null)
                    {
                        switch (rightCard.type)
                        {
                            case Card.Type.PLANT:
                                break;
                            case Card.Type.WATER:
                                break;
                            case Card.Type.WIND:
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                break;
                            case Card.Type.PLAINS:
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if below placed position is a card and do appropriate action based on its type
                    if (belowCard != null)
                    {
                        switch (belowCard.type)
                        {
                            case Card.Type.PLANT:
                                break;
                            case Card.Type.WATER:
                                break;
                            case Card.Type.WIND:
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                break;
                            case Card.Type.PLAINS:
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if left of placed position is a card and do appropriate action based on its type
                    if (leftCard != null)
                    {
                        switch (leftCard.type)
                        {
                            case Card.Type.PLANT:
                                break;
                            case Card.Type.WATER:
                                break;
                            case Card.Type.WIND:
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                break;
                            case Card.Type.PLAINS:
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                #endregion
                #region Placed Plains Card
                case Card.Type.PLAINS:
                    DrawCards(1);
                    CheckForCopies(PlayerHand.Last());
                    //Check if above placed position is a card and do appropriate action based on its type
                    if (aboveCard != null)
                    {
                        switch (aboveCard.type)
                        {
                            case Card.Type.PLANT:
                                break;
                            case Card.Type.WATER:
                                break;
                            case Card.Type.WIND:
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                break;
                            case Card.Type.PLAINS:
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if right of placed position is a card and do appropriate action based on its type
                    if (rightCard != null)
                    {
                        switch (rightCard.type)
                        {
                            case Card.Type.PLANT:
                                break;
                            case Card.Type.WATER:
                                break;
                            case Card.Type.WIND:
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                break;
                            case Card.Type.PLAINS:
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if below placed position is a card and do appropriate action based on its type
                    if (belowCard != null)
                    {
                        switch (belowCard.type)
                        {
                            case Card.Type.PLANT:
                                break;
                            case Card.Type.WATER:
                                break;
                            case Card.Type.WIND:
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                break;
                            case Card.Type.PLAINS:
                                break;
                            default:
                                break;
                        }
                    }

                    //Check if left of placed position is a card and do appropriate action based on its type
                    if (leftCard != null)
                    {
                        switch (leftCard.type)
                        {
                            case Card.Type.PLANT:
                                break;
                            case Card.Type.WATER:
                                break;
                            case Card.Type.WIND:
                                break;
                            case Card.Type.FIRE:
                                break;
                            case Card.Type.MOUNTAIN:
                                break;
                            case Card.Type.PLAINS:
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                #endregion
                default:
                    break;
            }
        }

        /// <summary>
        /// Checks to see if the player has created the given habitat.
        /// If not, it gives them the points and adds the habitat to the obtained habitats
        /// </summary>
        /// <param name="habitat">habitat to be checked</param>
        /// <param name="pointValue">point value of the habitat</param>
        private void CheckObtainedHabitats(string habitat, int pointValue)
        {
            if (ObtainedHabitats.ContainsKey(habitat))
            {
                if (!ObtainedHabitats[habitat])
                {
                    HabitatPoints += pointValue;
                    ObtainedHabitats[habitat] = true;
                }
            }
            else
            {
                throw new ArgumentException($"ObtainedHabitats doesn't have the habitat {habitat}");
            }
        }

        /// <summary>
        /// Checks for the card type with the most copies in the players hand and discards one card of that type.
        /// Primarily used by the wind card.
        /// </summary>
        private void CheckForCopies()
        {
            Card.Type? mostCopies = null;
            int numOfCopies = 0;

            for (int i = 0; i < Enum.GetValues(typeof(Card.Type)).Length; i++)
            {
                Card.Type currentType = (Card.Type)Enum.GetValues(typeof(Card.Type)).GetValue(i);
                int currentNumOfcopies = PlayerHand.FindAll(card => card.type == ((Card.Type)Enum.GetValues(typeof(Card.Type)).GetValue(i))).Count;
                if (currentNumOfcopies > numOfCopies)
                {
                    mostCopies = currentType;
                    numOfCopies = currentNumOfcopies;
                }
            }
            DiscardCard(mostCopies);
            DrawCards(1);
        }

        /// <summary>
        /// Checks for copies of a given card type and if more than 1 is found, discards one of that card.
        /// </summary>
        /// <param name="checkedType">The type of card to be searched for.</param>
        private void CheckForCopies(Card.Type checkedType)
        {
            if (PlayerHand.FindAll(card => card.type == checkedType).Count > 1)
            {
                DiscardCard(checkedType);
            }
        }

        /// <summary>
        /// Discards a card of the given type from the player's hand.
        /// If the player doesn't have a card of that type, it does nothing.
        /// </summary>
        /// <param name="type">Type of card to be discarded.</param>
        private void DiscardCard(Card.Type? type)
        {
            if (PlayerHand.FindIndex(card => card.type == type) > -1)
            {
                PlayerHand[PlayerHand.FindIndex(card => card.type == type)] = null;
            }
        }
    }
}
