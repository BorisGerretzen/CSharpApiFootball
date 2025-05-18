namespace ApiFootball.IntegrationTests.Tests;

[TestFixture]
public class FixturesIntegrationTests : BaseIntegrationTest
{
    [Test]
    public async Task GetFixtures_ShouldReturnFixtures()
    {
        var fixturesClient = ServiceProvider.GetRequiredService<IFixturesClient>();
        var response = await fixturesClient.GetFixtures(league: 88, season: 2024);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Has.Count.EqualTo(response.Results));
        }

        var twenteAjax = response.Response.FirstOrDefault(f => f.Fixture.Id == 1213546);
        Assert.That(twenteAjax, Is.Not.Null);

        using var scope = Assert.EnterMultipleScope();
        Assert.That(twenteAjax.Fixture.Id, Is.EqualTo(1213546));
        Assert.That(twenteAjax.Fixture.Referee, Is.EqualTo("A. Lindhout"));
        Assert.That(twenteAjax.Fixture.Timezone, Is.EqualTo("UTC"));
        Assert.That(twenteAjax.Fixture.Status.Short, Is.EqualTo("FT"));
        Assert.That(twenteAjax.Fixture.Status.Elapsed, Is.EqualTo(90));

        Assert.That(twenteAjax.League.Id, Is.EqualTo(88));
        Assert.That(twenteAjax.League.Name, Is.EqualTo("Eredivisie"));
        Assert.That(twenteAjax.League.Country, Is.EqualTo("Netherlands"));
        Assert.That(twenteAjax.League.Season, Is.EqualTo(2024));

        Assert.That(twenteAjax.Teams.Home.Id, Is.EqualTo(415));
        Assert.That(twenteAjax.Teams.Home.Name, Is.EqualTo("Twente"));
        Assert.That(twenteAjax.Teams.Away.Id, Is.EqualTo(194));
        Assert.That(twenteAjax.Teams.Away.Name, Is.EqualTo("Ajax"));

        Assert.That(twenteAjax.Goals.Home, Is.EqualTo(2));
        Assert.That(twenteAjax.Goals.Away, Is.EqualTo(2));
        Assert.That(twenteAjax.Score.Fulltime.Home, Is.EqualTo(2));
        Assert.That(twenteAjax.Score.Fulltime.Away, Is.EqualTo(2));
        Assert.That(twenteAjax.Score.Halftime.Home, Is.EqualTo(1));
        Assert.That(twenteAjax.Score.Halftime.Away, Is.EqualTo(0));
    }

    [Test]
    public async Task GetRounds_ShouldReturnRounds()
    {
        var fixturesClient = ServiceProvider.GetRequiredService<IFixturesClient>();
        var response = await fixturesClient.GetRounds(88, 2022);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Has.Count.EqualTo(response.Results));
        }

        using var scope = Assert.EnterMultipleScope();
        Assert.That(response.Get, Is.EqualTo("fixtures/rounds"));
        Assert.That(response.Parameters, Contains.Key("league").WithValue("88"));
        Assert.That(response.Parameters, Contains.Key("season").WithValue("2022"));
        Assert.That(response.Results, Is.EqualTo(37));

        Assert.That(response.Response, Contains.Item("Regular Season - 1"));
        Assert.That(response.Response, Contains.Item("Regular Season - 34"));
    }
}