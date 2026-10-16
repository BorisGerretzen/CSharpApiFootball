namespace ApiFootball.Models;

/// <summary>Account and quota data returned by the status endpoint.</summary>
public sealed class ApiStatusResponse
{
    public ApiAccountStatus? Account { get; init; }
    public ApiSubscriptionStatus? Subscription { get; init; }
    public ApiRequestStatus? Requests { get; init; }
}

public sealed class ApiAccountStatus
{
    public string? Firstname { get; init; }
    public string? Lastname { get; init; }
    public string? Email { get; init; }
}

public sealed class ApiSubscriptionStatus
{
    public string? Plan { get; init; }
    public DateTimeOffset? End { get; init; }
    public bool? Active { get; init; }
}

public sealed class ApiRequestStatus
{
    public int? Current { get; init; }
    [JsonPropertyName("limit_day")]
    public int? LimitDay { get; init; }
}

/// <summary>Statistical summary returned by the teams/statistics endpoint.</summary>
public sealed class TeamStatisticsResponse
{
    public TeamStatisticsLeague? League { get; init; }
    public TeamReference? Team { get; init; }
    public string? Form { get; init; }
    public TeamStatisticsFixtures? Fixtures { get; init; }
    public TeamStatisticsGoals? Goals { get; init; }
    public TeamStatisticsBiggest? Biggest { get; init; }
    [JsonPropertyName("clean_sheet")]
    public TeamStatisticsCount? CleanSheet { get; init; }
    [JsonPropertyName("failed_to_score")]
    public TeamStatisticsCount? FailedToScore { get; init; }
    public TeamStatisticsPenalty? Penalty { get; init; }
    public List<TeamStatisticsLineup>? Lineups { get; init; }
    public Dictionary<string, Dictionary<string, TeamStatisticsCardCount>>? Cards { get; init; }
}

public class TeamStatisticsLeague
{
    public int? Id { get; init; }
    public string? Name { get; init; }
    public string? Country { get; init; }
    public string? Logo { get; init; }
    public string? Flag { get; init; }
    public int Season { get; init; }
}

public sealed class TeamStatisticsFixtures
{
    public TeamStatisticsCount? Played { get; init; }
    public TeamStatisticsCount? Wins { get; init; }
    public TeamStatisticsCount? Draws { get; init; }
    public TeamStatisticsCount? Loses { get; init; }
}

public sealed class TeamStatisticsCount
{
    public int? Home { get; init; }
    public int? Away { get; init; }
    public int? Total { get; init; }
}

public sealed class TeamStatisticsGoals
{
    public TeamStatisticsGoalSplit? For { get; init; }
    public TeamStatisticsGoalSplit? Against { get; init; }
}

public sealed class TeamStatisticsGoalSplit
{
    public TeamStatisticsCount? Total { get; init; }
    public TeamStatisticsAverage? Average { get; init; }
    public Dictionary<string, TeamStatisticsCardCount>? Minute { get; init; }
    [JsonPropertyName("under_over")]
    public Dictionary<string, JsonElement>? UnderOver { get; init; }
}

public sealed class TeamStatisticsAverage
{
    public string? Home { get; init; }
    public string? Away { get; init; }
    public string? Total { get; init; }
}

public sealed class TeamStatisticsCardCount
{
    public int? Total { get; init; }
    public string? Percentage { get; init; }
}

public sealed class TeamStatisticsBiggest
{
    public TeamStatisticsScore? Streak { get; init; }
    public TeamStatisticsScore? Wins { get; init; }
    public TeamStatisticsScore? Loses { get; init; }
    public TeamStatisticsBiggestGoals? Goals { get; init; }
}

public sealed class TeamStatisticsScore
{
    public string? Home { get; init; }
    public string? Away { get; init; }
    public int? Wins { get; init; }
    public int? Draws { get; init; }
    public int? Loses { get; init; }
    public string? For { get; init; }
    public string? Against { get; init; }
}

public sealed class TeamStatisticsBiggestGoals
{
    public TeamStatisticsBiggestGoalSide? For { get; init; }
    public TeamStatisticsBiggestGoalSide? Against { get; init; }
}

public sealed class TeamStatisticsBiggestGoalSide
{
    public int? Home { get; init; }
    public int? Away { get; init; }
}

public sealed class TeamStatisticsPenalty
{
    public TeamStatisticsPenaltyCount? Scored { get; init; }
    public TeamStatisticsPenaltyCount? Missed { get; init; }
}

public sealed class TeamStatisticsPenaltyCount
{
    public int? Total { get; init; }
    public string? Percentage { get; init; }
}

public sealed class TeamStatisticsLineup
{
    public string? Formation { get; init; }
    public int? Played { get; init; }
}

public sealed class FixtureRoundDetails
{
    public string? Round { get; init; }
    public List<string>? Dates { get; init; }
}

public class PlayerProfile
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Firstname { get; init; }
    public string? Lastname { get; init; }
    public int? Age { get; init; }
    public PlayerBirth? Birth { get; init; }
    public string? Nationality { get; init; }
    public string? Height { get; init; }
    public string? Weight { get; init; }
    public bool? Injured { get; init; }
    public int? Number { get; init; }
    public string? Position { get; init; }
    public string? Photo { get; init; }
}

