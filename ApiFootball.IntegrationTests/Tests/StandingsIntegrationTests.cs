namespace ApiFootball.IntegrationTests.Tests;

[TestFixture]
public class StandingsIntegrationTests : BaseIntegrationTest
{
    [Test]
    public async Task GetStandings_ShouldReturnStandings()
    {
        var standingsClient = ServiceProvider.GetRequiredService<IStandingsClient>();
        var response = await standingsClient.GetStandings(2022, 88);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Has.Count.EqualTo(response.Results));
        }

        var eredivisie = response.Response.FirstOrDefault();
        Assert.That(eredivisie, Is.Not.Null);

        using var scope = Assert.EnterMultipleScope();
        Assert.That(eredivisie.League.Name, Is.EqualTo("Eredivisie"));
        Assert.That(eredivisie.League.Country, Is.EqualTo("Netherlands"));
        Assert.That(eredivisie.League.Season, Is.EqualTo(2022));

        var standings = eredivisie.League.Standings;
        Assert.That(standings, Is.Not.Null);
        Assert.That(standings.Count, Is.EqualTo(1));
        Assert.That(standings.FirstOrDefault(), Is.Not.Null);
        Assert.That(standings.FirstOrDefault()?.Count, Is.EqualTo(18));

        var first = standings.FirstOrDefault()?.FirstOrDefault();
        Assert.That(first, Is.Not.Null);
        Assert.That(first.Rank, Is.EqualTo(1));
        Assert.That(first.Team.Id, Is.EqualTo(209));
        Assert.That(first.Team.Name, Is.EqualTo("Feyenoord"));
        Assert.That(first.Team.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/209.png"));
        Assert.That(first.Points, Is.EqualTo(82));
        Assert.That(first.GoalsDiff, Is.EqualTo(51));
        Assert.That(first.Group, Is.EqualTo("Eredivisie"));
        Assert.That(first.Form, Is.EqualTo("LWWWW"));
        Assert.That(first.Status, Is.EqualTo("same"));
        Assert.That(first.Description, Is.EqualTo("Promotion - Champions League (Group Stage: )"));
        Assert.That(first.All, Is.Not.Null);
        Assert.That(first.All.Played, Is.EqualTo(34));
        Assert.That(first.All.Win, Is.EqualTo(25));
        Assert.That(first.All.Draw, Is.EqualTo(7));
        Assert.That(first.All.Lose, Is.EqualTo(2));
        Assert.That(first.All.Goals.For, Is.EqualTo(81));
        Assert.That(first.All.Goals.Against, Is.EqualTo(30));
        Assert.That(first.Home, Is.Not.Null);
        Assert.That(first.Home.Played, Is.EqualTo(17));
        Assert.That(first.Home.Win, Is.EqualTo(12));
        Assert.That(first.Home.Draw, Is.EqualTo(4));
        Assert.That(first.Home.Lose, Is.EqualTo(1));
        Assert.That(first.Home.Goals.For, Is.EqualTo(37));
        Assert.That(first.Home.Goals.Against, Is.EqualTo(10));
        Assert.That(first.Away, Is.Not.Null);
        Assert.That(first.Away.Played, Is.EqualTo(17));
        Assert.That(first.Away.Win, Is.EqualTo(13));
        Assert.That(first.Away.Draw, Is.EqualTo(3));
        Assert.That(first.Away.Lose, Is.EqualTo(1));
        Assert.That(first.Away.Goals.For, Is.EqualTo(44));
        Assert.That(first.Away.Goals.Against, Is.EqualTo(20));
        Assert.That(first.Update, Is.EqualTo(new DateTimeOffset(2023, 6, 14, 0, 0, 0, TimeSpan.Zero)));
    }
}