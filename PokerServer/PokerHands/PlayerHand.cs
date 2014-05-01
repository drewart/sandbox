using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHands
{
    public class PlayerHand : IComparable
    {
        public const int MAX_CARDS = 5;

        /// <summary>
        /// Name of the player
        /// </summary>
        public String PlayerName { get; private set; }

        /// <summary>
        /// Hand of cards
        /// </summary>
        public Card[] Hand { get; private set; }

        /// <summary>
        /// hand evaluation object
        /// </summary>
        public HandEval HandEvaluation { get; protected set; }

        /// <summary>
        /// Player hand is the player
        /// </summary>
        /// <param name="playerName">name of player</param>
        /// <param name="hand">string of cards comma seperated</param>
        public PlayerHand(String playerName, String hand)
        {
            this.PlayerName = playerName;

            String[] cardStrings = hand.Split(new char[] {','});

            // check hand size
            if (cardStrings.Length > MAX_CARDS || cardStrings.Length < 0)
                throw new ArgumentException(string.Format("{0} hand has too many values: {1}",playerName,hand));

            Hand = new Card[MAX_CARDS];

            for(int i = 0; i < MAX_CARDS; i++)
            {
                Hand[i] = new Card(cardStrings[i]);
            }
            // sort hand
            Array.Sort(Hand);

            HandEvaluation = new HandEval(Hand);
        }

        /// <summary>
        /// compare two hands
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj.GetType() != typeof(PlayerHand))
                throw new ArgumentException("Can only compary PlayerHands");

            var playerHand = obj as PlayerHand;

            if (playerHand == null)
                return 1;

            // compare hand rank via HandRank enum
            int rankCompare = this.HandEvaluation.Rank.CompareTo(playerHand.HandEvaluation.Rank);

            // if same type of hands evalute high card
            if (rankCompare == 0)
            {
                // handle full house, two pair, pair
                if (HandEvaluation.HasMatch)
                {
                    return CompareHighCard(playerHand);
                }

                return CompareHandRank(playerHand);
            }

            return rankCompare;
        }


        /// <summary>
        /// Compare for StraightFlush,Flush,Straight,nothing
        /// </summary>
        /// <param name="playerHand"></param>
        /// <returns></returns>
        private int CompareHandRank(PlayerHand playerHand)
        {
            Card[] pRankCards = playerHand.Hand.OrderBy(c => c.Rank).ToArray();
            Card[] rankCards = Hand.OrderBy(c => c.Rank).ToArray();

            for (int i = MAX_CARDS-1; i >= 0; i--)
            {
                int compareRank = rankCards[i].Rank.CompareTo(pRankCards[i].Rank);
                if (compareRank != 0)
                    return compareRank;
            }
            return 0;
        }


        /// <summary>
        /// handles compare of FullHouse, two pair, pair
        /// </summary>
        /// <param name="playerHand"></param>
        /// <returns></returns>
        private int CompareHighCard(PlayerHand playerHand)
        {
            int highCardCompare = HandEvaluation.HighCard.Rank.CompareTo(playerHand.HandEvaluation.HighCard.Rank);

            // if highCard ditermines return
            if (highCardCompare != 0)
                return highCardCompare;
            
            // check for second set match
            if (HandEvaluation.MatchHelper.SetCount > 1 && HandEvaluation.MatchHelper.SetCount == playerHand.HandEvaluation.MatchHelper.SetCount)
            {
                var pSecondHighest = playerHand.HandEvaluation.MatchHelper.SecondSet.First();
                int secondHighestCmp = HandEvaluation.MatchHelper.SecondSet.First().Rank.CompareTo(pSecondHighest.Rank);
                
                // second set match if two pair, need to compare kicker
                if (secondHighestCmp == 0)
                    return CompareHandRank(playerHand);

                return secondHighestCmp;
            }
            else
            {
                return CompareHandRank(playerHand);
            }
        }

    }
}
