namespace ApiFootball.Models;

public class FixturePlayers
{
    public required FixturePlayerTeam Team { get; init; }
    public required List<FixturePlayerEntry> Players { get; init; }
}

public class FixturePlayerTeam
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Logo { get; init; }
    public required DateTimeOffset Update { get; init; }
}

public class FixturePlayerEntry
{
    public required FixturePlayerProfile Player { get; init; }
    public required List<FixturePlayerStatistics> Statistics { get; init; }
}

public class FixturePlayerProfile
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Photo { get; init; }
}

public class FixturePlayerStatistics
{
    public required FixturePlayerGames Games { get; init; }
    public int? Offsides { get; init; }
    public FixturePlayerShots? Shots { get; init; }
    public FixturePlayerGoals? Goals { get; init; }
    public FixturePlayerPasses? Passes { get; init; }
    public FixturePlayerTackles? Tackles { get; init; }
    public FixturePlayerDuels? Duels { get; init; }
    public FixturePlayerDribbles? Dribbles { get; init; }
    public FixturePlayerFouls? Fouls { get; init; }
    public FixturePlayerCards? Cards { get; init; }
    public FixturePlayerPenalty? Penalty { get; init; }
}

public class FixturePlayerGames
{
    public int? Minutes { get; init; }
    public int? Number { get; init; }
    public string? Position { get; init; }
    public string? Rating { get; init; }
    public bool? Captain { get; init; }
    public bool? Substitute { get; init; }
}

public class FixturePlayerShots
{
    public int? Total { get; init; }
    public int? On { get; init; }
}

public class FixturePlayerGoals
{
    public int? Total { get; init; }
    public int? Conceded { get; init; }
    public int? Assists { get; init; }
    public int? Saves { get; init; }
}

public class FixturePlayerPasses
{
    public int? Total { get; init; }
    public int? Key { get; init; }
    public string? Accuracy { get; init; }
}

public class FixturePlayerTackles
{
    public int? Total { get; init; }
    public int? Blocks { get; init; }
    public int? Interceptions { get; init; }
}

public class FixturePlayerDuels
{
    public int? Total { get; init; }
    public int? Won { get; init; }
}

public class FixturePlayerDribbles
{
    public int? Attempts { get; init; }
    public int? Success { get; init; }
    public int? Past { get; init; }
}

public class FixturePlayerFouls
{
    public int? Drawn { get; init; }
    public int? Committed { get; init; }
}

public class FixturePlayerCards
{
    public int? Yellow { get; init; }
    public int? Red { get; init; }
}

public class FixturePlayerPenalty
{
    public int? Won { get; init; }

    [JsonPropertyName("commited")]
    public int? Committed { get; init; }

    public int? Scored { get; init; }
    public int? Missed { get; init; }
    public int? Saved { get; init; }
}
