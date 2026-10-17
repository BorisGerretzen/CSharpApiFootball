using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

[Parallelizable(ParallelScope.All)]
public class CountriesClientTests : BaseEndpointTest
{
    private const string Route = "countries";

    [Test]
    public async Task Test_Countries_Valid_Response()
    {
        var factory = MockFactory(Route, GetExpected(nameof(Test_Countries_Valid_Response)));
        var client = new CountriesClient(factory, MockOptions());
        var response = await client.GetCountries();
        Assert.That(response.Get, Is.EqualTo(Route));
        Assert.That(response.Errors, Has.Count.EqualTo(0));
        Assert.That(response.Parameters, Has.Count.EqualTo(0));
        Assert.That(response.Results, Is.EqualTo(2));
        Assert.That(response.Response, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(response.Response[0].Name, Is.EqualTo("Albania"));
            Assert.That(response.Response[0].Code, Is.EqualTo("AL"));
            Assert.That(response.Response[0].Flag, Is.EqualTo("https://media-6.api-sports.io/flags/al.svg"));
            Assert.That(response.Response[1].Name, Is.EqualTo("Algeria"));
            Assert.That(response.Response[1].Code, Is.EqualTo("DZ"));
            Assert.That(response.Response[1].Flag, Is.EqualTo("https://media-4.api-sports.io/flags/dz.svg"));
        });
    }

    [Test]
    public async Task Test_Invalid_Search_Param()
    {
        const string query = "?search=ab";
        var factory = MockFactory(Route + query, GetExpected(nameof(Test_Invalid_Search_Param)));
        var client = new CountriesClient(factory, MockOptions());
        var response = await client.GetCountries(search: "ab");
        Assert.That(response.Get, Is.EqualTo(Route));
        Assert.That(response.Errors, Has.Count.EqualTo(1));
        Assert.That(response.Errors["search"], Is.EqualTo("The Search field must be at least 3 characters in length."));
        Assert.That(response.Parameters, Has.Count.EqualTo(1));
        Assert.That(response.Parameters["search"], Is.EqualTo("ab"));
        Assert.That(response.Results, Is.EqualTo(0));
        Assert.That(response.Response, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task GetCountries_WhenError_ThrowsIfEnabled()
    {
        const string query = "?search=ab";
        var factory = MockFactory(Route + query, GetExpected(DefaultErrorResponse));
        var client = new CountriesClient(factory, MockOptions(o => o.ThrowOnError = true));
        var exception =
            await Assert.ThrowsAsync<ApiFootballException>(async () => await client.GetCountries(search: "ab"));
        AssertDefaultException(exception);
    }

    [Test]
    public async Task GetCountries_WithName_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("countries?name=Netherlands",
            GetExpected("Test_Countries_Netherlands_Real_Response"));
        var response = await new CountriesClient(factory, MockOptions()).GetCountries("Netherlands");
        CapturedApiResponseAssertions.AssertPage(response, "countries");
        CapturedApiResponseAssertions.AssertParameter(response, "name", "Netherlands");
        Assert.That(response.Response, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(response.Response[0].Name, Is.EqualTo("Netherlands"));
            Assert.That(response.Response[0].Code, Is.EqualTo("NL"));
            Assert.That(response.Response[0].Flag, Is.EqualTo("https://media.api-sports.io/flags/nl.svg"));
        });
    }
}