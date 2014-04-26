package com.drewart.Poker;

import java.text.*;
import java.util.*;
//import com.drewart.Poker.*;



public class TestCard
{
	public TestCard()
	{}


	static void TestCard()
	{
		Card c = new Card("TC");
		
		System.out.println("value" + c.getValueRank());
		if (c.getValueRank() == 10)
			System.out.println("+");
		else
			System.out.println("-");
	}
	
	static void TestPattern()
	{
		StringBuffer sb = new StringBuffer();
		char[] cards = {'2','3','4','5','6','7','8','9','T','J','Q','K','A'};
		char[] suits = {'C','D','H','S' };
		for(char s : suits)
			for(char c : cards)
				sb.append(c).append(s).append(',');
				
		String allCards = sb.toString();
		String[] cardArray = allCards.split(",");
		ArrayList<Card> cardList = new ArrayList<Card>();
		for(String cardStr : cardArray)
			cardList.add(new Card(cardStr));
		
		for(Card card : cardList) {
			System.out.print(card);
			System.out.print(" : ");
			System.out.print(card.getValueRank());
			System.out.println();
		}
		
		Card[] sortCards = cardList.toArray(new Card[0]);
		Arrays.sort(sortCards);
		//List<Card> sortedCards = cardSet.sort();
		System.out.println("sorted");
		for(Card card : sortCards)
		{
			System.out.print(card);
			System.out.print(" : ");
			System.out.print(card.getValueRank());
			System.out.println();
		}
	}


	public static void main(String args[])
	{
			TestCard();
			TestPattern();
	}
}