public sealed class PlayerBirth
{
    public DateOnly? Date { get; init; }
    public string? Place { get; init; }
    public string? Country { get; init; }
}

public sealed class PlayerProfileResponse
{
    public PlayerProfile? Player { get; init; }
}

public sealed class PlayerStatisticsResponse
{
    public PlayerProfile? Player { get; init; }
    public List<PlayerSeasonStatistics>? Statistics { get; init; }
}

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

public sealed class PlayerGamesStatistics
{
    [JsonPropertyName("appearences")]
    public int? Appearances { get; init; }
    public int? Lineups { get; init; }
    public int? Minutes { get; init; }
    public int? Number { get; init; }
    public string? Position { get; init; }
    public string? Rating { get; init; }
    public bool? Captain { get; init; }
}

public sealed class PlayerSubstitutesStatistics
{
    public int? In { get; init; }
    public int? Out { get; init; }
    public int? Bench { get; init; }
}

public sealed class PlayerShotsStatistics
{
    public int? Total { get; init; }
    public int? On { get; init; }
}

public sealed class PlayerGoalsStatistics
{
    public int? Total { get; init; }
    public int? Conceded { get; init; }
    public int? Assists { get; init; }
    public int? Saves { get; init; }
}

public sealed class PlayerPassesStatistics
{
    public int? Total { get; init; }
    public int? Key { get; init; }
    public int? Accuracy { get; init; }
}

public sealed class PlayerTacklesStatistics
{
    public int? Total { get; init; }
    public int? Blocks { get; init; }
    public int? Interceptions { get; init; }
}

public sealed class PlayerDuelsStatistics
{
    public int? Total { get; init; }
    public int? Won { get; init; }
}

public sealed class PlayerDribblesStatistics
{
    public int? Attempts { get; init; }
    public int? Success { get; init; }
    public int? Past { get; init; }
}

public sealed class PlayerFoulsStatistics
{
    public int? Drawn { get; init; }
    public int? Committed { get; init; }
}

public sealed class PlayerCardsStatistics
{
    public int? Yellow { get; init; }
    public int? Yellowred { get; init; }
    public int? Red { get; init; }
}

public sealed class PlayerPenaltyStatistics
{
    public int? Won { get; init; }

    [JsonPropertyName("commited")]
    public int? Committed { get; init; }

    public int? Scored { get; init; }
    public int? Missed { get; init; }
    public int? Saved { get; init; }
}

public sealed class PlayerSquadResponse
{
    public TeamReference? Team { get; init; }
    public List<PlayerSquadMember>? Players { get; init; }
}

public sealed class PlayerSquadMember : PlayerProfile
{
}

public sealed class PlayerTeamHistory
{
    public TeamReference? Team { get; init; }
    public List<int>? Seasons { get; init; }
}

public sealed class CoachProfile
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Firstname { get; init; }
    public string? Lastname { get; init; }
    public int? Age { get; init; }
    public PlayerBirth? Birth { get; init; }
    public string? Nationality { get; init; }
    public string? Height { get; init; }
    public string? Weight { get; init; }
    public string? Photo { get; init; }
    public TeamReference? Team { get; init; }
    public List<CoachCareerEntry>? Career { get; init; }
}

public sealed class CoachCareerEntry
{
    public CoachCareerTeam? Team { get; init; }
    public DateOnly? Start { get; init; }
    public DateOnly? End { get; init; }
}

public sealed class CoachCareerTeam
{
    public int? Id { get; init; }
    public string? Name { get; init; }
    public string? Logo { get; init; }
}

public sealed class InjuryResponse
{
    public InjuryPlayer? Player { get; init; }
    public TeamReference? Team { get; init; }
    public FixtureReference? Fixture { get; init; }
    public TeamStatisticsLeague? League { get; init; }
}

public sealed class InjuryPlayer
{
    public int? Id { get; init; }
    public string? Name { get; init; }
    public string? Photo { get; init; }
    public string? Type { get; init; }
    public string? Reason { get; init; }
}

public sealed class FixtureReference
{
    public int? Id { get; init; }
    public string? Timezone { get; init; }
    public DateTimeOffset? Date { get; init; }
    public long? Timestamp { get; init; }
}

public sealed class PredictionResponse
{
    public PredictionDetails? Predictions { get; init; }
    public TeamStatisticsLeague? League { get; init; }
    public PredictionTeams? Teams { get; init; }
    public PredictionComparison? Comparison { get; init; }
    public List<FixturesResponse>? H2h { get; init; }
}

public sealed class PredictionDetails
{
    public PredictionWinner? Winner { get; init; }
    [JsonPropertyName("win_or_draw")]
    public bool? WinOrDraw { get; init; }
    [JsonPropertyName("under_over")]
    public string? UnderOver { get; init; }
    public PredictionGoals? Goals { get; init; }
    public string? Advice { get; init; }
    public PredictionPercent? Percent { get; init; }
}

public sealed class PredictionWinner
{
    public int? Id { get; init; }
    public string? Name { get; init; }
    public string? Comment { get; init; }
}

