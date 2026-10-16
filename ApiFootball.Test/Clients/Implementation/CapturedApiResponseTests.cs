using ApiFootball.Clients.Implementation;
using ApiFootball.Models.Responses;

namespace ApiFootball.Test.Clients.Implementation;

/// <summary>
/// Exercises client routes and response models with JSON captured once from API-Football.
/// These tests stay offline; only the LiveTests project makes network requests.
/// </summary>
[TestFixture]
public sealed class CapturedApiResponseTests : BaseEndpointTest
{
    private static void AssertPage<T>(PagedResponse<T> response, string route)
    {
        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo(route));
            Assert.That(response.Errors, Is.Empty);
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Is.Not.Empty);
            Assert.That(response.Paging.Current, Is.EqualTo(1));
        });
    }

    private static void AssertParameter(BaseResponse response, string key, string value)
    {
        Assert.That(response.Parameters, Contains.Key(key).WithValue(value));
    }

    [Test]
    public async Task GetCoachs_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("coachs?team=40", GetExpected("Test_Coachs_Team40_Real_Response"));
        var response = await new CoachesClient(factory, MockOptions()).GetCoachs(team: 40);

        AssertPage(response, "coachs");
        AssertParameter(response, "team", "40");
        Assert.That(response.Response.Any(coach => coach.Id > 0 && coach.Team?.Id == 40), Is.True);
        Assert.That(response.Response.Any(coach => coach.Career is { Count: > 0 }), Is.True);
    }

    [Test]
    public async Task GetCountries_WithName_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("countries?name=Netherlands", GetExpected("Test_Countries_Netherlands_Real_Response"));
        var response = await new CountriesClient(factory, MockOptions()).GetCountries(name: "Netherlands");

        AssertPage(response, "countries");
        AssertParameter(response, "name", "Netherlands");
        Assert.That(response.Response[0].Code, Is.EqualTo("NL"));
    }

    [Test]
    public async Task GetLeagues_WithCountry_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("leagues?country=Netherlands", GetExpected("Test_Leagues_Netherlands_Real_Response"));
        var response = await new LeaguesClient(factory, MockOptions()).GetLeagues(country: "Netherlands");

        AssertPage(response, "leagues");
        AssertParameter(response, "country", "Netherlands");
        Assert.That(response.Response.Any(item => item.Country.Name == "Netherlands"), Is.True);
    }

    [Test]
    public async Task GetVenues_WithSearch_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("venues?search=Twente", GetExpected("Test_Venues_Twente_Real_Response"));
        var response = await new VenuesClient(factory, MockOptions()).GetVenues(search: "Twente");

        AssertPage(response, "venues");
        AssertParameter(response, "search", "Twente");
        Assert.That(response.Response.Any(venue => venue.Name?.Contains("Twente", StringComparison.OrdinalIgnoreCase) == true), Is.True);
    }

    [Test]
    public async Task GetRoundsWithDates_DeserializesCapturedApiResponse()
    {
        var route = "fixtures/rounds?league=88&season=2022&dates=true&timezone=UTC";
        var factory = MockFactory(route, GetExpected("Test_FixtureRounds_Dates_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetRoundsWithDates(88, 2022, timezone: "UTC");

        AssertPage(response, "fixtures/rounds");
        AssertParameter(response, "dates", "true");
        Assert.That(response.Response.Any(round => !string.IsNullOrWhiteSpace(round.Round) && round.Dates is { Count: > 0 }), Is.True);
    }

    [Test]
    public async Task GetRoundsInTimezone_DeserializesCapturedApiResponse()
    {
        var route = "fixtures/rounds?league=88&season=2022&timezone=Europe%2fAmsterdam";
        var factory = MockFactory(route, GetExpected("Test_FixtureRounds_Timezone_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetRoundsInTimezone(88, 2022, "Europe/Amsterdam");

        AssertPage(response, "fixtures/rounds");
        AssertParameter(response, "timezone", "Europe/Amsterdam");
        Assert.That(response.Response, Does.Contain("Regular Season - 1"));
    }

    [Test]
    public async Task GetFixtures_ById_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("fixtures?id=1213546", GetExpected("Test_FixtureById_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetFixtures(id: 1213546);

        AssertPage(response, "fixtures");
        AssertParameter(response, "id", "1213546");
        Assert.That(response.Response[0].Fixture.Id, Is.EqualTo(1213546));
        Assert.That(response.Response[0].Teams.Home.Name, Is.Not.Empty);
        Assert.That(response.Response[0].Score.Fulltime.Home, Is.Not.Null);
    }

    [Test]
    public async Task GetFixtures_WithNextFilter_DeserializesCapturedApiResponse()
    {
        var route = "fixtures?league=39&season=2026&next=1";
        var factory = MockFactory(route, GetExpected("Test_Fixtures_NextEpl_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetFixtures(league: 39, season: 2026, next: 1);

        AssertPage(response, "fixtures");
        AssertParameter(response, "league", "39");
        AssertParameter(response, "next", "1");
        Assert.That(response.Response[0].Fixture.Id, Is.EqualTo(1557430));
        Assert.That(response.Response[0].Fixture.Date.Year, Is.EqualTo(2026));
    }

    [Test]
    public async Task GetHeadToHead_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("fixtures/headtohead?h2h=415-194&last=5", GetExpected("Test_HeadToHead_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetHeadToHead("415-194", last: 5);

        AssertPage(response, "fixtures/headtohead");
        AssertParameter(response, "h2h", "415-194");
        Assert.That(response.Response.All(fixture => fixture.Teams.Home.Id > 0 && fixture.Teams.Away.Id > 0), Is.True);
    }

    [Test]
    public async Task GetFixtureStatistics_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("fixtures/statistics?fixture=1213546", GetExpected("Test_FixtureStatistics_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetFixtureStatistics(1213546);

        AssertPage(response, "fixtures/statistics");
        AssertParameter(response, "fixture", "1213546");
        Assert.That(response.Response.All(team => team.Team.Id > 0 && team.Statistics.Count > 0), Is.True);
    }

    [Test]
    public async Task GetFixtureEvents_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("fixtures/events?fixture=1213546", GetExpected("Test_FixtureEvents_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetFixtureEvents(1213546);

        AssertPage(response, "fixtures/events");
        AssertParameter(response, "fixture", "1213546");
        Assert.That(response.Response.Any(item => item.Time.Elapsed > 0 && !string.IsNullOrWhiteSpace(item.Type)), Is.True);
    }

    [Test]
    public async Task GetFixtureLineups_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("fixtures/lineups?fixture=1213546", GetExpected("Test_FixtureLineups_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetFixtureLineups(1213546);

        AssertPage(response, "fixtures/lineups");
        AssertParameter(response, "fixture", "1213546");
        Assert.That(response.Response.All(item => item.Team.Id > 0 && item.StartXI.Count > 0), Is.True);
    }

    [Test]
    public async Task GetFixturePlayers_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("fixtures/players?fixture=1213546", GetExpected("Test_FixturePlayers_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetFixturePlayers(1213546);

        AssertPage(response, "fixtures/players");
        AssertParameter(response, "fixture", "1213546");
        Assert.That(response.Response.All(team => team.Team.Id > 0 && team.Players.Count > 0), Is.True);
    }

    [Test]
    public async Task GetInjuries_DeserializesCapturedApiResponse()
    {
        var route = "injuries?league=39&season=2026&team=40";
        var factory = MockFactory(route, GetExpected("Test_Injuries_EPL_Real_Response"));
        var response = await new InjuriesClient(factory, MockOptions()).GetInjuries(league: 39, season: 2026, team: 40);

        AssertPage(response, "injuries");
        AssertParameter(response, "league", "39");
        Assert.That(response.Response.Any(item => item.Player?.Id is > 0 && item.Team?.Id == 40), Is.True);
    }

    [Test]
    public async Task GetOdds_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("odds?fixture=1557430", GetExpected("Test_Odds_UpcomingFixture_Real_Response"));
        var response = await new OddsClient(factory, MockOptions()).GetOdds(fixture: 1557430);

        AssertPage(response, "odds");
        AssertParameter(response, "fixture", "1557430");
        Assert.That(response.Response.Any(item => item.Fixture?.Id == 1557430 && item.Bookmakers is { Count: > 0 }), Is.True);
    }

    [Test]
    public async Task GetOddsMapping_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("odds/mapping?page=1", GetExpected("Test_OddsMapping_Page1_Real_Response"));
        var response = await new OddsClient(factory, MockOptions()).GetOddsMapping(page: 1);

        AssertPage(response, "odds/mapping");
        AssertParameter(response, "page", "1");
        Assert.That(response.Response.Any(item => item.Fixture?.Id is > 0 && item.League?.Id is > 0), Is.True);
    }

    [Test]
    public async Task GetBookmakers_WithSearch_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("odds/bookmakers?search=Bet365", GetExpected("Test_OddsBookmakers_Search_Real_Response"));
        var response = await new OddsClient(factory, MockOptions()).GetBookmakers(search: "Bet365");

        AssertPage(response, "odds/bookmakers");
        Assert.That(response.Response[0].Name, Is.EqualTo("Bet365"));
    }

    [Test]
    public async Task GetBets_WithSearch_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("odds/bets?search=Winner", GetExpected("Test_OddsBets_Search_Real_Response"));
        var response = await new OddsClient(factory, MockOptions()).GetBets(search: "Winner");

        AssertPage(response, "odds/bets");
        Assert.That(response.Response.Any(item => item.Id > 0 && item.Name?.Contains("Winner", StringComparison.OrdinalIgnoreCase) == true), Is.True);
    }

    [Test]
    public async Task GetLiveOdds_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("odds/live", GetExpected("Test_LiveOdds_Real_Response"));
        var response = await new OddsClient(factory, MockOptions()).GetLiveOdds();

        AssertPage(response, "odds/live");
        Assert.That(response.Response.All(item => item.Fixture?.Id is > 0 && item.Odds is { Count: > 0 }), Is.True);
    }

    [Test]
    public async Task GetLiveBets_WithSearch_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("odds/live/bets?search=Winner", GetExpected("Test_LiveBets_Search_Real_Response"));
        var response = await new OddsClient(factory, MockOptions()).GetLiveBets(search: "Winner");

        AssertPage(response, "odds/live/bets");
        Assert.That(response.Response.Any(item => item.Id > 0 && !string.IsNullOrWhiteSpace(item.Name)), Is.True);
    }

    [Test]
    public async Task GetPlayerSeasons_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("players/seasons?player=276", GetExpected("Test_PlayerSeasons_Real_Response"));
        var response = await new PlayersClient(factory, MockOptions()).GetSeasons(player: 276);

        AssertPage(response, "players/seasons");
        Assert.That(response.Response, Has.All.GreaterThan(2000));
    }

    [Test]
    public async Task GetPlayerProfiles_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("players/profiles?search=Messi", GetExpected("Test_PlayerProfiles_Real_Response"));
        var response = await new PlayersClient(factory, MockOptions()).GetProfiles(search: "Messi");

        AssertPage(response, "players/profiles");
        Assert.That(response.Response.Any(item => item.Player?.Name?.Contains("Messi", StringComparison.OrdinalIgnoreCase) == true), Is.True);
    }

    [Test]
    public async Task GetPlayerStatistics_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("players?id=276&season=2024", GetExpected("Test_PlayerStatistics_Real_Response"));
        var response = await new PlayersClient(factory, MockOptions()).GetPlayerStatistics(id: 276, season: 2024);

        AssertPage(response, "players");
        Assert.That(response.Response.Any(item => item.Player?.Id == 276 && item.Statistics is { Count: > 0 }), Is.True);
    }

    [Test]
    public async Task GetPlayerSquads_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("players/squads?team=40", GetExpected("Test_PlayerSquads_Real_Response"));
        var response = await new PlayersClient(factory, MockOptions()).GetSquads(team: 40);

        AssertPage(response, "players/squads");
        Assert.That(response.Response.Any(item => item.Team?.Id == 40 && item.Players is { Count: > 0 }), Is.True);
    }

    [Test]
    public async Task GetPlayerTeams_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("players/teams?player=276", GetExpected("Test_PlayerTeams_Real_Response"));
        var response = await new PlayersClient(factory, MockOptions()).GetTeams(276);

        AssertPage(response, "players/teams");
        AssertParameter(response, "player", "276");
        Assert.That(response.Response.Any(item => item.Team?.Id > 0 && item.Seasons is { Count: > 0 }), Is.True);
    }

    [TestCase("topscorers", "Test_TopScorers_Real_Response")]
    [TestCase("topassists", "Test_TopAssists_Real_Response")]
    [TestCase("topyellowcards", "Test_TopYellowCards_Real_Response")]
    [TestCase("topredcards", "Test_TopRedCards_Real_Response")]
    public async Task GetPlayerLeaders_DeserializesCapturedApiResponse(string endpoint, string fixture)
    {
        var route = $"players/{endpoint}?league=39&season=2024";
        var factory = MockFactory(route, GetExpected(fixture));
        var client = new PlayersClient(factory, MockOptions());
        var response = endpoint switch
        {
            "topscorers" => await client.GetTopScorers(39, 2024),
            "topassists" => await client.GetTopAssists(39, 2024),
            "topyellowcards" => await client.GetTopYellowCards(39, 2024),
            "topredcards" => await client.GetTopRedCards(39, 2024),
            _ => throw new ArgumentOutOfRangeException(nameof(endpoint), endpoint, "Unknown player leader endpoint")
        };

        AssertPage(response, $"players/{endpoint}");
        Assert.That(response.Response.All(item => item.Player?.Id > 0 && item.Statistics is { Count: > 0 }), Is.True);
    }

    [Test]
    public async Task GetPredictions_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("predictions?fixture=1557430", GetExpected("Test_Predictions_UpcomingFixture_Real_Response"));
        var response = await new PredictionsClient(factory, MockOptions()).GetPredictions(1557430);

        AssertPage(response, "predictions");
        Assert.That(response.Response[0].Predictions?.Percent, Is.Not.Null);
        Assert.That(response.Response[0].Teams?.Home?.Id, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetSidelined_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("sidelined?player=276", GetExpected("Test_Sidelined_Player_Real_Response"));
        var response = await new SidelinedClient(factory, MockOptions()).GetSidelined(player: 276);

        AssertPage(response, "sidelined");
        Assert.That(response.Response.Any(item => !string.IsNullOrWhiteSpace(item.Type) && item.Start.HasValue), Is.True);
    }

    [Test]
    public async Task GetStatus_DeserializesCapturedApiResponseWithoutAccountIdentity()
    {
        var factory = MockFactory("status", GetExpected("Test_Status_Real_Response"));
        var response = await new StatusClient(factory, MockOptions()).GetStatus();

        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo("status"));
            Assert.That(response.Errors, Is.Empty);
            Assert.That(response.Response.Subscription?.Plan, Is.Not.Null.And.Not.Empty);
            Assert.That(response.Response.Requests?.LimitDay, Is.GreaterThan(0));
            Assert.That(response.Response.Account?.Email, Is.Null);
        });
    }

    [Test]
    public async Task GetTeamStatistics_DeserializesCapturedApiResponse()
    {
        var route = "teams/statistics?league=39&season=2024&team=40";
        var factory = MockFactory(route, GetExpected("Test_TeamStatistics_Real_Response"));
        var response = await new TeamsClient(factory, MockOptions()).GetTeamStatistics(39, 2024, 40);

        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo("teams/statistics"));
            Assert.That(response.Errors, Is.Empty);
            Assert.That(response.Response.League?.Id, Is.EqualTo(39));
            Assert.That(response.Response.League?.Season, Is.EqualTo(2024));
            Assert.That(response.Response.Team?.Id, Is.EqualTo(40));
            Assert.That(response.Response.Fixtures?.Played?.Total, Is.GreaterThan(0));
        });
    }

    [Test]
    public async Task GetTransfers_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("transfers?player=276", GetExpected("Test_Transfers_Player_Real_Response"));
        var response = await new TransfersClient(factory, MockOptions()).GetTransfers(player: 276);

        AssertPage(response, "transfers");
        Assert.That(response.Response.Any(item => item.Player?.Id == 276 && item.Transfers is { Count: > 0 }), Is.True);
    }

    [Test]
    public async Task GetTrophies_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("trophies?player=276", GetExpected("Test_Trophies_Player_Real_Response"));
        var response = await new TrophiesClient(factory, MockOptions()).GetTrophies(player: 276);

        AssertPage(response, "trophies");
        Assert.That(response.Response.Any(item => !string.IsNullOrWhiteSpace(item.League) && !string.IsNullOrWhiteSpace(item.Season)), Is.True);
    }
}
