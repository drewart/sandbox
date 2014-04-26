package com.drewart.Poker;

import static org.junit.Assert.assertEquals;

import org.junit.Test;
import org.junit.Ignore;
import org.junit.runner.RunWith;
import org.junit.runners.JUnit4;

/**
 * Tests for {@link Foo}.
 *
 * @Drew Pierce drew@drewart.com ()
 */
@RunWith(JUnit4.class)
public class PlayerHandTest {

    @Test
    public void thisAlwaysPasses() {
    	
    }
    
    @Test
    public void testPlayerHandParse() 
    {
    	try {
				PlayerHand pHand = new PlayerHand("Drew","5C,3C,2C,6C,4C");
				assertEquals(pHand.getHand()[0].toString(),"2C");
				assertEquals(pHand.getHand()[1].toString(),"3C");
				assertEquals(pHand.getHand()[2].toString(),"4C");
				assertEquals(pHand.getHand()[3].toString(),"5C");
				assertEquals(pHand.getHand()[4].toString(),"5C");
    	} 
    	catch (Exception e)
    	{
    		
    	}
    }

    @Test
    @Ignore
    public void thisIsIgnored() {
    		
    }
}
