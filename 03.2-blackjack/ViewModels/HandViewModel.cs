using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Layout;
using Avalonia.Media;
using ReactiveUI;
using Blackjack.Model;

namespace Blackjack.ViewModels;

public class HandViewModel : ViewModelBase
{
    public List<CardViewModel> CardViews { get; } = new();
    public string PlayerName { get; }
    public IBrush ForeColor { get; }
    public int? Width { get; set; } = null;
    public int? Height { get; set; } = 244;
    public int? CardWidth { get; } = null;
    public int? CardHeight { get; } = null;
    public StackLayout Layout { get; } = new StackLayout{Orientation = Orientation.Horizontal, Spacing = 4};
    public HandViewModel(GamblerType type)
    {

        if (type == GamblerType.Meat) {
            PlayerName = "YOU";
            ForeColor = Brushes.DarkSeaGreen;
        }
        else
        {
            PlayerName = "DEALER";
            ForeColor = Brushes.DarkOrange;
        }
    }
    public void CreateViews(List<Card> cards)
    {
        CardViews.Clear();
        foreach (var card in cards)
            AddCardView(card);
    }
    void AddCardView(Card card) {
        CardViews.Add(new CardViewModel
        {
            Card = card,
            Width = CardWidth,
            Height = CardHeight,
        });
    }
}