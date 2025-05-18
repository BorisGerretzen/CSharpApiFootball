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
        var client = new LeaguesClient(factory);
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
        Assert.That(season.Year, Is.EqualTo(2008));
        Assert.That(season.Start.Year, Is.EqualTo(2008));
        Assert.That(season.Start.Month, Is.EqualTo(6));
        Assert.That(season.Start.Day, Is.EqualTo(7));
    }

    [Test]
    public async Task Test_Seasons_Valid_Response()
    {
        const string path = "/seasons";
        var factory = MockFactory(Route + path, GetExpected(nameof(Test_Seasons_Valid_Response)));
        var client = new LeaguesClient(factory);
        var response = await client.GetSeasons();
        Assert.That(response.Get, Is.EqualTo(Route + path));
        Assert.That(response.Errors, Has.Count.EqualTo(0));
        Assert.That(response.Parameters, Has.Count.EqualTo(0));
        Assert.That(response.Results, Is.EqualTo(16));
        Assert.That(response.Response, Has.Count.EqualTo(16));
    }
}