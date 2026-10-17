namespace ApiFootball.Models;

public sealed class PlayerSeasonStatistics
{
    public TeamReference? Team { get; init; }
    public TeamStatisticsLeague? League { get; init; }
    public PlayerGamesStatistics? Games { get; init; }
    public PlayerSubstitutesStatistics? Substitutes { get; init; }
    public PlayerShotsStatistics? Shots { get; init; }
    public PlayerGoalsStatistics? Goals { get; init; }
    public PlayerPassesStatistics? Passes { get; init; }
    public PlayerTacklesStatistics? Tackles { get; init; }
    public PlayerDuelsStatistics? Duels { get; init; }
    public PlayerDribblesStatistics? Dribbles { get; init; }
    public PlayerFoulsStatistics? Fouls { get; init; }
    public PlayerCardsStatistics? Cards { get; init; }
    public PlayerPenaltyStatistics? Penalty { get; init; }
}