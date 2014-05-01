using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHands
{
    /// <summary>
    /// class to help with matching cards
    /// </summary>
    public class MatchHelper
    {
        // local card set
        private Card[] cards;

        /// <summary>
        /// 1st set of matches
        /// </summary>
        public Card[] FirstSet { get; private set; }
        
        /// <summary>
        /// 2nd set of matches
        /// </summary>
        public Card[] SecondSet { get; private set; }

        /// <summary>
        /// highest # of matches four kind, three, two
        /// </summary>
        public int MatchCount { get { return FirstSet.Length; } }

        /// <summary>
        /// how many match sets 1 or 2 for two pair and full house
        /// </summary>
        public int SetCount
        {
            get
            {
                if (SecondSet.Length > 1)
                    return 2;
                return FirstSet.Length > 1 ? 1 : 0;
            }
        }


        /// <summary>
        /// MatchHelper class to evaluate card matching
        /// </summary>
        /// <param name="cards">hand (assumes 5 card hand)</param>
        public MatchHelper(Card[] cards)
        {
            this.cards = cards;
            FirstSet = new Card[0];
            SecondSet = new Card[0];
            
            FindMatches();

        }
            
        
        /// <summary>
        /// finds card matches by rank hash
        /// </summary>
        private void FindMatches()
        {
            var cardDictionary = new Dictionary<int, List<Card>>();

            foreach (Card c in cards)
            {
                int rankKey = c.Rank;
                if (!cardDictionary.ContainsKey(rankKey))
                    cardDictionary.Add(rankKey, new List<Card>());

                cardDictionary[rankKey].Add(c);

                // get first set/second set
                Card[] currentMatches = cardDictionary[rankKey].ToArray();

                // no matches go to next
                if (currentMatches.Length < 2)
                    continue;

                
                // set first set should be longest and highest
                if (currentMatches.Length > FirstSet.Length ||
                    (FirstSet.Length > 0 && FirstSet.Length == currentMatches.Length && 
                    currentMatches.Last().CompareTo(FirstSet.Last()) > 0))
                {
                    // swap first to second if FirstSet is not sub set of current
                    if (FirstSet.Length > 0 && !currentMatches.Contains(FirstSet.First()))
                        SecondSet = FirstSet;
                    
                    FirstSet = currentMatches.ToArray();
                } 
                // else set SecondSet if not subset of first
                else if (currentMatches.Length > SecondSet.Length && !currentMatches.Contains(FirstSet.First()))
                {
                    SecondSet = currentMatches.ToArray();
                }
                
            }
        }// end FindMatches

    }
}
