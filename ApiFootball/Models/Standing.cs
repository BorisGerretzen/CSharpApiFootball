namespace ApiFootball.Models;
public class Standing
{
    public int Rank { get; private set; }
    public Team Team { get; private set; }
    public int Points { get; set; }
    public int GoalsDiff { get; private set; }
    public string Group { get; private set; }
    public string Form { get; private set; }
    public string Status { get; private set; }
    public string Description { get; private set; }
    public StandingGames All { get; private set; }
    public StandingGames Home { get; private set; }
    public StandingGames Away { get; private set; }
    public DateTime Update { get; private set; }
}

public class StandingGames
{
    public int Played { get; private set; }
    public int Win { get; private set; }
    public int Draw { get; private set; }
    public int Lose { get; private set; }
    public StandingGameGoal Goals { get; private set; }
}

public class StandingGameGoal
{
    public int For { get; private set; }
    public int Against { get; private set; }
}