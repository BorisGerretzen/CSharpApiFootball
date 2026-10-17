using ApiFootball.Clients.Implementation;
using ApiFootball.Models;

namespace ApiFootball.Test.Clients.Implementation;

public class StandingsClientTests : BaseEndpointTest
{
    private const string Route = "standings";

    [Test]
    public async Task Test_Standings_Valid_Response()
    {
        var factory = MockFactory(Route + "?season=2020&league=79", GetExpected(nameof(Test_Standings_Valid_Response)));
        var client = new StandingsClient(factory, MockOptions());
        var response = await client.GetStandings(2020, 79);
        Assert.That(response.Get, Is.EqualTo(Route));
        Assert.That(response.Errors, Has.Count.EqualTo(0));
        Assert.That(response.Parameters, Has.Count.EqualTo(2));
        Assert.That(response.Parameters["season"], Is.EqualTo("2020"));
        Assert.That(response.Parameters["league"], Is.EqualTo("79"));
        Assert.That(response.Results, Is.EqualTo(1));
        Assert.That(response.Response, Has.Count.EqualTo(1));
        var item = response.Response[0];
        Assert.That(item.League.Name, Is.EqualTo("2. Bundesliga"));
        Assert.That(item.League.Id, Is.EqualTo(79));
        Assert.That(item.League.Type, Is.EqualTo(LeagueType.League));
        Assert.That(item.League.Country, Is.EqualTo("Germany"));
        Assert.That(item.League.Logo, Is.EqualTo("https://media-1.api-sports.io/football/leagues/79.png"));
        Assert.That(item.League.Flag, Is.EqualTo("https://media-1.api-sports.io/flags/de.svg"));
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
        Assert.That(standingItem.Team.Logo, Is.EqualTo("https://media-1.api-sports.io/football/teams/176.png"));
        Assert.That(standingItem.All.Played, Is.EqualTo(34));
        Assert.That(standingItem.All.Win, Is.EqualTo(21));
        Assert.That(standingItem.All.Draw, Is.EqualTo(4));
        Assert.That(standingItem.All.Lose, Is.EqualTo(9));
        Assert.That(standingItem.All.Goals.For, Is.EqualTo(66));
        Assert.That(standingItem.All.Goals.Against, Is.EqualTo(39));
        Assert.Multiple(() =>
        {
            Assert.That(standingItem.Home.Played, Is.EqualTo(17));
            Assert.That(standingItem.Home.Win, Is.EqualTo(12));
            Assert.That(standingItem.Home.Draw, Is.EqualTo(2));
            Assert.That(standingItem.Home.Lose, Is.EqualTo(3));
            Assert.That(standingItem.Home.Goals.For, Is.EqualTo(40));
            Assert.That(standingItem.Home.Goals.Against, Is.EqualTo(16));
            Assert.That(standingItem.Away.Played, Is.EqualTo(17));
            Assert.That(standingItem.Away.Win, Is.EqualTo(9));
            Assert.That(standingItem.Away.Draw, Is.EqualTo(2));
            Assert.That(standingItem.Away.Lose, Is.EqualTo(6));
            Assert.That(standingItem.Away.Goals.For, Is.EqualTo(26));
            Assert.That(standingItem.Away.Goals.Against, Is.EqualTo(23));
            Assert.That(standingItem.Update, Is.EqualTo(new DateTimeOffset(2021, 5, 26, 0, 0, 0, TimeSpan.Zero)));
        });
    }

    [Test]
    public async Task GetStandings_WhenError_ThrowsIfEnabled()
    {
        var factory = MockFactory(Route + "?season=2022&league=79", GetExpected(DefaultErrorResponse));
        var client = new StandingsClient(factory, MockOptions(o => o.ThrowOnError = true));
        var exception = await Assert.ThrowsAsync<ApiFootballException>(async () => await client.GetStandings(2022, 79));
        AssertDefaultException(exception);
    }
}