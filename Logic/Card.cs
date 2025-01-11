namespace Logic;

public class Card
{
    public int Id => GetId();

    private int GetId()
    {
        var suitId = Suit switch
        {
            "Espada" => 0,
            "Basto" => 10,
            "Oro" => 20,
            "Copa" => 30,
            _ => throw new ArgumentException("Invalid suit")
        };

        return suitId + (Value > 7 ? Value - 3 : Value - 1);
    }


    public string Suit { get; }
    public int Value { get; }

    public Card(string suit, int value)
    {
        Suit = suit;
        Value = value;
    }

    public string ToStringFormatted()
    {
        var colorCode = Suit switch
        {
            "Espada" => "\u001b[38;5;33m",
            "Basto" => "\u001b[38;5;2m",
            "Oro" => "\u001b[38;5;11m",
            "Copa" => "\u001b[38;5;1m",
            _ => ""
        };
        return $"{colorCode}{Value} de {Suit}\u001b[0m";
    }
    
    public override string ToString()
    {
        return $"{Value} de {Suit}";
    }
}