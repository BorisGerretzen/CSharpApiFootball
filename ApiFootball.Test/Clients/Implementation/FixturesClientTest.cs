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
        var client = new FixturesClient(factory);
        var response = await client.GetRounds(88, 2022);
        Assert.That(response.Get, Is.EqualTo(Route + path));
        Assert.That(response.Errors, Has.Count.EqualTo(0));
        Assert.That(response.Parameters, Has.Count.EqualTo(2));
        Assert.That(response.Parameters["league"], Is.EqualTo("88"));
        Assert.That(response.Parameters["season"], Is.EqualTo("2022"));
        Assert.That(response.Results, Is.EqualTo(10));
        Assert.That(response.Response, Has.Count.EqualTo(10));
        foreach (var responseItem in response.Response) Assert.That(responseItem, Is.Not.Null.Or.Empty);
    }

    [Test]
    public async Task Test_Fixtures_Valid_Response()
    {
        const string query = "?league=88&season=2022";
        var factory = MockFactory(Route + query, GetExpected(nameof(Test_Fixtures_Valid_Response)));
        var client = new FixturesClient(factory);
        var response = await client.GetFixtures(league: 88, season: 2022);
        Assert.That(response.Get, Is.EqualTo(Route));
        Assert.That(response.Errors, Has.Count.EqualTo(0));
        Assert.That(response.Parameters, Has.Count.EqualTo(2));
        Assert.That(response.Parameters["league"], Is.EqualTo("88"));
        Assert.That(response.Parameters["season"], Is.EqualTo("2022"));
        Assert.That(response.Results, Is.EqualTo(2));
        Assert.That(response.Response, Has.Count.EqualTo(2));

        var item = response.Response[0];
        Assert.That(item.Fixture.Date, Is.EqualTo("2022-08-05T18:00:00+00:00"));
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
        Assert.That(item.Fixture.Date, Is.EqualTo("2023-05-28T12:30:00+00:00"));
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
}