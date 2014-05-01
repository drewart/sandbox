using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerHands;

namespace Test
{
    [TestClass]
    public class PlayerHandTest
    {


        [TestMethod]
        public void TestPlayerHandSix()
        {
            try
            {
                var player = new PlayerHand("Drew", "5C,3C,2C,6C,4C,4D");
                Assert.Fail("ArgumentException not thrown");
            }
            catch (ArgumentException)
            {
                // should be true
            }
            catch (Exception)
            {
                Assert.Fail("should not hit exception");
            }

        }

        [TestMethod]
        public void TestPlayerHandSort()
        {
            var player = new PlayerHand("Drew", "5C,3C,2C,6C,4C");

            Card[] cards = player.Hand;
            Assert.AreEqual(cards[0].ToString(),"2C");
            Assert.AreEqual(cards[1].ToString(), "3C");
            Assert.AreEqual(cards[2].ToString(), "4C");
            Assert.AreEqual(cards[3].ToString(), "5C");
            Assert.AreEqual(cards[4].ToString(), "6C");
        }

        [TestMethod]
        public void TestPlayerHandSuitSort()
        {
            var player = new PlayerHand("Drew", "5C,3C,2D,6S,4H");

            Card[] cards = player.Hand;
            Assert.AreEqual(cards[0].ToString(), "3C");
            Assert.AreEqual(cards[1].ToString(), "5C");
            Assert.AreEqual(cards[2].ToString(), "2D");
            Assert.AreEqual(cards[3].ToString(), "4H");
            Assert.AreEqual(cards[4].ToString(), "6S");
        }



        [TestMethod]
        public void TestPlayerHandCompareNothingVsTwoPair()
        {
            var player1 = new PlayerHand("Foo", "AC,3S,8H,6C,4S");  // nothing
            var player2 = new PlayerHand("Bar", "5C,5H,2C,8D,TD");  // pair

            Assert.AreEqual(-1, player1.CompareTo(player2));
            Assert.AreEqual(1,player2.CompareTo(player1));

        }

        [TestMethod]
        public void TestPlayerHandFullHouse()
        {
            var p1 = new PlayerHand("fullhouse", "TD,TC,TH,8H,8D");  // FullHouse
            
            Assert.AreEqual(2,p1.HandEvaluation.MatchHelper.SetCount, "SetCount not set corretly");
            Assert.AreEqual(3,p1.HandEvaluation.MatchHelper.FirstSet.Length);
            Assert.AreEqual(2, p1.HandEvaluation.MatchHelper.SecondSet.Length);

            Assert.AreEqual(HandEval.HandRank.FullHouse, p1.HandEvaluation.Rank);

        }

        [TestMethod]
        public void TestPlayerHandThreeKind()
        {
            var p1 = new PlayerHand("threekind", "5C,5H,5D,2D,TD");  // FullHouse

            Assert.AreEqual(1, p1.HandEvaluation.MatchHelper.SetCount, "SetCount not set corretly");
            Assert.AreEqual(3, p1.HandEvaluation.MatchHelper.FirstSet.Length);
            Assert.AreEqual(0, p1.HandEvaluation.MatchHelper.SecondSet.Length);

            Assert.AreEqual(HandEval.HandRank.ThreeKind, p1.HandEvaluation.Rank);

        }


        [TestMethod]
        public void TestPlayerHandStraight()
        {
            var p1 = new PlayerHand("straight", "9C,TH,JD,QC,KS");  // FullHouse

            Assert.AreEqual(0, p1.HandEvaluation.MatchHelper.SetCount, "SetCount not set corretly");

            Assert.AreEqual(HandEval.HandRank.Straight, p1.HandEvaluation.Rank);

        }



        [TestMethod]
        public void TestPlayerHandCompareHands()
        {
            String[] handOrder =
            {
                "nothing", "pair", "twopair", "threekind", "straight", "flush", "fullhouse", "fourkind",
                "straightflush"
            };

            var p1 = new PlayerHand("nothing", "AC,3S,8H,6C,4S");  // nothing
            var p2 = new PlayerHand("pair", "5C,5H,2C,8D,TD");  // pair
            var p3 = new PlayerHand("twopair", "5C,5H,2C,2D,TD");  // twopair
            var p4 = new PlayerHand("threekind", "5C,5H,5D,2D,TD");  // threekind
            var p5 = new PlayerHand("straight", "9C,TH,JD,QC,KS");  // straight
            var p6 = new PlayerHand("flush", "2H,4H,6H,8H,TH");  // flush
            var p7 = new PlayerHand("fullhouse", "TD,TC,TH,8H,8D");  // FullHouse
            var p8 = new PlayerHand("fourkind", "TD,TC,TH,TS,8D");  // FourKind
            var p9 = new PlayerHand("straightflush", "9D,TD,JD,QD,KD");  // StraightFlush

            // array of hands mixed
            PlayerHand[] hands = { p3, p2, p1, p4, p5, p7, p9, p8, p6 };

            // assert hand types
            for (int i = 0; i < hands.Length; i++)
            {
                Assert.AreEqual(hands[i].PlayerName,hands[i].HandEvaluation.Rank.ToString().ToLower());
            }


            Array.Sort(hands);
            
            for (int i = 0; i < hands.Length; i++)
            {
                Assert.AreEqual(handOrder[i], hands[i].PlayerName
                                , string.Format("{0} != {1}", hands[i].PlayerName, i)); 
            }
        }

        [TestMethod]
        public void TestPlayerHandKicker()
        {
            var p1 = new PlayerHand("twopair1", "5C,5H,2C,2D,TD");  // two pair
            var p2 = new PlayerHand("twopair2", "5S,5D,2S,2H,JC");  // two pair

            Assert.AreEqual(-1,p1.CompareTo(p2));

            var p3 = new PlayerHand("twopair3", "5C,5H,2C,2D,JD");  // two pair same
            var p4 = new PlayerHand("twopair4", "5S,5D,2S,2H,JC");  // two pair same

            Assert.AreEqual(0, p3.CompareTo(p4));


            var p5 = new PlayerHand("pair1", "5C,5H,2D,3D,TD");  // pair same
            var p6 = new PlayerHand("pair2", "5D,5S,2S,3D,TS");  // pair same

            Assert.AreEqual(0, p5.CompareTo(p6));


            var p7 = new PlayerHand("pair1", "5C,5H,2D,3D,TD");  // pair lower
            var p8 = new PlayerHand("pair2", "5D,5S,2S,3D,JS");  // pair higher

            Assert.AreEqual(-1, p7.CompareTo(p8));

        }

        
    }
}
