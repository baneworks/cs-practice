using System;
using Blackjack.Model;

namespace Blackjack.ViewModels;

public class DeckViewModel : ViewModelBase
{
    Deck _deck;
    public DeckViewModel(Deck deck, Action? callbackLeft = null, Action? callbackCurrent = null)
    {
        _deck = deck;

        CardLeft = new CardViewModel(_deck.Card);
        if (callbackLeft != null)
            CardLeft.Callback = callbackLeft;

        CardCurrent = new CardViewModel(_deck.Left);
        if (callbackCurrent != null)
            CardCurrent.Callback = callbackCurrent;
    }
    public CardViewModel CardLeft { get; }
    public CardViewModel CardCurrent { get; }
}