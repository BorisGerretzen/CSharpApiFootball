using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public class TeamsClientTests : BaseEndpointTest
{
    private const string Route = "teams";

    [Test]
    public async Task Test_TeamsInformation_Valid_Response()
    {
        var factory = MockFactory(Route + "?id=69", GetExpected(nameof(Test_TeamsInformation_Valid_Response)));
        var client = new TeamsClient(factory, MockOptions());
        var response = await client.GetTeamsInformation(69);
        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo(Route));
            Assert.That(response.Errors, Has.Count.EqualTo(0));
            Assert.That(response.Parameters, Has.Count.EqualTo(1));
            Assert.That(response.Results, Is.EqualTo(1));
            Assert.That(response.Response, Has.Count.EqualTo(1));
        });
        var item = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(item.Team.Id, Is.EqualTo(69));
            Assert.That(item.Team.Name, Is.EqualTo("Derby"));
            Assert.That(item.Team.Code, Is.EqualTo("DER"));
            Assert.That(item.Team.Country, Is.EqualTo("England"));
            Assert.That(item.Team.Founded, Is.EqualTo(1884));
            Assert.That(item.Team.National, Is.False);
            Assert.That(item.Team.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/69.png"));
            Assert.That(item.Venue.Id, Is.EqualTo(527));
            Assert.That(item.Venue.Name, Is.EqualTo("Pride Park Stadium"));
            Assert.That(item.Venue.Address, Is.EqualTo("Pride Park, Royal Way"));
            Assert.That(item.Venue.City, Is.EqualTo("Derby"));
            Assert.That(item.Venue.Capacity, Is.EqualTo(33597));
            Assert.That(item.Venue.Surface, Is.EqualTo("grass"));
            Assert.That(item.Venue.Image, Is.EqualTo("https://media.api-sports.io/football/venues/527.png"));
        });
    }

    [Test]
    public async Task GetTeamsInformation_WhenError_ThrowsIfEnabled()
    {
        var factory = MockFactory(Route + "?id=69", GetExpected(DefaultErrorResponse));
        var client = new TeamsClient(factory, MockOptions(o => o.ThrowOnError = true));
        var exception =
            await Assert.ThrowsAsync<ApiFootballException>(async () => await client.GetTeamsInformation(69));
        AssertDefaultException(exception);
    }

    [Test]
    public async Task Test_TeamsSeasons_Valid_Response()
    {
        const string route = "teams/seasons";
        var factory = MockFactory(route + "?team=69", GetExpected(nameof(Test_TeamsSeasons_Valid_Response)));
        var client = new TeamsClient(factory, MockOptions());
        var response = await client.GetTeamsSeasons(69);
        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo(route));
            Assert.That(response.Errors, Has.Count.EqualTo(0));
            Assert.That(response.Parameters, Has.Count.EqualTo(1));
            Assert.That(response.Parameters, Contains.Key("team").WithValue("69"));
            Assert.That(response.Results, Is.EqualTo(14));
            Assert.That(response.Response, Has.Count.EqualTo(14));
        });
        Assert.That(response.Response, Is.EqualTo(new[]
        {
            2011, 2012, 2013, 2014, 2015, 2016, 2017,
            2018, 2019, 2020, 2021, 2022, 2023, 2024
        }));
    }

    [Test]
    public async Task GetTeamsSeasons_WhenError_ThrowsIfEnabled()
    {
        const string route = "teams/seasons?team=69";
        var factory = MockFactory(route, GetExpected(DefaultErrorResponse));
        var client = new TeamsClient(factory, MockOptions(o => o.ThrowOnError = true));
        var exception = await Assert.ThrowsAsync<ApiFootballException>(async () => await client.GetTeamsSeasons(69));
        AssertDefaultException(exception);
    }

    [Test]
    public async Task Test_TeamsCountries_Valid_Response()
    {
        const string route = "teams/countries";
        var factory = MockFactory(route, GetExpected(nameof(Test_TeamsCountries_Valid_Response)));
        var client = new TeamsClient(factory, MockOptions());
        var response = await client.GetTeamsCountries();
        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo(route));
            Assert.That(response.Errors, Has.Count.EqualTo(0));
            Assert.That(response.Parameters, Has.Count.EqualTo(0));
            Assert.That(response.Results, Is.EqualTo(271));
            Assert.That(response.Response, Has.Count.EqualTo(271));
        });
        var netherlands = response.Response.FirstOrDefault(c => c.Name == "Netherlands");
        var england = response.Response.FirstOrDefault(c => c.Name == "England");
        Assert.Multiple(() =>
        {
            Assert.That(netherlands?.Name, Is.EqualTo("Netherlands"));
            Assert.That(netherlands?.Code, Is.EqualTo("NL"));
            Assert.That(netherlands?.Flag, Is.EqualTo("https://media.api-sports.io/flags/nl.svg"));
            Assert.That(england?.Name, Is.EqualTo("England"));
            Assert.That(england?.Code, Is.EqualTo("GB-ENG"));
            Assert.That(england?.Flag, Is.EqualTo("https://media.api-sports.io/flags/gb-eng.svg"));
        });
    }

    [Test]
    public async Task GetTeamsCountries_WhenError_ThrowsIfEnabled()
    {
        const string route = "teams/countries";
        var factory = MockFactory(route, GetExpected(DefaultErrorResponse));
        var client = new TeamsClient(factory, MockOptions(o => o.ThrowOnError = true));
        var exception = await Assert.ThrowsAsync<ApiFootballException>(async () => await client.GetTeamsCountries());
        AssertDefaultException(exception);
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
            Assert.That(response.Parameters, Is.EqualTo(new Dictionary<string, string>
            {
                ["league"] = "39", ["season"] = "2024", ["team"] = "40"
            }));
            Assert.That(response.Results, Is.EqualTo(11));
            Assert.That(response.Response.League?.Id, Is.EqualTo(39));
            Assert.That(response.Response.League?.Name, Is.EqualTo("Premier League"));
            Assert.That(response.Response.League?.Country, Is.EqualTo("England"));
            Assert.That(response.Response.League?.Logo,
                Is.EqualTo("https://media.api-sports.io/football/leagues/39.png"));
            Assert.That(response.Response.League?.Flag, Is.EqualTo("https://media.api-sports.io/flags/gb-eng.svg"));
            Assert.That(response.Response.League?.Season, Is.EqualTo(2024));
            Assert.That(response.Response.Team?.Id, Is.EqualTo(40));
            Assert.That(response.Response.Team?.Name, Is.EqualTo("Liverpool"));
            Assert.That(response.Response.Team?.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/40.png"));
            Assert.That(response.Response.Form, Is.EqualTo("WWWLWWWWDWWWWDDWWWDDWWWDWDWWWWLWWWLDLD"));
            Assert.That(response.Response.Fixtures?.Played?.Home, Is.EqualTo(19));
            Assert.That(response.Response.Fixtures?.Played?.Away, Is.EqualTo(19));
            Assert.That(response.Response.Fixtures?.Played?.Total, Is.EqualTo(38));
            Assert.That(response.Response.Fixtures?.Wins?.Home, Is.EqualTo(14));
            Assert.That(response.Response.Fixtures?.Wins?.Away, Is.EqualTo(11));
            Assert.That(response.Response.Fixtures?.Wins?.Total, Is.EqualTo(25));
            Assert.That(response.Response.Fixtures?.Draws?.Home, Is.EqualTo(4));
            Assert.That(response.Response.Fixtures?.Draws?.Away, Is.EqualTo(5));
            Assert.That(response.Response.Fixtures?.Draws?.Total, Is.EqualTo(9));
            Assert.That(response.Response.Fixtures?.Loses?.Home, Is.EqualTo(1));
            Assert.That(response.Response.Fixtures?.Loses?.Away, Is.EqualTo(3));
            Assert.That(response.Response.Fixtures?.Loses?.Total, Is.EqualTo(4));
            Assert.That(response.Response.Goals?.For?.Total?.Home, Is.EqualTo(42));
            Assert.That(response.Response.Goals?.For?.Total?.Away, Is.EqualTo(44));
            Assert.That(response.Response.Goals?.For?.Total?.Total, Is.EqualTo(86));
            Assert.That(response.Response.Goals?.For?.Average?.Home, Is.EqualTo("2.2"));
            Assert.That(response.Response.Goals?.For?.Average?.Away, Is.EqualTo("2.3"));
            Assert.That(response.Response.Goals?.For?.Average?.Total, Is.EqualTo("2.3"));
            Assert.That(response.Response.Goals?.For?.Minute, Has.Count.EqualTo(8));
            Assert.That(response.Response.Goals?.For?.Minute?["0-15"].Total, Is.EqualTo(9));
            Assert.That(response.Response.Goals?.For?.Minute?["0-15"].Percentage, Is.EqualTo("10.34%"));
            Assert.That(response.Response.Goals?.For?.UnderOver, Has.Count.EqualTo(5));
            Assert.That(response.Response.Goals?.For?.UnderOver?["0.5"].GetProperty("over").GetInt32(), Is.EqualTo(37));
            Assert.That(response.Response.Goals?.For?.UnderOver?["0.5"].GetProperty("under").GetInt32(), Is.EqualTo(1));
            Assert.That(response.Response.Goals?.Against?.Total?.Home, Is.EqualTo(16));
            Assert.That(response.Response.Goals?.Against?.Total?.Away, Is.EqualTo(25));
            Assert.That(response.Response.Goals?.Against?.Total?.Total, Is.EqualTo(41));
            Assert.That(response.Response.Goals?.Against?.Average?.Home, Is.EqualTo("0.8"));
            Assert.That(response.Response.Goals?.Against?.Average?.Away, Is.EqualTo("1.3"));
            Assert.That(response.Response.Goals?.Against?.Average?.Total, Is.EqualTo("1.1"));
            Assert.That(response.Response.Biggest?.Streak?.Wins, Is.EqualTo(4));
            Assert.That(response.Response.Biggest?.Streak?.Draws, Is.EqualTo(2));
            Assert.That(response.Response.Biggest?.Streak?.Loses, Is.EqualTo(1));
            Assert.That(response.Response.Biggest?.Wins?.Home, Is.EqualTo("5-1"));
            Assert.That(response.Response.Biggest?.Wins?.Away, Is.EqualTo("0-5"));
            Assert.That(response.Response.Biggest?.Loses?.Home, Is.EqualTo("0-1"));
            Assert.That(response.Response.Biggest?.Loses?.Away, Is.EqualTo("3-1"));
            Assert.That(response.Response.Biggest?.Goals?.For?.Home, Is.EqualTo(5));
            Assert.That(response.Response.Biggest?.Goals?.For?.Away, Is.EqualTo(6));
            Assert.That(response.Response.Biggest?.Goals?.Against?.Home, Is.EqualTo(2));
            Assert.That(response.Response.Biggest?.Goals?.Against?.Away, Is.EqualTo(3));
            Assert.That(response.Response.CleanSheet?.Home, Is.EqualTo(6));
            Assert.That(response.Response.CleanSheet?.Away, Is.EqualTo(8));
            Assert.That(response.Response.CleanSheet?.Total, Is.EqualTo(14));
            Assert.That(response.Response.FailedToScore?.Home, Is.EqualTo(1));
            Assert.That(response.Response.FailedToScore?.Away, Is.EqualTo(0));
            Assert.That(response.Response.FailedToScore?.Total, Is.EqualTo(1));
            Assert.That(response.Response.Penalty?.Scored?.Total, Is.EqualTo(9));
            Assert.That(response.Response.Penalty?.Scored?.Percentage, Is.EqualTo("100.00%"));
            Assert.That(response.Response.Penalty?.Missed?.Total, Is.EqualTo(0));
            Assert.That(response.Response.Penalty?.Missed?.Percentage, Is.EqualTo("0%"));
            Assert.That(response.Response.Lineups, Has.Count.EqualTo(2));
            Assert.That(response.Response.Lineups![0].Formation, Is.EqualTo("4-2-3-1"));
            Assert.That(response.Response.Lineups[0].Played, Is.EqualTo(36));
            Assert.That(response.Response.Lineups[1].Formation, Is.EqualTo("4-3-3"));
            Assert.That(response.Response.Lineups[1].Played, Is.EqualTo(2));
            Assert.That(response.Response.Cards, Has.Count.EqualTo(2));
            Assert.That(response.Response.Cards!["yellow"], Has.Count.EqualTo(8));
            Assert.That(response.Response.Cards["yellow"]["0-15"].Total, Is.EqualTo(4));
            Assert.That(response.Response.Cards["yellow"]["0-15"].Percentage, Is.EqualTo("5.97%"));
            Assert.That(response.Response.Cards["red"]["16-30"].Total, Is.EqualTo(1));
            Assert.That(response.Response.Cards["red"]["16-30"].Percentage, Is.EqualTo("33.33%"));
        });
        CapturedApiResponseAssertions.AssertMinuteCounts(response.Response.Goals!.For!.Minute,
            ("0-15", 9, "10.34%"), ("16-30", 16, "18.39%"), ("31-45", 11, "12.64%"),
            ("46-60", 17, "19.54%"), ("61-75", 17, "19.54%"), ("76-90", 15, "17.24%"),
            ("91-105", 2, "2.30%"), ("106-120", null, null));
        CapturedApiResponseAssertions.AssertUnderOver(response.Response.Goals.For.UnderOver,
            ("0.5", 37, 1), ("1.5", 31, 7), ("2.5", 10, 28), ("3.5", 4, 34), ("4.5", 3, 35));
        CapturedApiResponseAssertions.AssertMinuteCounts(response.Response.Goals.Against!.Minute,
            ("0-15", 9, "22.50%"), ("16-30", 1, "2.50%"), ("31-45", 8, "20.00%"),
            ("46-60", 7, "17.50%"), ("61-75", 7, "17.50%"), ("76-90", 6, "15.00%"),
            ("91-105", 2, "5.00%"), ("106-120", null, null));
        CapturedApiResponseAssertions.AssertUnderOver(response.Response.Goals.Against.UnderOver,
            ("0.5", 24, 14), ("1.5", 12, 26), ("2.5", 5, 33), ("3.5", 0, 38), ("4.5", 0, 38));
        CapturedApiResponseAssertions.AssertMinuteCounts(response.Response.Cards!["yellow"],
            ("0-15", 4, "5.97%"), ("16-30", 4, "5.97%"), ("31-45", 15, "22.39%"),
            ("46-60", 14, "20.90%"), ("61-75", 12, "17.91%"), ("76-90", 10, "14.93%"),
            ("91-105", 8, "11.94%"), ("106-120", null, null));
        CapturedApiResponseAssertions.AssertMinuteCounts(response.Response.Cards["red"],
            ("0-15", null, null), ("16-30", 1, "33.33%"), ("31-45", null, null),
            ("46-60", null, null), ("61-75", 1, "33.33%"), ("76-90", null, null),
            ("91-105", 1, "33.33%"), ("106-120", null, null));
    }
}