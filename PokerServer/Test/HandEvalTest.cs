using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerHands;

namespace Test
{
    [TestClass]
    public class HandEvalTest
    {

        private Card[] GenCards(string str)
        {
            var player = new PlayerHand("foo", str);
            return player.Hand;
        }


        [TestMethod]
        public void TestTwoPair()
        {
            Card[] cards = GenCards("5C,5D,2C,2D,JS");
            var eval = new HandEval(cards);
            Assert.AreEqual(2, eval.MatchHelper.FirstSet.Length);
            Assert.AreEqual(2, eval.MatchHelper.SecondSet.Length);
            Assert.AreEqual(5,eval.MatchHelper.FirstSet[0].Rank);
            Assert.AreEqual(2,eval.MatchHelper.SecondSet[0].Rank);
            Assert.AreEqual(eval.Rank,HandEval.HandRank.TwoPair);
        }

        [TestMethod]
        public void TestEvalFullHouse()
        {
            Card[] cards = GenCards("5C,5D,5S,2D,2S");
            var eval = new HandEval(cards);
            Assert.AreEqual(eval.Rank, HandEval.HandRank.FullHouse);
            Assert.AreEqual(eval.HighCard.ToString(),"5C");
        }


        [TestMethod]
        public void TestEvalStraightFlush()
        {
            // low
            Card[] cards = GenCards("2C,3C,4C,5C,6C");
            var eval = new HandEval(cards);
            Assert.AreEqual(eval.Rank, HandEval.HandRank.StraightFlush);
            Assert.AreEqual(eval.HighCard.ToString(), "6C");

            // high
            cards = GenCards("TC,JC,QC,KC,AC");
            eval = new HandEval(cards);

            Assert.AreEqual(eval.Rank, HandEval.HandRank.StraightFlush);


        }

        [TestMethod]
        public void TestHandRank()
        {
            Assert.IsTrue(HandEval.HandRank.Pair < HandEval.HandRank.TwoPair);
        }

 

        
    }
}
