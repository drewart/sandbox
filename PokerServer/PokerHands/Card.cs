using System;

using System.Text.RegularExpressions;

namespace PokerHands
{
    public class Card : IComparable
    {

        // card pattern
        static Regex CardPattern = new Regex("([2-9]|T|J|Q|K|A)(C|D|H|S)",RegexOptions.Compiled);

        // original input string of card
        private String cardString;

        /// <summary>
        /// Suit of the card valid char C,D,H,S
        /// </summary>
        public Char Suit { get; private set; }

        // card value 2 to Ace
        public Char Value { get; private set; }
        
        // Rank is a numarical value of the card
        public int Rank { get; private set; }

        /// <summary>
        /// Card with the following input Regex("([2-9]|T|J|Q|K|A)(C|D|H|S)")
        /// </summary>
        /// <param name="card">2 string with Card Value & suit chars</param>
        public Card(String card)
        {
            // validate card input
            Match match = CardPattern.Match(card);
            if (!CardPattern.IsMatch(card))
                throw new ArgumentException("card {0} is not a valid card", card);

            cardString = card;

            // get value pattern
            Value = match.Groups[1].Value[0];
            // get suit pattern
            Suit = match.Groups[2].Value[0];

            Rank = GenerateRank(Value);

        }

        /// <summary>
        /// gets a numarical value of card
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int GenerateRank(Char value)
        {
            int rank = -1;

            switch (value)
            {
                case 'T':
                    rank = 10;
                    break;
                case 'J':
                    rank = 11;
                    break;
                case 'Q':
                    rank = 12;
                    break;
                case 'K':
                    rank = 13;
                    break;
                case 'A':
                    rank = 14;
                    break;
                default:
                    rank = value - '0';
                    break;
            }

            return rank;

        }

        /// <summary>
        /// String Value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            return cardString;
        }

        /// <summary>
        /// way to compare the cards
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            var card = obj as Card;

            if (Suit == card.Suit && Rank == card.Rank)
                return 0;
            if (Suit > card.Suit)
                return 1;
            if (Suit < card.Suit)
                return -1;
            if (Rank < card.Rank)
                return -1;
            return 1;
        }
    }
}
