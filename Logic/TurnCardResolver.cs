namespace Logic;

public class TurnCardResolver
{
    private Card _muestra;
    private Card[] _hand1;
    private Card[] _hand2;
    
    public TurnCardResolver(Gamestate gamestate)
    {
        _muestra = gamestate.Muestra;
        _hand1 = gamestate.Players[0].Hand;
        _hand2 = gamestate.Players[1].Hand;
    }
    
    public int CompareCards(int player1CardIndex, int player2CardIndex)
    {
        var card1 = _hand1[player1CardIndex];
        var card2 = _hand2[player2CardIndex];
        var card1Power = CardPower(card1);
        var card2Power = CardPower(card2);
        if (card1Power > card2Power) return 1;
        return card1Power < card2Power ? -1 : 0;
    }

    public int EnvidoPlayer1()
    {
        if(HasFlor(_hand1)) throw new InvalidOperationException("No se puede cantar envido con flor");
        if(HasPieza(_hand1)) return EnvidoPieza(_hand1);
        if(DoubleSuit(_hand1)) return EnvidoDoubles(_hand1);
        return EnvidoSingles(_hand1);
    }
    
    public int EnvidoPlayer2()
    {
        if(HasFlor(_hand2)) throw new InvalidOperationException("No se puede cantar envido con flor");
        if(HasPieza(_hand2)) return EnvidoPieza(_hand2);
        if(DoubleSuit(_hand2)) return EnvidoDoubles(_hand2);
        return EnvidoSingles(_hand2);
    }
    
    private int CardPower(Card card)
    {
        if (card.Suit == _muestra.Suit)
        {
            switch (card.Value)
            {
                case 2: return 20;
                case 4: return 19;
                case 5: return 18;
                case 11: return 17;
                case 10: return 16;
                case 12 when IsPieza(_muestra) : return CardPower(_muestra);
            }
        }

        if (card.Value == 1 && card.Suit == "Espada") return 15;
        if (card.Value == 1 && card.Suit == "Basto") return 14;
        if (card.Value == 7 && card.Suit == "Espada") return 13;
        if (card.Value == 7 && card.Suit == "Oro") return 12;
        if (card.Value == 3) return 11;
        if (card.Value == 2) return 10;
        if (card.Value == 1) return 9;
        return card.Value - 4;
    }
    
    private int EnvidoPower(Card card)
    {
        if (card.Suit == _muestra.Suit)
        {
            switch (card.Value)
            {
                case 2: return 30;
                case 4: return 29;
                case 5: return 28;
                case 11: return 27;
                case 10: return 26;
                case 12 when IsPieza(_muestra) : return EnvidoPower(_muestra);
            }
        }

        return IsNegra(card) ? 0 : card.Value;
    }
    
    private bool IsPieza(Card card)
    {
        return CardPower(card) > 15;
    }
    
    private bool IsNegra(Card card)
    {
        return card.Value == 12 || card.Value == 11 || card.Value == 10;
    }

    private bool HasFlor(Card[] hand)
    {
        return TripleSuit(hand) || PiezaAndDoubleSuit(hand) || DoubleSuit(hand);
    }
    
    private bool HasPieza(Card[] hand)
    {
        return IsPieza(hand[0]) || IsPieza(hand[1]) || IsPieza(hand[2]);
    }
    
    private bool TripleSuit(Card[] hand)
    {
        return hand[0].Suit == hand[1].Suit && hand[1].Suit == hand[2].Suit;
    }
    
    private bool PiezaAndDoubleSuit(Card[] hand)
    {
        return IsPieza(hand[0]) && hand[1].Suit == hand[2].Suit ||
               IsPieza(hand[1]) && hand[0].Suit == hand[2].Suit ||
               IsPieza(hand[2]) && hand[0].Suit == hand[1].Suit;
    }
    
    private bool DoubleSuit(Card[] hand)
    {
        return hand[0].Suit == hand[1].Suit ||
               hand[1].Suit == hand[2].Suit ||
               hand[0].Suit == hand[2].Suit;
    }
    
    private int EnvidoPieza(Card[] hand)
    {
        var powers = hand.Select(EnvidoPower).ToList();
        powers.Sort();
        return powers[1] + powers[2];
    }
    
    private int EnvidoDoubles(Card[] hand)
    {
        if (hand[0].Suit == hand[1].Suit) return EnvidoPower(hand[0]) + EnvidoPower(hand[1]) + 20;
        if (hand[1].Suit == hand[2].Suit) return EnvidoPower(hand[1]) + EnvidoPower(hand[2]) + 20;
        return EnvidoPower(hand[0]) + EnvidoPower(hand[2]) + 20;
    }
    
    private int EnvidoSingles(Card[] hand)
    {
        var powers = hand.Select(EnvidoPower).ToList();
        powers.Sort();
        return powers[2];
    }
}