using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public class TeamsClientTests : BaseEndpointTest
{
    private const string Route = "teams";

    [Test]
    public async Task Test_TeamsInformation_Valid_Response()
    {
        var factory = MockFactory(Route, GetExpected(nameof(Test_TeamsInformation_Valid_Response)));
        var client = new TeamsClient(factory);
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
    public async Task Test_TeamsSeasons_Valid_Response()
    {
        const string route = "teams/seasons";
        var factory = MockFactory(route, GetExpected(nameof(Test_TeamsSeasons_Valid_Response)));
        var client = new TeamsClient(factory);
        var response = await client.GetTeamsSeasons(69);

        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo(route));
            Assert.That(response.Errors, Has.Count.EqualTo(0));
            Assert.That(response.Parameters, Has.Count.EqualTo(1));
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Is.Not.Empty);
        });

        Assert.That(response.Response, Has.All.GreaterThan(2000));
        Assert.That(response.Response, Has.Count.EqualTo(14));
    }

    [Test]
    public async Task Test_TeamsCountries_Valid_Response()
    {
        const string route = "teams/countries";
        var factory = MockFactory(route, GetExpected(nameof(Test_TeamsCountries_Valid_Response)));
        var client = new TeamsClient(factory);
        var response = await client.GetTeamsCountries();

        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo(route));
            Assert.That(response.Errors, Has.Count.EqualTo(0));
            Assert.That(response.Parameters, Has.Count.EqualTo(0));
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Is.Not.Empty);
        });

        Assert.That(response.Response.Any(c => c is { Name: "Netherlands", Code: "NL" }));
        Assert.That(response.Response.Any(c => c is { Name: "England", Code: "GB-ENG" }));

        var netherlands = response.Response.FirstOrDefault(c => c.Name == "Netherlands");
        Assert.That(netherlands?.Flag, Is.EqualTo("https://media.api-sports.io/flags/nl.svg"));
    }
}