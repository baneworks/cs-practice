using System.Collections.Generic;
using ReactiveUI;

namespace Blackjack.Model;

public enum GamblerType {
    Dealer,
    Meat
}

public abstract class Gambler
{
    public List<Card> Cards { get; set; } = new();
    public virtual GamblerType? Type()
    {
        return null;
    }
    public virtual void AddCard(Card card)
    {
        Cards.Add(card);
    }
    public abstract bool DoMove();
}

public class Dealer : Gambler
{
    public override GamblerType? Type()
    {
        return GamblerType.Dealer;
    }
    public override bool DoMove()
    {
        return true;
    }
}

public class Meat : Gambler
{
    public override GamblerType? Type()
    {
        return GamblerType.Meat;
    }
    public override bool DoMove()
    {
        return true;
    }
}