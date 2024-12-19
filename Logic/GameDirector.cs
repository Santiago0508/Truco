namespace Logic;

public class GameDirector(int goal, int players)
{
    private Gamestate Gamestate { get; set; } = new(goal, players);
    private bool GameIsOver => Gamestate.Team1Won || Gamestate.Team2Won;

    public void StartGame()
    {
        while (!GameIsOver)
        {
            Gamestate.StartTurn();
            // var action = Play(Gamestate.Players[Gamestate.CurrentPlayerTurn]);
        }

        if (Gamestate.Team1Won)
        {
            Team1Won();
        }
        else
        {
            Team2Won();
        }
    }
    
    private void Team1Won()
    {
        Console.WriteLine("Team 1 won!");
    }
    
    private void Team2Won()
    {
        Console.WriteLine("Team 2 won!");
    }
}