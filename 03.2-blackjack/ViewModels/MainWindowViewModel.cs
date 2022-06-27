using ReactiveUI;
using Blackjack.Model;

namespace Blackjack.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public Game Game = new Game();
    public DeckViewModel DeckView { get; }
    public HandViewModel DealerView { get; set; }
    public HandViewModel PlayerView { get; set; }
    public ScoreViewModel ScoreView { get; set; }
    public MainWindowViewModel()
    {
        DeckView = Game.DeckView;
        DealerView = Game.DealerView;
        PlayerView = Game.PlayerView;
        ScoreView = Game.ScoreView;

        //? this works as expected
        Game.AddCardToDealer(new Card(CardSuit.Club, CardName.Ace));
        Game.AddCardToDealer(new Card(CardSuit.Club, CardName.Ten));

        //? this also works fine
        Game.AddCardToPlayer(new Card(CardSuit.Club, CardName.Ace));
        Game.AddCardToPlayer(new Card(CardSuit.Club, CardName.Ten));
    }
}

