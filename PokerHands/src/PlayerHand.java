package com.drewart.Poker;

import java.util.*;

public class PlayerHand  implements Comparable {
	
	public PlayerHand(String playerName,String handStr) throws Exception
	{
		this.playerName = playerName;
		String[] rawCards = handStr.split(",");
		if (rawCards.length < 5) {
			throw new Exception("hand not complete: "+hand);
		}
		for(int i = 0; i < 5; i++)
		{
			this.hand[i] = new Card(rawCards[i]);
		}
		Arrays.sort(hand);		//sort hand to help ranking/score
	}
	
  public enum HandRank { Nothing,Pair,TwoPair,ThreeKind,Straight,Flush,FullHouse,FourKind,StraightFlush };
  
  private String playerName;
  private HandRank rank;
  
  private Card[] hand = new Card[5];
  
  // 
  public String getPlayerName()
  {
  	return playerName;
  }
  
  public Card[] getHand()
  {
  	return hand;
  }
  
  public HandRank getRank()
  {
  	
  }
  
	@Override
	public int compareTo(Object o) {
		PlayerHand hand = (PlayerHand)o;
		
		
	}
	
	
  private void score() {
  	
  }
  
  public bool hasStraightFlush()
  {
  	for(int i = 1; i < 4; i++) {
  		if (hand[0].getSuit() != hand[i].getSuit() && //same suit 
  			hand[0].getValueRank() + i != hand[i].getValueRank())  // straight
  				return false;
  	}
  	return true;
  }

  public bool hasStraight()
  {
  	for(int i = 1; i < 4; i++) {
  		if (hand[0].getValueRank() + i != hand[i].getValueRank())  // straight
  				return false;
  	}
  	return true;
  }
  
  public bool hasFourKind()
  {
  	// loop to see if 4 in a row
  	int count = 0;
  	int max = 0;
  	for(int i = 0; i < 2; i++) {
  		for(int j = 0; j < 5; j++) {
  			
  			if (i == j) continue; //skip self
  			
  		  if (hand[i].getValueRank() == hand[j].getValueRank())
  		  	count++;
  		}
  		if (count > max)
  			count = max;
  		count = 0;  //reset count
  	}
  	if (count == 4)
  		return true;
  	
  	return false;
  }
  
  public bool hasTwoPair()
  {
  	if (hasFourKind())
  		return false;
  	
  	if (hand[0].getRank() == hand[1].getRank() && hand[2].getRank() == hand[3].getRank())
  		return true;
  	else if (hand[0].getRank() == hand[1].getRank() && hand[2].getRank() == hand[4].getRank())
  		return true;
  	else if (hand[0].getRank() == hand[1].getRank() && hand[3].getRank() == hand[4].getRank())
  		return true;
  	else if (hand[1].getRank() == hand[2].getRank() && hand[3].getRank() == hand[4].getRank())
  		return true;	
  		
  }

	public HashMap getMatches()
	{
		HashMap<Integer,HashSet> matchMap = new HashMap<Integer,HashSet>();
		
		// count matches
  	for(int i = 0; i < 5; i++) {
  	
  		int rank = hand[i].getRank();
  		if (matchMap.containsKey(rank)) {
  			matchMap.get(rank).add(hand[i]);
  	  } else {
  			HashSet set = new HashSet()
  			set.put(hand[i]);
  			matchMap.put(rank,set);
  			matchMap.get(rank).add(hand[i])
  	  }
  	
  	return matchMap;
  	}

  
  
	}
}
