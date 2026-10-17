using ApiFootball.Clients.Implementation;
using ApiFootball.Models;

namespace ApiFootball.Test.Clients.Implementation;

public class LeaguesClientTests : BaseEndpointTest
{
    private const string Route = "leagues";

    [Test]
    public async Task Test_Leagues_Valid_Response()
    {
        var factory = MockFactory(Route, GetExpected(nameof(Test_Leagues_Valid_Response)));
        var client = new LeaguesClient(factory, MockOptions());
        var response = await client.GetLeagues();
        Assert.That(response.Get, Is.EqualTo(Route));
        Assert.That(response.Errors, Has.Count.EqualTo(0));
        Assert.That(response.Parameters, Has.Count.EqualTo(0));
        Assert.That(response.Results, Is.EqualTo(1));
        Assert.That(response.Response, Has.Count.EqualTo(1));
        var item = response.Response[0];
        Assert.That(item.League.Name, Is.EqualTo("Euro Championship"));
        Assert.That(item.League.Id, Is.EqualTo(4));
        Assert.That(item.League.Type, Is.EqualTo(LeagueType.Cup));
        Assert.That(item.Seasons, Has.Count.EqualTo(4));
        var season = item.Seasons[0];
        Assert.Multiple(() =>
        {
            Assert.That(item.League.Logo, Is.EqualTo("https://media-6.api-sports.io/football/leagues/4.png"));
            Assert.That(item.Country.Name, Is.EqualTo("World"));
            Assert.That(item.Country.Code, Is.Null);
            Assert.That(item.Country.Flag, Is.Null);
            Assert.That(season.Year, Is.EqualTo(2008));
            Assert.That(season.Start, Is.EqualTo(new DateOnly(2008, 6, 7)));
            Assert.That(season.End, Is.EqualTo(new DateOnly(2008, 6, 29)));
            Assert.That(season.Current, Is.False);
            Assert.That(season.Coverage.Fixtures.Events, Is.True);
            Assert.That(season.Coverage.Fixtures.Lineups, Is.True);
            Assert.That(season.Coverage.Fixtures.StatisticsFixtures, Is.False);
            Assert.That(season.Coverage.Fixtures.StatisticsPlayers, Is.False);
            Assert.That(season.Coverage.Standings, Is.False);
            Assert.That(season.Coverage.Players, Is.False);
            Assert.That(season.Coverage.TopScorers, Is.False);
            Assert.That(season.Coverage.TopAssists, Is.False);
            Assert.That(season.Coverage.TopCards, Is.False);
            Assert.That(season.Coverage.Injuries, Is.False);
            Assert.That(season.Coverage.Predictions, Is.True);
            Assert.That(season.Coverage.Odds, Is.False);
        });
    }

    [Test]
    public async Task GetLeagues_WhenError_ThrowsIfEnabled()
    {
        var factory = MockFactory(Route, GetExpected(DefaultErrorResponse));
        var client = new LeaguesClient(factory, MockOptions(o => o.ThrowOnError = true));
        var exception = await Assert.ThrowsAsync<ApiFootballException>(async () => await client.GetLeagues());
        AssertDefaultException(exception);
    }

    [Test]
    public async Task Test_Seasons_Valid_Response()
    {
        const string path = "/seasons";
        var factory = MockFactory(Route + path, GetExpected(nameof(Test_Seasons_Valid_Response)));
        var client = new LeaguesClient(factory, MockOptions());
        var response = await client.GetSeasons();
        Assert.That(response.Get, Is.EqualTo(Route + path));
        Assert.That(response.Errors, Has.Count.EqualTo(0));
        Assert.That(response.Parameters, Has.Count.EqualTo(0));
        Assert.That(response.Results, Is.EqualTo(16));
        Assert.That(response.Response, Has.Count.EqualTo(16));
        Assert.That(response.Response, Is.EqualTo(new[]
        {
            2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015,
            2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023
        }));
    }

    [Test]
    public async Task GetSeasons_WhenError_ThrowsIfEnabled()
    {
        const string path = "/seasons";
        var factory = MockFactory(Route + path, GetExpected(DefaultErrorResponse));
        var client = new LeaguesClient(factory, MockOptions(o => o.ThrowOnError = true));
        var exception = await Assert.ThrowsAsync<ApiFootballException>(async () => await client.GetSeasons());
        AssertDefaultException(exception);
    }

    [Test]
    public async Task GetLeagues_WithCountry_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("leagues?country=Netherlands", GetExpected("Test_Leagues_Netherlands_Real_Response"));
        var response = await new LeaguesClient(factory, MockOptions()).GetLeagues(country: "Netherlands");
        CapturedApiResponseAssertions.AssertPage(response, "leagues");
        CapturedApiResponseAssertions.AssertParameter(response, "country", "Netherlands");
        Assert.That(response.Response, Has.Count.EqualTo(14));
        var eredivisie = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(eredivisie.League.Id, Is.EqualTo(88));
            Assert.That(eredivisie.League.Name, Is.EqualTo("Eredivisie"));
            Assert.That(eredivisie.League.Type, Is.EqualTo(LeagueType.League));
            Assert.That(eredivisie.League.Logo, Is.EqualTo("https://media.api-sports.io/football/leagues/88.png"));
            Assert.That(eredivisie.Country.Name, Is.EqualTo("Netherlands"));
            Assert.That(eredivisie.Country.Code, Is.EqualTo("NL"));
            Assert.That(eredivisie.Country.Flag, Is.EqualTo("https://media.api-sports.io/flags/nl.svg"));
            Assert.That(eredivisie.Seasons, Has.Count.EqualTo(17));
        });
        var currentSeason = eredivisie.Seasons[^1];
        Assert.Multiple(() =>
        {
            Assert.That(currentSeason.Year, Is.EqualTo(2026));
            Assert.That(currentSeason.Start, Is.EqualTo(new DateOnly(2026, 8, 7)));
            Assert.That(currentSeason.End, Is.EqualTo(new DateOnly(2027, 5, 23)));
            Assert.That(currentSeason.Current, Is.True);
            Assert.That(currentSeason.Coverage.Fixtures.Events, Is.True);
            Assert.That(currentSeason.Coverage.Fixtures.Lineups, Is.True);
            Assert.That(currentSeason.Coverage.Fixtures.StatisticsFixtures, Is.True);
            Assert.That(currentSeason.Coverage.Fixtures.StatisticsPlayers, Is.True);
            Assert.That(currentSeason.Coverage.Standings, Is.True);
            Assert.That(currentSeason.Coverage.Players, Is.True);
            Assert.That(currentSeason.Coverage.TopScorers, Is.True);
            Assert.That(currentSeason.Coverage.TopAssists, Is.True);
            Assert.That(currentSeason.Coverage.TopCards, Is.True);
            Assert.That(currentSeason.Coverage.Injuries, Is.True);
            Assert.That(currentSeason.Coverage.Predictions, Is.True);
            Assert.That(currentSeason.Coverage.Odds, Is.True);
        });
    }
}