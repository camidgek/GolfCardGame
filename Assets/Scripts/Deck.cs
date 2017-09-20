using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using ExtensionMethods;

public class Deck {

	private List<Card> _drawPile = new List<Card>();
	private List<Card> _discardPile = new List<Card>();

	private static System.Random rnd = new System.Random();

	public Deck()
	{
		for (int suit = 0; suit < 4; suit++) {
			for (int value = 1; value < 14; value++) {
				_drawPile.Add (new Card (suit, value));
			}
		}
	}

	public void shuffle()  
	{
        _drawPile.Shuffle();
	}

    public Card takeCard(string pile)
    {
        Card card;
        switch (pile)
        {
            default:
            case "draw":
                if (_drawPile.Count == 0)
                {
                    return null;
                    // Re-shuffle discard?
                }
                card = _drawPile[0];
                _drawPile.RemoveAt(0);
                break;
            case "discard":
                if (_discardPile.Count == 0)
                {
                    return null;
                }
                card = _discardPile[0];
                _discardPile.RemoveAt(0);
                break;
        }
		return card;
	}

    public void discard(Card card)
    {
        _discardPile.Insert(0, card);
    }

}
