using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public class FixturesClientTest : BaseEndpointTest
{
    private const string Route = "fixtures";

    [Test]
    public async Task Test_Rounds_Valid_Response()
    {
        const string path = "/rounds";
        const string query = "?league=88&season=2022";
        var factory = MockFactory(Route + path + query, GetExpected(nameof(Test_Rounds_Valid_Response)));
        var client = new FixturesClient(factory, MockOptions());
        var response = await client.GetRounds(88, 2022);
        Assert.That(response.Get, Is.EqualTo(Route + path));
        Assert.That(response.Errors, Has.Count.EqualTo(0));
        Assert.That(response.Parameters, Has.Count.EqualTo(2));
        Assert.That(response.Parameters["league"], Is.EqualTo("88"));
        Assert.That(response.Parameters["season"], Is.EqualTo("2022"));
        Assert.That(response.Results, Is.EqualTo(10));
        Assert.That(response.Response, Has.Count.EqualTo(10));
        Assert.That(response.Response, Is.EqualTo(new[]
        {
            "Regular Season - 1", "Regular Season - 2", "Regular Season - 3", "Regular Season - 4",
            "Regular Season - 5", "Regular Season - 6", "Regular Season - 7", "Regular Season - 8",
            "Regular Season - 9", "Regular Season - 10"
        }));
    }

    [Test]
    public async Task GetRounds_WhenError_ThrowsIfEnabled()
    {
        const string path = "/rounds";
        const string query = "?league=88&season=2022";
        var factory = MockFactory(Route + path + query, GetExpected(DefaultErrorResponse));
        var client = new FixturesClient(factory, MockOptions(o => o.ThrowOnError = true));
        var exception = await Assert.ThrowsAsync<ApiFootballException>(async () => await client.GetRounds(88, 2022));
        AssertDefaultException(exception);
    }

    [Test]
    public async Task Test_Fixtures_Valid_Response()
    {
        const string query = "?league=88&season=2022";
        var factory = MockFactory(Route + query, GetExpected(nameof(Test_Fixtures_Valid_Response)));
        var client = new FixturesClient(factory, MockOptions());
        var response = await client.GetFixtures(league: 88, season: 2022);
        Assert.That(response.Get, Is.EqualTo(Route));
        Assert.That(response.Errors, Has.Count.EqualTo(0));
        Assert.That(response.Parameters, Has.Count.EqualTo(2));
        Assert.That(response.Parameters["league"], Is.EqualTo("88"));
        Assert.That(response.Parameters["season"], Is.EqualTo("2022"));
        Assert.That(response.Results, Is.EqualTo(2));
        Assert.That(response.Response, Has.Count.EqualTo(2));
        var item = response.Response[0];
        Assert.That(item.Fixture.Date, Is.EqualTo(new DateTimeOffset(2022, 8, 5, 18, 0, 0, TimeSpan.FromHours(0))));
        Assert.That(item.League.Id, Is.EqualTo(88));
        Assert.That(item.League.Name, Is.EqualTo("Eredivisie"));
        Assert.That(item.Teams.Home.Name, Is.EqualTo("Heerenveen"));
        Assert.That(item.Teams.Away.Name, Is.EqualTo("Sparta Rotterdam"));
        Assert.That(item.Teams.Home.Winner, Is.Null);
        Assert.That(item.Teams.Away.Winner, Is.Null);
        Assert.That(item.Score.Fulltime.Home, Is.EqualTo(0));
        Assert.That(item.Score.Fulltime.Away, Is.EqualTo(0));
        Assert.That(item.Score.Extratime.Home, Is.Null);
        Assert.That(item.Score.Extratime.Away, Is.Null);
        Assert.That(item.Goals.Home, Is.EqualTo(0));
        Assert.That(item.Goals.Away, Is.EqualTo(0));
        item = response.Response[1];
        Assert.That(item.Fixture.Date, Is.EqualTo(new DateTimeOffset(2023, 5, 28, 12, 30, 0, TimeSpan.FromHours(0))));
        Assert.That(item.League.Id, Is.EqualTo(88));
        Assert.That(item.League.Name, Is.EqualTo("Eredivisie"));
        Assert.That(item.Teams.Home.Name, Is.EqualTo("FC Volendam"));
        Assert.That(item.Teams.Away.Name, Is.EqualTo("Excelsior"));
        Assert.That(item.Teams.Home.Winner, Is.Null);
        Assert.That(item.Teams.Away.Winner, Is.Null);
        Assert.That(item.Score.Fulltime.Home, Is.Null);
        Assert.That(item.Score.Fulltime.Away, Is.Null);
        Assert.That(item.Score.Extratime.Home, Is.Null);
        Assert.That(item.Score.Extratime.Away, Is.Null);
        Assert.That(item.Goals.Home, Is.Null);
        Assert.That(item.Goals.Away, Is.Null);
    }

    [Test]
    public async Task GetFixtures_WhenError_ThrowsIfEnabled()
    {
        const string query = "?league=88&season=2022";
        var factory = MockFactory(Route + query, GetExpected(DefaultErrorResponse));
        var client = new FixturesClient(factory, MockOptions(o => o.ThrowOnError = true));
        var exception =
            await Assert.ThrowsAsync<ApiFootballException>(async () =>
                await client.GetFixtures(league: 88, season: 2022));
        AssertDefaultException(exception);
    }

    [Test]
    public async Task GetFixtures_IndividualResponse()
    {
        const string query = "?id=1552176";
        const int id = 1552176;
        var factory = MockFactory(Route + query, GetExpected(nameof(GetFixtures_IndividualResponse)));
        var client = new FixturesClient(factory, MockOptions());
        var response = await client.GetFixtures(id);
        Assert.That(response.Get, Is.EqualTo(Route));
        Assert.That(response.Errors, Is.Empty);
        Assert.That(response.Parameters, Contains.Key("id").WithValue(id.ToString()));
        Assert.That(response.Results, Is.EqualTo(1));
        Assert.That(response.Response, Has.Count.EqualTo(1));
        var fixture = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(fixture.Fixture.Id, Is.EqualTo(id));
            Assert.That(fixture.Teams.Home.Name, Is.EqualTo("Feyenoord"));
            Assert.That(fixture.Teams.Away.Name, Is.EqualTo("Utrecht"));
            Assert.That(fixture.Goals.Home, Is.EqualTo(5));
            Assert.That(fixture.Goals.Away, Is.EqualTo(0));
            Assert.That(fixture.Events, Is.Not.Empty);
            Assert.That(fixture.Lineups, Has.Count.EqualTo(2));
            Assert.That(fixture.Statistics, Has.Count.EqualTo(2));
            Assert.That(fixture.Players, Has.Count.EqualTo(2));
        });
    }

    [Test]
    public async Task GetRoundsWithDates_DeserializesCapturedApiResponse()
    {
        var route = "fixtures/rounds?league=88&season=2022&dates=true&timezone=UTC";
        var factory = MockFactory(route, GetExpected("Test_FixtureRounds_Dates_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetRoundsWithDates(88, 2022, timezone: "UTC");
        CapturedApiResponseAssertions.AssertPage(response, "fixtures/rounds");
        CapturedApiResponseAssertions.AssertParameter(response, "league", "88", 4);
        CapturedApiResponseAssertions.AssertParameter(response, "season", "2022", 4);
        CapturedApiResponseAssertions.AssertParameter(response, "dates", "true", 4);
        CapturedApiResponseAssertions.AssertParameter(response, "timezone", "UTC", 4);
        Assert.That(response.Response, Has.Count.EqualTo(37));
        Assert.That(response.Response[0].Round, Is.EqualTo("Regular Season - 1"));
        Assert.That(response.Response[0].Dates, Is.EqualTo(new[] { "2022-08-05", "2022-08-06", "2022-08-07" }));
        Assert.That(response.Response[1].Dates, Is.EqualTo(new[] { "2022-08-12", "2022-08-13", "2022-08-14" }));
    }

    [Test]
    public async Task GetRoundsInTimezone_DeserializesCapturedApiResponse()
    {
        var route = "fixtures/rounds?league=88&season=2022&timezone=Europe%2fAmsterdam";
        var factory = MockFactory(route, GetExpected("Test_FixtureRounds_Timezone_Real_Response"));
        var response =
            await new FixturesClient(factory, MockOptions()).GetRoundsInTimezone(88, 2022, "Europe/Amsterdam");
        CapturedApiResponseAssertions.AssertPage(response, "fixtures/rounds");
        CapturedApiResponseAssertions.AssertParameter(response, "league", "88", 3);
        CapturedApiResponseAssertions.AssertParameter(response, "season", "2022", 3);
        CapturedApiResponseAssertions.AssertParameter(response, "timezone", "Europe/Amsterdam", 3);
        Assert.That(response.Response, Has.Count.EqualTo(37));
        Assert.That(response.Response.Take(3),
            Is.EqualTo(new[] { "Regular Season - 1", "Regular Season - 2", "Regular Season - 3" }));
    }

    [Test]
    public async Task GetFixtures_ById_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("fixtures?id=1213546", GetExpected("Test_FixtureById_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetFixtures(1213546);
        CapturedApiResponseAssertions.AssertPage(response, "fixtures");
        CapturedApiResponseAssertions.AssertParameter(response, "id", "1213546");
        var fixture = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(fixture.Fixture.Id, Is.EqualTo(1213546));
            Assert.That(fixture.Fixture.Referee, Is.EqualTo("A. Lindhout"));
            Assert.That(fixture.Fixture.Timezone, Is.EqualTo("UTC"));
            Assert.That(fixture.Fixture.Date, Is.EqualTo(new DateTimeOffset(2024, 11, 10, 13, 30, 0, TimeSpan.Zero)));
            Assert.That(fixture.Fixture.Timestamp, Is.EqualTo(1731245400));
            Assert.That(fixture.Fixture.Periods.First, Is.EqualTo(1731245400));
            Assert.That(fixture.Fixture.Periods.Second, Is.EqualTo(1731249000));
            Assert.That(fixture.Fixture.Venue.Id, Is.EqualTo(1153));
            Assert.That(fixture.Fixture.Venue.Name, Is.EqualTo("De Grolsch Veste"));
            Assert.That(fixture.Fixture.Venue.City, Is.EqualTo("Enschede"));
            Assert.That(fixture.Fixture.Status.Long, Is.EqualTo("Match Finished"));
            Assert.That(fixture.Fixture.Status.Short, Is.EqualTo("FT"));
            Assert.That(fixture.Fixture.Status.Elapsed, Is.EqualTo(90));
            Assert.That(fixture.Fixture.Status.Extra, Is.EqualTo(5));
            Assert.That(fixture.League.Id, Is.EqualTo(88));
            Assert.That(fixture.League.Name, Is.EqualTo("Eredivisie"));
            Assert.That(fixture.League.Country, Is.EqualTo("Netherlands"));
            Assert.That(fixture.League.Logo, Is.EqualTo("https://media.api-sports.io/football/leagues/88.png"));
            Assert.That(fixture.League.Flag, Is.EqualTo("https://media.api-sports.io/flags/nl.svg"));
            Assert.That(fixture.League.Season, Is.EqualTo(2024));
            Assert.That(fixture.League.Round, Is.EqualTo("Regular Season - 12"));
            Assert.That(fixture.League.Standings, Is.True);
            Assert.That(fixture.Teams.Home.Id, Is.EqualTo(415));
            Assert.That(fixture.Teams.Home.Name, Is.EqualTo("Twente"));
            Assert.That(fixture.Teams.Home.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/415.png"));
            Assert.That(fixture.Teams.Home.Winner, Is.Null);
            Assert.That(fixture.Teams.Away.Id, Is.EqualTo(194));
            Assert.That(fixture.Teams.Away.Name, Is.EqualTo("Ajax"));
            Assert.That(fixture.Teams.Away.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/194.png"));
            Assert.That(fixture.Teams.Away.Winner, Is.Null);
            Assert.That(fixture.Goals.Home, Is.EqualTo(2));
            Assert.That(fixture.Goals.Away, Is.EqualTo(2));
            Assert.That(fixture.Score.Halftime.Home, Is.EqualTo(1));
            Assert.That(fixture.Score.Halftime.Away, Is.EqualTo(0));
            Assert.That(fixture.Score.Fulltime.Home, Is.EqualTo(2));
            Assert.That(fixture.Score.Fulltime.Away, Is.EqualTo(2));
            Assert.That(fixture.Score.Extratime.Home, Is.Null);
            Assert.That(fixture.Score.Extratime.Away, Is.Null);
            Assert.That(fixture.Score.Penalty.Home, Is.Null);
            Assert.That(fixture.Score.Penalty.Away, Is.Null);
            Assert.That(fixture.Events, Has.Count.EqualTo(16));
            Assert.That(fixture.Lineups, Has.Count.EqualTo(2));
            Assert.That(fixture.Statistics, Has.Count.EqualTo(2));
            Assert.That(fixture.Players, Has.Count.EqualTo(2));
        });
        var firstEvent = fixture.Events![0];
        Assert.Multiple(() =>
        {
            Assert.That(firstEvent.Time.Elapsed, Is.EqualTo(42));
            Assert.That(firstEvent.Time.Extra, Is.Null);
            Assert.That(firstEvent.Team.Id, Is.EqualTo(415));
            Assert.That(firstEvent.Team.Name, Is.EqualTo("Twente"));
            Assert.That(firstEvent.Team.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/415.png"));
            Assert.That(firstEvent.Player.Id, Is.EqualTo(36982));
            Assert.That(firstEvent.Player.Name, Is.EqualTo("M. Vlap"));
            Assert.That(firstEvent.Assist, Is.Null);
            Assert.That(firstEvent.Type, Is.EqualTo("Goal"));
            Assert.That(firstEvent.Detail, Is.EqualTo("Normal Goal"));
            Assert.That(firstEvent.Comments, Is.Null);
        });
        Assert.That(fixture.Events[1].Assist?.Id, Is.EqualTo(671));
        Assert.That(fixture.Events[1].Assist?.Name, Is.EqualTo("B. Traoré"));
    }

    [Test]
    public async Task GetFixtures_WithNextFilter_DeserializesCapturedApiResponse()
    {
        var route = "fixtures?league=39&season=2026&next=1";
        var factory = MockFactory(route, GetExpected("Test_Fixtures_NextEpl_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetFixtures(league: 39, season: 2026, next: 1);
        CapturedApiResponseAssertions.AssertPage(response, "fixtures");
        CapturedApiResponseAssertions.AssertParameter(response, "league", "39", 3);
        CapturedApiResponseAssertions.AssertParameter(response, "season", "2026", 3);
        CapturedApiResponseAssertions.AssertParameter(response, "next", "1", 3);
        Assert.That(response.Response, Has.Count.EqualTo(1));
        var fixture = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(fixture.Fixture.Id, Is.EqualTo(1557430));
            Assert.That(fixture.Fixture.Referee, Is.EqualTo("Paul Tierney"));
            Assert.That(fixture.Fixture.Timezone, Is.EqualTo("UTC"));
            Assert.That(fixture.Fixture.Date, Is.EqualTo(new DateTimeOffset(2026, 10, 17, 11, 30, 0, TimeSpan.Zero)));
            Assert.That(fixture.Fixture.Timestamp, Is.EqualTo(1792236600));
            Assert.That(fixture.Fixture.Periods.First, Is.Null);
            Assert.That(fixture.Fixture.Periods.Second, Is.Null);
            Assert.That(fixture.Fixture.Venue.Id, Is.EqualTo(22033));
            Assert.That(fixture.Fixture.Venue.Name, Is.EqualTo("Hill Dickinson Stadium"));
            Assert.That(fixture.Fixture.Venue.City, Is.EqualTo("Liverpool"));
            Assert.That(fixture.Fixture.Status.Long, Is.EqualTo("Not Started"));
            Assert.That(fixture.Fixture.Status.Short, Is.EqualTo("NS"));
            Assert.That(fixture.Fixture.Status.Elapsed, Is.Null);
            Assert.That(fixture.Fixture.Status.Extra, Is.Null);
            Assert.That(fixture.League.Id, Is.EqualTo(39));
            Assert.That(fixture.League.Name, Is.EqualTo("Premier League"));
            Assert.That(fixture.League.Country, Is.EqualTo("England"));
            Assert.That(fixture.League.Logo, Is.EqualTo("https://media.api-sports.io/football/leagues/39.png"));
            Assert.That(fixture.League.Flag, Is.EqualTo("https://media.api-sports.io/flags/gb-eng.svg"));
            Assert.That(fixture.League.Season, Is.EqualTo(2026));
            Assert.That(fixture.League.Round, Is.EqualTo("Regular Season - 7"));
            Assert.That(fixture.League.Standings, Is.True);
            Assert.That(fixture.Teams.Home.Id, Is.EqualTo(45));
            Assert.That(fixture.Teams.Home.Name, Is.EqualTo("Everton"));
            Assert.That(fixture.Teams.Home.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/45.png"));
            Assert.That(fixture.Teams.Home.Winner, Is.Null);
            Assert.That(fixture.Teams.Away.Id, Is.EqualTo(49));
            Assert.That(fixture.Teams.Away.Name, Is.EqualTo("Chelsea"));
            Assert.That(fixture.Teams.Away.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/49.png"));
            Assert.That(fixture.Teams.Away.Winner, Is.Null);
            Assert.That(fixture.Goals.Home, Is.Null);
            Assert.That(fixture.Goals.Away, Is.Null);
            Assert.That(fixture.Score.Halftime.Home, Is.Null);
            Assert.That(fixture.Score.Halftime.Away, Is.Null);
            Assert.That(fixture.Score.Fulltime.Home, Is.Null);
            Assert.That(fixture.Score.Fulltime.Away, Is.Null);
            Assert.That(fixture.Score.Extratime.Home, Is.Null);
            Assert.That(fixture.Score.Extratime.Away, Is.Null);
            Assert.That(fixture.Score.Penalty.Home, Is.Null);
            Assert.That(fixture.Score.Penalty.Away, Is.Null);
        });
    }

    [Test]
    public async Task GetHeadToHead_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("fixtures/headtohead?h2h=415-194&last=5",
            GetExpected("Test_HeadToHead_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetHeadToHead("415-194", last: 5);
        CapturedApiResponseAssertions.AssertPage(response, "fixtures/headtohead");
        CapturedApiResponseAssertions.AssertParameter(response, "h2h", "415-194", 2);
        CapturedApiResponseAssertions.AssertParameter(response, "last", "5", 2);
        Assert.That(response.Response, Has.Count.EqualTo(5));
        var first = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(first.Fixture.Id, Is.EqualTo(1542241));
            Assert.That(first.Fixture.Referee, Is.Null);
            Assert.That(first.Fixture.Timezone, Is.EqualTo("UTC"));
            Assert.That(first.Fixture.Date, Is.EqualTo(new DateTimeOffset(2026, 4, 16, 12, 0, 0, TimeSpan.Zero)));
            Assert.That(first.Fixture.Timestamp, Is.EqualTo(1776340800));
            Assert.That(first.Fixture.Periods.First, Is.EqualTo(1776340800));
            Assert.That(first.Fixture.Periods.Second, Is.EqualTo(1776344400));
            Assert.That(first.Fixture.Venue.Id, Is.Null);
            Assert.That(first.Fixture.Venue.Name, Is.Null);
            Assert.That(first.Fixture.Venue.City, Is.Null);
            Assert.That(first.Fixture.Status.Long, Is.EqualTo("Match Finished"));
            Assert.That(first.Fixture.Status.Short, Is.EqualTo("FT"));
            Assert.That(first.Fixture.Status.Elapsed, Is.EqualTo(90));
            Assert.That(first.Fixture.Status.Extra, Is.Null);
            Assert.That(first.League.Id, Is.EqualTo(667));
            Assert.That(first.League.Name, Is.EqualTo("Friendlies Clubs"));
            Assert.That(first.League.Country, Is.EqualTo("World"));
            Assert.That(first.League.Logo, Is.EqualTo("https://media.api-sports.io/football/leagues/667.png"));
            Assert.That(first.League.Flag, Is.Null);
            Assert.That(first.League.Season, Is.EqualTo(2026));
            Assert.That(first.League.Round, Is.EqualTo("Club Friendly"));
            Assert.That(first.League.Standings, Is.False);
            Assert.That(first.Teams.Home.Id, Is.EqualTo(194));
            Assert.That(first.Teams.Home.Name, Is.EqualTo("Ajax"));
            Assert.That(first.Teams.Home.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/194.png"));
            Assert.That(first.Teams.Home.Winner, Is.False);
            Assert.That(first.Teams.Away.Id, Is.EqualTo(415));
            Assert.That(first.Teams.Away.Name, Is.EqualTo("Twente"));
            Assert.That(first.Teams.Away.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/415.png"));
            Assert.That(first.Teams.Away.Winner, Is.True);
            Assert.That(first.Goals.Home, Is.EqualTo(0));
            Assert.That(first.Goals.Away, Is.EqualTo(1));
            Assert.That(first.Score.Halftime.Home, Is.EqualTo(0));
            Assert.That(first.Score.Halftime.Away, Is.EqualTo(0));
            Assert.That(first.Score.Fulltime.Home, Is.EqualTo(0));
            Assert.That(first.Score.Fulltime.Away, Is.EqualTo(1));
            Assert.That(first.Score.Extratime.Home, Is.Null);
            Assert.That(first.Score.Extratime.Away, Is.Null);
            Assert.That(first.Score.Penalty.Home, Is.Null);
            Assert.That(first.Score.Penalty.Away, Is.Null);
        });
    }

    [Test]
    public async Task GetFixtureStatistics_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("fixtures/statistics?fixture=1213546",
            GetExpected("Test_FixtureStatistics_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetFixtureStatistics(1213546);
        CapturedApiResponseAssertions.AssertPage(response, "fixtures/statistics");
        CapturedApiResponseAssertions.AssertParameter(response, "fixture", "1213546");
        Assert.That(response.Response, Has.Count.EqualTo(2));
        Assert.That(response.Response[0].Team.Id, Is.EqualTo(415));
        Assert.That(response.Response[0].Team.Name, Is.EqualTo("Twente"));
        Assert.That(response.Response[0].Team.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/415.png"));
        Assert.That(response.Response[0].Statistics, Has.Count.EqualTo(18));
        var stats = response.Response[0].Statistics;
        Assert.That(stats.Select(item => (item.Type, item.Value?.ToString())).ToArray(), Is.EqualTo(new[]
        {
            ("Shots on Goal", "7"), ("Shots off Goal", "5"), ("Total Shots", "17"), ("Blocked Shots", "5"),
            ("Shots insidebox", "12"), ("Shots outsidebox", "5"), ("Fouls", "13"), ("Corner Kicks", "5"),
            ("Offsides", "2"), ("Ball Possession", "39%"), ("Yellow Cards", "1"), ("Red Cards", null),
            ("Goalkeeper Saves", "4"), ("Total passes", "402"), ("Passes accurate", "329"), ("Passes %", "82%"),
            ("expected_goals", "1.80"), ("goals_prevented", "0.65")
        }));
    }

    [Test]
    public async Task GetFixtureEvents_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("fixtures/events?fixture=1213546", GetExpected("Test_FixtureEvents_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetFixtureEvents(1213546);
        CapturedApiResponseAssertions.AssertPage(response, "fixtures/events");
        CapturedApiResponseAssertions.AssertParameter(response, "fixture", "1213546");
        Assert.That(response.Response, Has.Count.EqualTo(16));
        var first = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(first.Time.Elapsed, Is.EqualTo(42));
            Assert.That(first.Time.Extra, Is.Null);
            Assert.That(first.Team.Id, Is.EqualTo(415));
            Assert.That(first.Team.Name, Is.EqualTo("Twente"));
            Assert.That(first.Team.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/415.png"));
            Assert.That(first.Player.Id, Is.EqualTo(36982));
            Assert.That(first.Player.Name, Is.EqualTo("M. Vlap"));
            Assert.That(first.Assist, Is.Null);
            Assert.That(first.Type, Is.EqualTo("Goal"));
            Assert.That(first.Detail, Is.EqualTo("Normal Goal"));
            Assert.That(first.Comments, Is.Null);
        });
        var substitution = response.Response[1];
        Assert.Multiple(() =>
        {
            Assert.That(substitution.Time.Elapsed, Is.EqualTo(46));
            Assert.That(substitution.Team.Id, Is.EqualTo(194));
            Assert.That(substitution.Player.Id, Is.EqualTo(340153));
            Assert.That(substitution.Player.Name, Is.EqualTo("M. Godts"));
            Assert.That(substitution.Assist?.Id, Is.EqualTo(671));
            Assert.That(substitution.Assist?.Name, Is.EqualTo("B. Traoré"));
            Assert.That(substitution.Type, Is.EqualTo("subst"));
            Assert.That(substitution.Detail, Is.EqualTo("Substitution 1"));
            Assert.That(substitution.Comments, Is.Null);
        });
    }

    [Test]
    public async Task GetFixtureLineups_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("fixtures/lineups?fixture=1213546", GetExpected("Test_FixtureLineups_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetFixtureLineups(1213546);
        CapturedApiResponseAssertions.AssertPage(response, "fixtures/lineups");
        CapturedApiResponseAssertions.AssertParameter(response, "fixture", "1213546");
        Assert.That(response.Response, Has.Count.EqualTo(2));
        var lineup = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(lineup.Team.Id, Is.EqualTo(415));
            Assert.That(lineup.Team.Name, Is.EqualTo("Twente"));
            Assert.That(lineup.Team.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/415.png"));
            Assert.That(lineup.Team.Colors.Player.Primary, Is.EqualTo("d10000"));
            Assert.That(lineup.Team.Colors.Player.Number, Is.EqualTo("ffffff"));
            Assert.That(lineup.Team.Colors.Player.Border, Is.EqualTo("d10000"));
            Assert.That(lineup.Team.Colors.GoalKeeper.Primary, Is.EqualTo("feac4d"));
            Assert.That(lineup.Team.Colors.GoalKeeper.Number, Is.EqualTo("ffffff"));
            Assert.That(lineup.Team.Colors.GoalKeeper.Border, Is.EqualTo("feac4d"));
            Assert.That(lineup.Coach.Id, Is.EqualTo(6671));
            Assert.That(lineup.Coach.Name, Is.EqualTo("J. Oosting"));
            Assert.That(lineup.Coach.Photo, Is.EqualTo("https://media.api-sports.io/football/coachs/6671.png"));
            Assert.That(lineup.Formation, Is.EqualTo("4-2-3-1"));
            Assert.That(lineup.StartXI, Has.Count.EqualTo(11));
            Assert.That(lineup.Substitutes, Has.Count.EqualTo(12));
        });
        Assert.Multiple(() =>
        {
            Assert.That(lineup.StartXI[0].Player.Id, Is.EqualTo(37765));
            Assert.That(lineup.StartXI[0].Player.Name, Is.EqualTo("L. Unnerstall"));
            Assert.That(lineup.StartXI[0].Player.Number, Is.EqualTo(1));
            Assert.That(lineup.StartXI[0].Player.Pos, Is.EqualTo("G"));
            Assert.That(lineup.StartXI[0].Player.Grid, Is.EqualTo("1:1"));
            Assert.That(lineup.Substitutes[0].Player.Id, Is.EqualTo(37784));
            Assert.That(lineup.Substitutes[0].Player.Name, Is.EqualTo("S. Steijn"));
            Assert.That(lineup.Substitutes[0].Player.Number, Is.EqualTo(14));
            Assert.That(lineup.Substitutes[0].Player.Pos, Is.EqualTo("M"));
            Assert.That(lineup.Substitutes[0].Player.Grid, Is.Null);
        });
    }

    [Test]
    public async Task GetFixturePlayers_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("fixtures/players?fixture=1213546", GetExpected("Test_FixturePlayers_Real_Response"));
        var response = await new FixturesClient(factory, MockOptions()).GetFixturePlayers(1213546);
        CapturedApiResponseAssertions.AssertPage(response, "fixtures/players");
        CapturedApiResponseAssertions.AssertParameter(response, "fixture", "1213546");
        Assert.That(response.Response, Has.Count.EqualTo(2));
        var twente = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(twente.Team.Id, Is.EqualTo(415));
            Assert.That(twente.Team.Name, Is.EqualTo("Twente"));
            Assert.That(twente.Team.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/415.png"));
            Assert.That(twente.Team.Update, Is.EqualTo(new DateTimeOffset(2025, 6, 3, 4, 10, 47, TimeSpan.Zero)));
            Assert.That(twente.Players, Has.Count.EqualTo(23));
        });
        var keeper = twente.Players[0];
        Assert.Multiple(() =>
        {
            Assert.That(keeper.Player.Id, Is.EqualTo(37765));
            Assert.That(keeper.Player.Name, Is.EqualTo("Lars Unnerstall"));
            Assert.That(keeper.Player.Photo, Is.EqualTo("https://media.api-sports.io/football/players/37765.png"));
            Assert.That(keeper.Statistics, Has.Count.EqualTo(1));
        });
        var keeperStats = keeper.Statistics[0];
        Assert.Multiple(() =>
        {
            Assert.That(keeperStats.Games.Minutes, Is.EqualTo(90));
            Assert.That(keeperStats.Games.Number, Is.EqualTo(1));
            Assert.That(keeperStats.Games.Position, Is.EqualTo("G"));
            Assert.That(keeperStats.Games.Rating, Is.EqualTo("6.9"));
            Assert.That(keeperStats.Games.Captain, Is.True);
            Assert.That(keeperStats.Games.Substitute, Is.False);
            Assert.That(keeperStats.Offsides, Is.Null);
            Assert.That(keeperStats.Shots?.Total, Is.Null);
            Assert.That(keeperStats.Shots?.On, Is.Null);
            Assert.That(keeperStats.Goals?.Total, Is.Null);
            Assert.That(keeperStats.Goals?.Conceded, Is.EqualTo(2));
            Assert.That(keeperStats.Goals?.Assists, Is.EqualTo(0));
            Assert.That(keeperStats.Goals?.Saves, Is.EqualTo(4));
            Assert.That(keeperStats.Passes?.Total, Is.EqualTo(25));
            Assert.That(keeperStats.Passes?.Key, Is.Null);
            Assert.That(keeperStats.Passes?.Accuracy, Is.EqualTo("15"));
            Assert.That(keeperStats.Tackles?.Total, Is.Null);
            Assert.That(keeperStats.Tackles?.Blocks, Is.Null);
            Assert.That(keeperStats.Tackles?.Interceptions, Is.Null);
            Assert.That(keeperStats.Duels?.Total, Is.EqualTo(1));
            Assert.That(keeperStats.Duels?.Won, Is.EqualTo(1));
            Assert.That(keeperStats.Dribbles?.Attempts, Is.EqualTo(1));
            Assert.That(keeperStats.Dribbles?.Success, Is.EqualTo(1));
            Assert.That(keeperStats.Dribbles?.Past, Is.Null);
            Assert.That(keeperStats.Fouls?.Drawn, Is.Null);
            Assert.That(keeperStats.Fouls?.Committed, Is.Null);
            Assert.That(keeperStats.Cards?.Yellow, Is.EqualTo(0));
            Assert.That(keeperStats.Cards?.Red, Is.EqualTo(0));
            Assert.That(keeperStats.Penalty?.Won, Is.Null);
            Assert.That(keeperStats.Penalty?.Committed, Is.Null);
            Assert.That(keeperStats.Penalty?.Scored, Is.EqualTo(0));
            Assert.That(keeperStats.Penalty?.Missed, Is.EqualTo(0));
            Assert.That(keeperStats.Penalty?.Saved, Is.EqualTo(0));
        });
        var defenderStats = twente.Players[1].Statistics[0];
        Assert.Multiple(() =>
        {
            Assert.That(twente.Players[1].Player.Id, Is.EqualTo(37370));
            Assert.That(twente.Players[1].Player.Name, Is.EqualTo("Bart van Rooij"));
            Assert.That(twente.Players[1].Player.Photo,
                Is.EqualTo("https://media.api-sports.io/football/players/37370.png"));
            Assert.That(defenderStats.Games.Minutes, Is.EqualTo(90));
            Assert.That(defenderStats.Games.Number, Is.EqualTo(28));
            Assert.That(defenderStats.Games.Position, Is.EqualTo("D"));
            Assert.That(defenderStats.Games.Rating, Is.EqualTo("7.2"));
            Assert.That(defenderStats.Games.Captain, Is.False);
            Assert.That(defenderStats.Games.Substitute, Is.False);
            Assert.That(defenderStats.Offsides, Is.Null);
            Assert.That(defenderStats.Shots?.Total, Is.Null);
            Assert.That(defenderStats.Shots?.On, Is.Null);
            Assert.That(defenderStats.Goals?.Total, Is.Null);
            Assert.That(defenderStats.Goals?.Conceded, Is.EqualTo(0));
            Assert.That(defenderStats.Goals?.Assists, Is.EqualTo(0));
            Assert.That(defenderStats.Goals?.Saves, Is.Null);
            Assert.That(defenderStats.Passes?.Total, Is.EqualTo(45));
            Assert.That(defenderStats.Passes?.Key, Is.EqualTo(4));
            Assert.That(defenderStats.Passes?.Accuracy, Is.EqualTo("32"));
            Assert.That(defenderStats.Tackles?.Total, Is.EqualTo(3));
            Assert.That(defenderStats.Tackles?.Blocks, Is.Null);
            Assert.That(defenderStats.Tackles?.Interceptions, Is.EqualTo(1));
            Assert.That(defenderStats.Duels?.Total, Is.EqualTo(10));
            Assert.That(defenderStats.Duels?.Won, Is.EqualTo(6));
            Assert.That(defenderStats.Dribbles?.Attempts, Is.Null);
            Assert.That(defenderStats.Dribbles?.Success, Is.Null);
            Assert.That(defenderStats.Dribbles?.Past, Is.EqualTo(3));
            Assert.That(defenderStats.Fouls?.Drawn, Is.EqualTo(1));
            Assert.That(defenderStats.Fouls?.Committed, Is.Null);
            Assert.That(defenderStats.Cards?.Yellow, Is.EqualTo(0));
            Assert.That(defenderStats.Cards?.Red, Is.EqualTo(0));
            Assert.That(defenderStats.Penalty?.Won, Is.Null);
            Assert.That(defenderStats.Penalty?.Committed, Is.Null);
            Assert.That(defenderStats.Penalty?.Scored, Is.EqualTo(0));
            Assert.That(defenderStats.Penalty?.Missed, Is.EqualTo(0));
            Assert.That(defenderStats.Penalty?.Saved, Is.Null);
        });
    }
}