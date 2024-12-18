using System.Text;

namespace Logic;

public class Deck
{
    private readonly int _deckSize = 40;
    private Card[] Cards { get; }

    public Deck()
    {
        Cards = new Card[_deckSize];
        var suits = new[] { "Espada", "Basto", "Oro", "Copa" };
        var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 10, 11, 12 };
        int pos = 0;
        foreach (var suit in suits)
        {
            foreach (var number in numbers)
            {
                Cards[pos] = new Card(suit, number);
                pos++;
            }
        }
    }
    
    public void Shuffle()
    {
        var random = new Random();
        for (var i = 0; i < _deckSize; i++)
        {
            var j = random.Next(i, _deckSize);
            (Cards[i], Cards[j]) = (Cards[j], Cards[i]);
        }
    }
    
    public Card[] Hand(int player)
    {
        return [Cards[player-1], Cards[player + 1], Cards[player + 2]];
    }
    
    public Card Muestra(int players)
    {
        return Cards[players * 3];
    }
    
    //For debugging purposes
    public override string ToString()
    {
        var deckString = new StringBuilder();
        foreach (var card in Cards)
        {
            deckString.Append(card + "\n");
        }
        return deckString.ToString();
    }
}