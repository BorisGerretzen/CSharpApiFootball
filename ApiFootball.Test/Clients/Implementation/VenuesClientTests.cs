using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public class VenuesClientTests : BaseEndpointTest
{
    private const string Route = "venues";

    [Test]
    public async Task Test_GetVenues_Valid_Response()
    {
        var factory = MockFactory(Route, GetExpected(nameof(Test_GetVenues_Valid_Response)));
        var client = new VenuesClient(factory, MockOptions());
        var response = await client.GetVenues(country: "Netherlands");

        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo(Route));
            Assert.That(response.Errors, Has.Count.EqualTo(0));
            Assert.That(response.Parameters, Has.Count.EqualTo(1));
            Assert.That(response.Results, Is.EqualTo(210));
            Assert.That(response.Response, Has.Count.EqualTo(210));
        });

        var grolschVeste = response.Response.FirstOrDefault(v => v.Id == 1153);

        Assert.That(grolschVeste, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(grolschVeste.Id, Is.EqualTo(1153));
            Assert.That(grolschVeste.Name, Is.EqualTo("De Grolsch Veste"));
            Assert.That(grolschVeste.Address, Is.EqualTo("Colosseum 65"));
            Assert.That(grolschVeste.City, Is.EqualTo("Enschede"));
            Assert.That(grolschVeste.Country, Is.EqualTo("Netherlands"));
            Assert.That(grolschVeste.Capacity, Is.EqualTo(30205));
            Assert.That(grolschVeste.Surface, Is.EqualTo("grass"));
            Assert.That(grolschVeste.Image, Is.EqualTo("https://media.api-sports.io/football/venues/1153.png"));
        });
    }

    [Test]
    public void GetVenues_WhenError_ThrowsIfEnabled()
    {
        var factory = MockFactory(Route, GetExpected(DefaultErrorResponse));
        var client = new VenuesClient(factory, MockOptions(o => o.ThrowOnError = true));

        var exception = Assert.ThrowsAsync<ApiFootballException>(async () => await client.GetVenues(country: "Netherlands"));
        AssertDefaultException(exception);
    }
}