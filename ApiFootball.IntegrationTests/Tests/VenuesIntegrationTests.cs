namespace ApiFootball.IntegrationTests.Tests;

public class VenuesIntegrationTests : BaseIntegrationTest
{
    [Test]
    public async Task GetVenues_ShouldReturnVenues()
    {
        var venuesClient = ServiceProvider.GetRequiredService<IVenuesClient>();
        var response = await venuesClient.GetVenues(country: "Netherlands");
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Has.Count.EqualTo(response.Results));
        }

        var venues = response.Response;
        Assert.That(venues, Is.Not.Null);
        Assert.That(venues, Is.Not.Empty);

        var grolschVeste = response.Response.FirstOrDefault(v => v.Id == 1153);
        Assert.That(grolschVeste, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(grolschVeste.Id, Is.EqualTo(1153));
            Assert.That(grolschVeste.Name, Is.EqualTo("De Grolsch Veste"));
            Assert.That(grolschVeste.Address, Is.EqualTo("Colosseum 65"));
            Assert.That(grolschVeste.City, Is.EqualTo("Enschede"));
            Assert.That(grolschVeste.Country, Is.EqualTo("Netherlands"));
            Assert.That(grolschVeste.Capacity, Is.GreaterThan(0));
            Assert.That(grolschVeste.Surface, Is.EqualTo("grass"));
            Assert.That(grolschVeste.Image, Is.EqualTo("https://media.api-sports.io/football/venues/1153.png"));
        });
    }
}