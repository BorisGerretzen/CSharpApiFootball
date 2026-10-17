using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public class VenuesClientTests : BaseEndpointTest
{
    private const string Route = "venues";

    [Test]
    public async Task Test_GetVenues_Valid_Response()
    {
        var factory = MockFactory(Route + "?country=Netherlands", GetExpected(nameof(Test_GetVenues_Valid_Response)));
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
    public async Task GetVenues_WhenError_ThrowsIfEnabled()
    {
        var factory = MockFactory(Route + "?country=Netherlands", GetExpected(DefaultErrorResponse));
        var client = new VenuesClient(factory, MockOptions(o => o.ThrowOnError = true));
        var exception =
            await Assert.ThrowsAsync<ApiFootballException>(async () => await client.GetVenues(country: "Netherlands"));
        AssertDefaultException(exception);
    }

    [Test]
    public async Task GetVenues_WithSearch_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("venues?search=Twente", GetExpected("Test_Venues_Twente_Real_Response"));
        var response = await new VenuesClient(factory, MockOptions()).GetVenues(search: "Twente");
        CapturedApiResponseAssertions.AssertPage(response, "venues");
        CapturedApiResponseAssertions.AssertParameter(response, "search", "Twente");
        Assert.That(response.Response, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(response.Response[0].Id, Is.EqualTo(12543));
            Assert.That(response.Response[0].Name, Is.EqualTo("FC Twente-trainingscentrum TC veld 1"));
            Assert.That(response.Response[0].Address, Is.EqualTo("Kuipersdijk 40"));
            Assert.That(response.Response[0].City, Is.EqualTo("Hengelo"));
            Assert.That(response.Response[0].Country, Is.EqualTo("Netherlands"));
            Assert.That(response.Response[0].Capacity, Is.EqualTo(1000));
            Assert.That(response.Response[0].Surface, Is.EqualTo("grass"));
            Assert.That(response.Response[0].Image,
                Is.EqualTo("https://media.api-sports.io/football/venues/12543.png"));
        });
    }
}