using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public class CountriesClientTests : BaseEndpointTest
{
    private const string Route = "countries";

    [Test]
    public async Task Test_Countries_Valid_Response()
    {
        var factory = MockFactory(Route, GetExpected(nameof(Test_Countries_Valid_Response)));

        var client = new CountriesClient(factory);
        var response = await client.GetCountries();

        Assert.That(response.Get, Is.EqualTo(Route));
        Assert.That(response.Errors, Has.Count.EqualTo(0));
        Assert.That(response.Parameters, Has.Count.EqualTo(0));
        Assert.That(response.Results, Is.EqualTo(2));
        Assert.That(response.Response, Has.Count.EqualTo(2));

        foreach (var responseItem in response.Response)
        {
            Assert.That(responseItem.Name, Is.Not.Null.Or.Empty);
            Assert.That(responseItem.Code, Is.Not.Null.Or.Empty);
            Assert.That(responseItem.Flag, Is.Not.Null.Or.Empty);
            Assert.That(responseItem.Code, Has.Length.EqualTo(2));
        }
    }

    [Test]
    public async Task Test_Invalid_Search_Param()
    {
        const string query = "?search=ab";
        var factory = MockFactory(Route + query, GetExpected(nameof(Test_Invalid_Search_Param)));
        var client = new CountriesClient(factory);
        var response = await client.GetCountries(search: "ab");
        Assert.That(response.Get, Is.EqualTo(Route));
        Assert.That(response.Errors, Has.Count.EqualTo(1));
        Assert.That(response.Errors["search"], Is.EqualTo("The Search field must be at least 3 characters in length."));
        Assert.That(response.Parameters, Has.Count.EqualTo(1));
        Assert.That(response.Parameters["search"], Is.EqualTo("ab"));
        Assert.That(response.Results, Is.EqualTo(0));
        Assert.That(response.Response, Has.Count.EqualTo(0));
    }
}