public sealed class PredictionGoals
{
    public string? Home { get; init; }
    public string? Away { get; init; }
}

public sealed class PredictionPercent
{
    public string? Home { get; init; }
    public string? Draw { get; init; }
    public string? Away { get; init; }
}

public sealed class PredictionTeams
{
    public PredictionTeam? Home { get; init; }
    public PredictionTeam? Away { get; init; }
}

public sealed class PredictionTeam : TeamReference
{
    [JsonPropertyName("last_5")]
    public PredictionLastFive? Last5 { get; init; }
}

public sealed class PredictionLastFive
{
    public string? Form { get; init; }
    public string? Att { get; init; }
    public string? Def { get; init; }
    [JsonPropertyName("goals")]
    public PredictionGoalSummary? Goals { get; init; }
}

public sealed class PredictionGoalSummary
{
    public PredictionGoalSplit? For { get; init; }
    public PredictionGoalSplit? Against { get; init; }
}

public sealed class PredictionGoalSplit
{
    public int? Total { get; init; }
    public string? Average { get; init; }
}

public sealed class PredictionComparison
{
    public PredictionComparisonSides? Form { get; init; }
    public PredictionComparisonSides? Att { get; init; }
    public PredictionComparisonSides? Def { get; init; }
    [JsonPropertyName("poisson_distribution")]
    public PredictionComparisonSides? PoissonDistribution { get; init; }
    public PredictionComparisonSides? H2h { get; init; }
    public PredictionComparisonSides? Goals { get; init; }
    public PredictionComparisonSides? Total { get; init; }
}

public sealed class PredictionComparisonSides
{
    public string? Home { get; init; }
    public string? Away { get; init; }
}

public sealed class TransferHistory
{
    public PlayerProfile? Player { get; init; }
    public DateTimeOffset? Update { get; init; }
    public List<TransferEntry>? Transfers { get; init; }
}

public sealed class TransferEntry
{
    public DateOnly? Date { get; init; }
    public string? Type { get; init; }
    public TransferTeams? Teams { get; init; }
}

public sealed class TransferTeams
{
    [JsonPropertyName("in")]
    public TeamReference? In { get; init; }
    public TeamReference? Out { get; init; }
}

public sealed class Trophy
{
    public string? League { get; init; }
    public string? Country { get; init; }
    public string? Season { get; init; }
    public string? Place { get; init; }
}

public sealed class SidelinedRecord
{
    public string? Type { get; init; }
    public DateOnly? Start { get; init; }
    public DateOnly? End { get; init; }
}

public sealed class OddsLookup
{
    public int Id { get; init; }
    public string? Name { get; init; }
}

public sealed class OddsResponse
{
    public OddsLeague? League { get; init; }
    public FixtureReference? Fixture { get; init; }
    public DateTimeOffset? Update { get; init; }
    public List<OddsBookmaker>? Bookmakers { get; init; }
}

public sealed class OddsLeague : TeamStatisticsLeague
{
}

public sealed class OddsBookmaker
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public List<OddsBet>? Bets { get; init; }
}

public sealed class OddsBet
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public List<OddsValue>? Values { get; init; }
}

public sealed class OddsValue
{
    public JsonElement? Value { get; init; }
    public string? Odd { get; init; }
    public string? Handicap { get; init; }
    public bool? Main { get; init; }
    public bool? Suspended { get; init; }
}

public sealed class LiveOddsResponse
{
    public LiveOddsFixture? Fixture { get; init; }
    public LiveOddsLeague? League { get; init; }
    public LiveOddsTeams? Teams { get; init; }
    public LiveOddsStatus? Status { get; init; }
    public DateTimeOffset? Update { get; init; }
    public List<OddsBet>? Odds { get; init; }
}

public sealed class LiveOddsFixture
{
    public int? Id { get; init; }
    public LiveOddsFixtureStatus? Status { get; init; }
}

public sealed class LiveOddsFixtureStatus
{
    public string? Long { get; init; }
    public int? Elapsed { get; init; }
    public string? Seconds { get; init; }
}

public sealed class LiveOddsLeague
{
    public int? Id { get; init; }
    public int? Season { get; init; }
}

public sealed class LiveOddsTeams
{
    public LiveOddsTeam? Home { get; init; }
    public LiveOddsTeam? Away { get; init; }
}

public sealed class LiveOddsTeam
{
    public int? Id { get; init; }
    public int? Goals { get; init; }
}

public sealed class LiveOddsStatus
{
    public bool? Stopped { get; init; }
    public bool? Blocked { get; init; }
    public bool? Finished { get; init; }
}

public sealed class OddsMapping
{
    public OddsMappingLeague? League { get; init; }
    public OddsMappingFixture? Fixture { get; init; }
    public DateTimeOffset? Update { get; init; }
}

public sealed class OddsMappingLeague
{
    public int? Id { get; init; }
    public int? Season { get; init; }
}

public sealed class OddsMappingFixture
{
    public int? Id { get; init; }
    public DateTimeOffset? Date { get; init; }
    public long? Timestamp { get; init; }
}
