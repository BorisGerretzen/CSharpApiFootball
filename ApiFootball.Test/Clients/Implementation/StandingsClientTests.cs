using ApiFootball.Clients.Implementation;
using ApiFootball.Models;

namespace ApiFootball.Test.Clients.Implementation;

public class StandingsClientTests : BaseEndpointTest
{
    private const string Route = "standings";

    [Test]
    public async Task Test_Standings_Valid_Response()
    {
        var factory = MockFactory(Route, GetExpected(nameof(Test_Standings_Valid_Response)));
        var client = new StandingsClient(factory);
        var response = await client.GetStandings(2022, 79);
        Assert.That(response.Get, Is.EqualTo(Route));
        Assert.That(response.Errors, Has.Count.EqualTo(0));
        Assert.That(response.Parameters, Has.Count.EqualTo(2));
        Assert.That(response.Results, Is.EqualTo(1));
        Assert.That(response.Response, Has.Count.EqualTo(1));
        var item = response.Response[0];
        Assert.That(item.League.Name, Is.EqualTo("2. Bundesliga"));
        Assert.That(item.League.Id, Is.EqualTo(79));
        Assert.That(item.League.Type, Is.EqualTo(LeagueType.League));
        Assert.That(item.League.Season, Is.EqualTo(2020));

        Assert.That(item.League.Standings, Is.Not.Null);
        var standingItem = item.League.Standings[0][0];
        Assert.That(standingItem.Rank, Is.EqualTo(1));
        Assert.That(standingItem.Team.Id, Is.EqualTo(176));
        Assert.That(standingItem.Points, Is.EqualTo(67));
        Assert.That(standingItem.GoalsDiff, Is.EqualTo(27));
        Assert.That(standingItem.Group, Is.EqualTo("2. Bundesliga"));
        Assert.That(standingItem.Form, Is.EqualTo("WDWLW"));
        Assert.That(standingItem.Status, Is.EqualTo("same"));
        Assert.That(standingItem.Description, Is.EqualTo("Promotion - Bundesliga"));
        Assert.That(standingItem.All.Played, Is.EqualTo(34));
        Assert.That(standingItem.All.Win, Is.EqualTo(21));
        Assert.That(standingItem.All.Draw, Is.EqualTo(4));
        Assert.That(standingItem.All.Lose, Is.EqualTo(9));
        Assert.That(standingItem.All.Goals.For, Is.EqualTo(66));
        Assert.That(standingItem.All.Goals.Against, Is.EqualTo(39));
    }
}