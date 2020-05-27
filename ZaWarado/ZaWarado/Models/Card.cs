using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ZaWarado.Models
{
    /// <summary>
    /// Represents a single card in the game. Its main property is its type, since its image is
    /// determined based on which type it is.
    /// </summary>
    public class Card
    {
        private static ImageSource[] cardImages = new ImageSource[Enum.GetValues(typeof(Type)).Length];

        /// <summary>
        /// Represents the type of a Card. A Card's individual effect, and the effects that occur
        /// based on adjacent cards, are determined by its type.
        /// </summary>
        public enum Type
        {
            PLANT,
            WATER,
            WIND,
            FIRE,
            MOUNTAIN,
            PLAINS
        }

        /// <summary>
        /// Which type of Card this is, such as PLANT. Use this for determining effects that
        /// occur when this Card is played. A Card's type never changes after it's created,
        /// so this property is read only.
        /// </summary>
        public Type type { get; private set; }

        /// <summary>
        /// The source image file that will be used to display this Card. The actual display
        /// is handled by an Image object. The file used is determined based on the Card's
        /// type, so this property is read only.
        /// </summary>
        public ImageSource imageFile
        {
            get
            {
                // Add code later for loading in an image if it hasn't been loaded yet
                int typeDex = (int)type;
                if (cardImages[typeDex] == null)
                {
                    switch (typeDex)
                    {
                        case 0:
                            cardImages[0] = new BitmapImage(new Uri("../../Assets/Images/Plant.png", UriKind.Relative));
                            break;
                        case 1:
                            cardImages[1] = new BitmapImage(new Uri("../../Assets/Images/Water.png", UriKind.Relative));
                            break;
                        case 2:
                            cardImages[2] = new BitmapImage(new Uri("../../Assets/Images/Wind.png", UriKind.Relative));
                            break;
                        case 3:
                            cardImages[3] = new BitmapImage(new Uri("../../Assets/Images/Fire.png", UriKind.Relative));
                            break;
                        case 4:
                            cardImages[4] = new BitmapImage(new Uri("../../Assets/Images/Mountain.png", UriKind.Relative));
                            break;
                        case 5:
                            cardImages[5] = new BitmapImage(new Uri("../../Assets/Images/Plains.png", UriKind.Relative));
                            break;
                    }
                }

                return cardImages[typeDex];
            }
        }

        /// <summary>
        /// Creates a new Card with a given type. Remember, the type is permanent once set.
        /// </summary>
        /// <param name="type">The type of Card to create.</param>
        public Card(Type type)
        {
            this.type = type;
        }

        /// <summary>
        /// Treats a Card as if it were its type.
        /// </summary>
        /// <param name="card">The Card to use as its type.</param>
        public static implicit operator Type(Card card) 
        {
            return card.type;
        }
    }
}
