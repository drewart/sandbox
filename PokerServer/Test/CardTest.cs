using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerHands;

namespace Test
{
    [TestClass]
    public class CardTest
    {


        [TestMethod]
        public void TestCardOneClub()
        {
            try
            {
                var card = new Card("1C");
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
        public void TestCard2Z()
        {
            try
            {
                var card = new Card("2Z");
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
        public void TestAllCards()
        {
            char[] cards = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
            char[] suits = { 'C', 'D', 'H', 'S' };
            foreach (var c in cards)
            {
                foreach (var s in suits)
                {
                    String cardStr = new string(new char[] {c, s});
                    var card = new Card(cardStr);
                    Assert.AreEqual(card.Value, c);
                    Assert.AreEqual(card.Suit, s);                   
                }
            }
        }

         [TestMethod]
        public void TestCardRank()
        {
            var card = new Card("2C");
            
            Assert.AreEqual(card.Rank, 2);

            card = new Card("AC");
            Assert.AreEqual(card.Rank, 14);
        }

        public void TestCardComapre()
        {
            var c1 = new Card("2C");
            var c2 = new Card("3C");
            var c3 = new Card("2C");
            Assert.IsTrue(c1.CompareTo(c2) < 0);

            Assert.IsTrue(c2.CompareTo(c1) > 0);

            Assert.IsTrue(c1.CompareTo(c3) == 0);

        }

        
    }
}
