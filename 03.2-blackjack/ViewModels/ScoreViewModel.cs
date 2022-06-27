using System.Collections.Generic;
using Blackjack.Model;
namespace Blackjack.ViewModels;

public class ScoreViewModel : ViewModelBase {
    public int PlayerScore { get; set; } = 0;
    public int DealerScore { get; set; } = 0;
    int Calc(int score, List<Card> cards)
    {
        int sum = 0;
        foreach (var card in cards)
            sum += Calc(score, card);
        return sum;
    }
    int Calc(int score, Card card)
    {
        if (card.Name == CardName.Ace)
        {
            if (score + 11 > 21)
                return 1;
            else
                return 11;
        }
        else
            return card.Score() ?? 0;
    }
    public void CalcDealer(Dealer dealer)
    {
        DealerScore = Calc(DealerScore, dealer.Cards);
    }
    public void AddToDealer(Card card)
    {
        DealerScore += Calc(DealerScore, card);
    }
    public void CalcPlayer(Meat player)
    {
        PlayerScore = Calc(PlayerScore, player.Cards);
    }
    public void AddToPlayer(Card card)
    {
        PlayerScore += Calc(PlayerScore, card);
    }
}