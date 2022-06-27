using Avalonia.Controls;

using Blackjack.ViewModels;

namespace Blackjack.Model;

public class Game
{
    bool PlayerTurn = true;
    public Deck Deck { get; } = new Deck();
    public Dealer Dealer { get; } = new Dealer();
    public Meat Player { get; } = new Meat();
    public DeckViewModel DeckView { get; set; }
    public HandViewModel PlayerView { get; set; }
    public HandViewModel DealerView { get; set; }
    public ScoreViewModel ScoreView { get; set; }
    public Game()
    {
        DeckView = new DeckViewModel(Deck, OnDeckLeft, OnDeckCurrent);
        DealerView = new HandViewModel(GamblerType.Dealer);
        PlayerView = new HandViewModel(GamblerType.Meat);
        ScoreView = new ScoreViewModel();
    }
    public void AddCardToDealer(Card card)
    {
        Dealer.AddCard(card);
        ScoreView.AddToDealer(card);
        DealerView.CreateViews(Dealer.Cards);
    }
    public void AddCardToPlayer(Card card)
    {
        Player.AddCard(card);
        ScoreView.AddToPlayer(card);
        PlayerView.CreateViews(Player.Cards);
    }
    public void OnDeckLeft()
    {
        // (new Window()).Show();
    }
    public void OnDeckCurrent()
    {
        // fixme: this not cause view update
        if (PlayerTurn)
        {
            this.AddCardToPlayer(new Card(Deck.Card.Suit, Deck.Card.Name));
        }
        // (new Window()).Show();
    }
}

