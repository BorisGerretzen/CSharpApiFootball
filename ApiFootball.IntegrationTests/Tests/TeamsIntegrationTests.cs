namespace ApiFootball.IntegrationTests.Tests;

public class TeamsIntegrationTests : BaseIntegrationTest
{
    [Test]
    public async Task GetTeamsInformation_ShouldReturnTeamInformation()
    {
        var teamsClient = ServiceProvider.GetRequiredService<ITeamsClient>();
        var response = await teamsClient.GetTeamsInformation(69);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Has.Count.EqualTo(response.Results));
        }

        var derby = response.Response.FirstOrDefault();
        Assert.That(derby, Is.Not.Null);

        using var scope = Assert.EnterMultipleScope();
        Assert.That(derby.Team.Name, Is.EqualTo("Derby"));
        Assert.That(derby.Team.Country, Is.EqualTo("England"));
        Assert.That(derby.Team.Founded, Is.EqualTo(1884));
        Assert.That(derby.Team.National, Is.False);
        Assert.That(derby.Team.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/69.png"));
        Assert.That(derby.Venue.Name, Is.EqualTo("Pride Park Stadium"));
        Assert.That(derby.Venue.Address, Is.EqualTo("Pride Park, Royal Way"));
        Assert.That(derby.Venue.City, Is.EqualTo("Derby"));
        Assert.That(derby.Venue.Capacity, Is.GreaterThan(0));
        Assert.That(derby.Venue.Surface, Is.EqualTo("grass"));
        Assert.That(derby.Venue.Image, Is.EqualTo("https://media.api-sports.io/football/venues/527.png"));
    }

    [Test]
    public async Task GetTeamsSeasons_ShouldReturnSeasons()
    {
        var teamsClient = ServiceProvider.GetRequiredService<ITeamsClient>();
        var response = await teamsClient.GetTeamsSeasons(69);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Has.Count.EqualTo(response.Results));
        }

        var seasons = response.Response;
        Assert.That(seasons, Is.Not.Null);
        Assert.That(seasons.Count, Is.GreaterThanOrEqualTo(14));
        Assert.That(seasons.FirstOrDefault(), Is.EqualTo(2011));
    }

    [Test]
    public async Task GetTeamsCountries_ShouldReturnCountries()
    {
        var teamsClient = ServiceProvider.GetRequiredService<ITeamsClient>();
        var response = await teamsClient.GetTeamsCountries();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Has.Count.EqualTo(response.Results));
        }

        var countries = response.Response;
        Assert.That(countries, Is.Not.Null);
        Assert.That(countries, Is.Not.Empty);
        Assert.That(countries.FirstOrDefault(), Is.Not.Null);
        Assert.That(countries.FirstOrDefault()?.Name, Is.EqualTo("Afghanistan"));
    }
}