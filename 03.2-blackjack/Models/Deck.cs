using System;
using System.Linq;
using System.Collections.Generic;

namespace Blackjack.Model;

public class Deck
{
    List<Card> _left = new();
    List<Card> _out = new();
    Card _current;
    public Deck()
    {
        // fixme: can i initialize with iterators?
        List<Card> cards = new();
        foreach (CardName name in Enum.GetValues(typeof(CardName)))
        {
            if (name == CardName.None) continue;
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                if (suit == CardSuit.None) continue;
                cards.Add(new Card(suit, name));
            }
        }

        // random indexes
        Random rnd = new Random();
        List<int> rndIndex = Enumerable.Range(0, 54).ToList<int>();
        int i = 0;
        while (rndIndex.Count > 0)
        {
            int rnd_i = rnd.Next(rndIndex.Count - 1);
            _left.Add(cards[rndIndex[rnd_i]]);
            rndIndex.RemoveAt(rnd_i);
            i++;
        }

        _current = _left[1];
        _current.Visible = false;
        _left.RemoveAt(1);
    }
    public void Draw()
    {

    }
    public Card Card => _current;
    public Card Left => _left[0];
}