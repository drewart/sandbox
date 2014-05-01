using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHands
{
    /// <summary>
    /// HandEval class to evaluate a hand of poker and give it a rank
    /// </summary>
    public class HandEval
    {

        // rank enum ordered by hand value
        public enum HandRank
        {
            Nothing,
            Pair,
            TwoPair,
            ThreeKind,
            Straight,
            Flush,
            FullHouse,
            FourKind,
            StraightFlush
        }

        // rank backer
        private HandRank rank;

        /// <summary>
        /// composite class to halp with matches
        /// </summary>
        public MatchHelper MatchHelper { get; private set; }

        /// <summary>
        /// rank of the hand
        /// </summary>
        public HandRank Rank
        {
            get { return rank; }
        }

        /// <summary>
        /// highest card in match or set
        /// </summary>
        public Card HighCard { get; private set; }

        /// <summary>
        /// bool if there is a match i.e. 4 kind,3 kind,2 pair pair 
        /// </summary>
        public bool HasMatch { get { return (MatchHelper.MatchCount > 1); }}

        // private array
        private Card[] cards;


        /// <summary>
        /// class to evaluate the hand
        /// </summary>
        /// <param name="cards">array of 5 cards</param>
        public HandEval(Card[] cards)
        {
            this.cards = cards;
            
            MatchHelper = new MatchHelper(cards);
            rank = EvaluateHandRank();

            SetHighCard();
        }

        /// <summary>
        /// sets the high card in a match or non match set
        /// </summary>
        private void SetHighCard()
        {
            // if has pair or greater match
            if (HasMatch)
                HighCard = MatchHelper.FirstSet.First();
            else
                HighCard = cards.OrderBy(c => c.Rank).Last();
        }


        /// <summary>
        /// evaluates card ranking
        /// </summary>
        /// <returns>rank of hand</returns>
        private HandRank EvaluateHandRank()
        {
            // go from highest rank to lowest rank
            if (HasStraightFlush())
                return HandRank.StraightFlush;
            
            if (MatchHelper.MatchCount == 4)
                return HandRank.FourKind;
            
            if (MatchHelper.SetCount == 2 && (MatchHelper.FirstSet.Length == 3 && MatchHelper.SecondSet.Length == 2))
                return HandRank.FullHouse;
            
            if (HasFlush())
                return HandRank.Flush;
            
            if (HasStraight())
                return HandRank.Straight;
            
            if (MatchHelper.SetCount == 1 && MatchHelper.MatchCount == 3)
                return HandRank.ThreeKind;
            
            if (MatchHelper.SetCount == 2 && MatchHelper.MatchCount == 2 && (MatchHelper.FirstSet.Length == 2 && MatchHelper.SecondSet.Length == 2))
                return HandRank.TwoPair;
            
            if (MatchHelper.SetCount == 1 && MatchHelper.FirstSet.Length == 2)
                return HandRank.Pair;

            return HandRank.Nothing;
        }


        #region Has Hand Methods
        // if is straight && flush
        public bool HasStraightFlush()
        {
            return (HasFlush() && HasStraight());
        }


        public bool HasStraight()
        {
            var rankOrder = cards.OrderBy(c => c.Rank).ToArray();
            // first should be i sub 0 = i sub 1 + 1 to i sub 4
            for (int i = 1; i < rankOrder.Length; i++)
            {
                if (rankOrder[0].Rank + i != rankOrder[i].Rank)
                    return false;
            }
            return true;
        }

        public bool HasFlush()
        {
            for (int i = 1; i < cards.Length; i++)
            {
                // prove it's a flush
                if (cards[0].Suit != cards[i].Suit) // straight
                    return false;
            }
            return true;

        }

        #endregion





    }// end class
}// end ns