using ApiFootball.Models;

namespace ApiFootball.IntegrationTests.Tests;

[TestFixture]
public class LeaguesIntegrationTests : BaseIntegrationTest
{
    [Test]
    public async Task GetLeagues_ShouldReturnLeagues()
    {
        var leaguesClient = ServiceProvider.GetRequiredService<ILeaguesClient>();
        var response = await leaguesClient.GetLeagues(country: "Netherlands");
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Has.Count.EqualTo(response.Results));
        }

        var eredivisie = response.Response.FirstOrDefault(l => l.League.Id == 88);
        Assert.That(eredivisie, Is.Not.Null);

        using var scope = Assert.EnterMultipleScope();
        Assert.That(eredivisie.League.Name, Is.EqualTo("Eredivisie"));
        Assert.That(eredivisie.League.Type, Is.EqualTo(LeagueType.League));
        Assert.That(eredivisie.League.Logo, Is.EqualTo("https://media.api-sports.io/football/leagues/88.png"));

        Assert.That(eredivisie.Country.Name, Is.EqualTo("Netherlands"));
        Assert.That(eredivisie.Country.Code, Is.EqualTo("NL"));
        Assert.That(eredivisie.Country.Flag, Is.EqualTo("https://media.api-sports.io/flags/nl.svg"));

        Assert.That(eredivisie.Seasons, Is.Not.Null);
        Assert.That(eredivisie.Seasons.Count, Is.GreaterThan(0));

        var season2023 = eredivisie.Seasons.FirstOrDefault(s => s.Year == 2023);
        Assert.That(season2023, Is.Not.Null);
        Assert.That(season2023.Start, Is.EqualTo(new DateOnly(2023, 8, 11)));
        Assert.That(season2023.End, Is.EqualTo(new DateOnly(2024, 6, 2)));
        Assert.That(season2023.Current, Is.False);

        Assert.That(season2023.Coverage.Fixtures.Events, Is.True);
        Assert.That(season2023.Coverage.Fixtures.Lineups, Is.True);
        Assert.That(season2023.Coverage.Fixtures.StatisticsFixtures, Is.True);
        Assert.That(season2023.Coverage.Fixtures.StatisticsPlayers, Is.True);
        Assert.That(season2023.Coverage.Standings, Is.True);
        Assert.That(season2023.Coverage.Players, Is.True);
        Assert.That(season2023.Coverage.TopScorers, Is.True);
        Assert.That(season2023.Coverage.TopAssists, Is.True);
        Assert.That(season2023.Coverage.TopCards, Is.True);
        Assert.That(season2023.Coverage.Injuries, Is.True);
        Assert.That(season2023.Coverage.Predictions, Is.True);
        Assert.That(season2023.Coverage.Odds, Is.False);
    }
}