using System;
using System.Reflection;

namespace Blackjack.Model;

public enum CardSuit
{
    None,
    Heart,
    Diamond,
    Spade,
    Club
}

public enum CardName
{
    None,
    Ace,
    Two,
    Tree,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Joker
}

public class Card
{
    static string? _assemblyName;
    CardSuit _suit;
    CardName _name;
    // bool _visible;

    public bool Visible { get; set; } = true;

    static Card()
    {
        _assemblyName = Assembly.GetEntryAssembly()!.GetName().Name ?? null;
    }

    public Card()
    {
        _suit = CardSuit.None;
        _name = CardName.None;
    }

    public Card(CardSuit suit, CardName name, bool visible = true)
        : this()
    {
        _suit = suit;
        _name = name;
        Visible = visible;
    }

    public Card(string suit, string name, bool visible = true)
        : this()
    {
        _suit = ParseSuit(suit);
        _name = ParseName(name);
        Visible = visible;
    }

    public CardSuit Suit {
        get => _suit;
        set => _suit = value;
    }

    public CardName Name {
        get => _name;
        set => _name = value;
    }

    // fixme: for clarity, rewrite to "(CardName) name" or drop and use explicit cast
    CardName ParseName(string name) => name switch
    {
        "Two"   => CardName.Two,
        "Tree"  => CardName.Tree,
        "Four"  => CardName.Four,
        "Five"  => CardName.Five,
        "Six"   => CardName.Six,
        "Seven" => CardName.Seven,
        "Eight" => CardName.Eight,
        "Nine"  => CardName.Nine,
        "Ten"   => CardName.Ten,
        "Jack"  => CardName.Jack,
        "Queen" => CardName.Queen,
        "King"  => CardName.King,
        "Ace"   => CardName.Ace,
        _ => CardName.None,
    };

    // fixme: for clarity, rewrite to "(CardSuit) suit" or drop and use explicit cast
    CardSuit ParseSuit(string suit) => suit switch
    {
        "Club"     => CardSuit.Club,
        "Diamond"  => CardSuit.Diamond,
        "Heart"    => CardSuit.Heart,
        "Spade"    => CardSuit.Spade,
        _ => CardSuit.None,
    };

    // fixme: for clarity, rewrite to "(string) name" or drop and use implicit cast
    static string? NameToString(CardName name) => name switch
    {
        CardName.Two   => "Two",
        CardName.Tree  => "Tree",
        CardName.Four  => "Four",
        CardName.Five  => "Five",
        CardName.Six   => "Six",
        CardName.Seven => "Seven",
        CardName.Eight => "Eight",
        CardName.Nine  => "Nine",
        CardName.Ten   => "Ten",
        CardName.Jack  => "Jack",
        CardName.Queen => "Queen",
        CardName.King  => "King",
        CardName.Ace   => "Ace",
        CardName.None  => null,
        _ => null,
    };

    // fixme: for clarity, rewrite to "(string) suit" or drop and use implicit cast
    static string? SuitToString(CardSuit? suit) => suit switch
    {
        CardSuit.Club => "Club",
        CardSuit.Diamond => "Diamond",
        CardSuit.Heart => "Heart",
        CardSuit.Spade => "Spade",
        CardSuit.None => null,
        _ => null,
    };

    public override string ToString()
    {
        return SuitToString(_suit) + " " + NameToString(_name);
    }

    public string ToUriString()
    {
        return SuitToString(_suit) + " " + _name switch
        {
            CardName.Ace   => "1",
            CardName.Two   => "2",
            CardName.Tree  => "3",
            CardName.Four  => "4",
            CardName.Five  => "5",
            CardName.Six   => "6",
            CardName.Seven => "7",
            CardName.Eight => "8",
            CardName.Nine  => "9",
            CardName.Ten   => "10",
            _ => NameToString(_name)
        };
    }
    public Uri Uri
    {
        get
        {
            if (_name == CardName.None || _suit == CardSuit.None)
                throw new InvalidOperationException("can't construct resource URI to empty card");

            string cardString = this
                                .ToUriString()
                                .ToLower()
                                .Replace(" ", "_");

            if (Visible)
                return new Uri($"avares://{_assemblyName}/Assets/cards/{cardString}.png");
            else
                return new Uri($"avares://{_assemblyName}/Assets/cards/back.png");
        }
        // set ;
    }
    public int? Score() => _name switch
    {
            CardName.Two   => 2,
            CardName.Tree  => 3,
            CardName.Four  => 4,
            CardName.Five  => 5,
            CardName.Six   => 6,
            CardName.Seven => 7,
            CardName.Eight => 8,
            CardName.Nine  => 9,
            CardName.Ten   => 10,
            CardName.Jack  => 10,
            CardName.Queen => 10,
            CardName.King  => 10,
            CardName.Ace   => null,
            CardName.None  => null,
            _ => null,
    };
}