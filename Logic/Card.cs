namespace Logic;

public class Card
{
    public string Id => $"{Suit[0]}{Value}";
    public string Suit { get; }
    public int Value { get; }
    
    public Card(string suit, int value)
    {
        Suit = suit;
        Value = value;
    }

    public override string ToString()
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
}