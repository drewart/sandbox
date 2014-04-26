package com.drewart.Poker;


import java.util.regex.*;

/*
* Card class 
*/
public class Card implements Comparable
{
    // pattern for card input
    static Pattern cardPattern  = Pattern.compile("([2-9]|T|J|Q|K|A)(C|D|H|S)");

    public Card(String cardString) throws IllegalArgumentException
    {
    	  // using regex to validate the card
        Matcher match = cardPattern.matcher(cardString);
        if (!match.matches()) {
            throw new IllegalArgumentException("Not a valid Card String: " + cardString);
        }

        value = match.group(1).charAt(0);
        suit =  match.group(2).charAt(0);
    }

    //clubs C, diamonds D, hearts H, or spades S
    private char suit;
    private char value;

    public char getSuit()
    {
        return suit;
    }

    public char getValue()
    {
        return value;
    }

    public int getRank()
    {
    	return getValueRank();
    }
    
    /*
     *
     */
    public int getValueRank()
    {
        int rank = -1;
					
				char val = value;

				switch (val) {
						case 'T':
								rank = 10;
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
    
    @Override
    public String toString()
    {
    	return new StringBuffer().append(value).append(suit).toString();
    }

    @Override
    public int compareTo(Object o) {
        Card c = (Card)o;
        if (getSuit() == c.getSuit() && getValueRank() == c.getValueRank())
            return 0;
        else if (getSuit() > c.getSuit())
            return 1;
        else if (getSuit() < c.getSuit())
            return -1;
        else if (getValueRank() < c.getValueRank())
            return -1;
        else
            return 1;
    }
}
