namespace Logic;

public class Gamestate
{
    public int Goal { get; set; }
    public int CurrentPlayerTurn { get; set; }
    public int Team1Score { get; set; } = 0;
    public bool Team1Won => Team1Score >= Goal;
    public int Team2Score { get; set; } = 0;
    public bool Team2Won => Team2Score >= Goal;
    public int Turn { get; set; } = 0;
    public int Bet { get; set; } = 1;
    public Deck Deck { get; set; } = new();
    public Card Muestra { get; set; }
    public List<Player> Players { get; set; }
    
    
    public Gamestate(int goal, int players)
    {
        Goal = goal;
        for (var i = 0; i < players; i++)
        {
            Players.Add(new Player());
        }
        CurrentPlayerTurn = players;
    }

    public void StartTurn()
    {
        CurrentPlayerTurn = + 1 % Players.Count;
        Turn++;
        Bet = 1;
        Deck.Shuffle();
        for(var i = 0; i < Players.Count; i++)
        {
            Players[i].Hand = Deck.Hand(i);
        }
        Muestra = Deck.Muestra(Players.Count);
    }
}