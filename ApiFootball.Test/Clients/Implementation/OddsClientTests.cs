using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public sealed class OddsClientTests : BaseEndpointTest
{
    [Test]
    public async Task GetBookmakers_DeserializesCapturedLiveResponse()
    {
        var factory = MockFactory("odds/bookmakers", GetExpected("Test_OddsBookmakers_Live_Response"));
        var client = new OddsClient(factory, MockOptions());

        var response = await client.GetBookmakers();

        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo("odds/bookmakers"));
            Assert.That(response.Errors, Is.Empty);
            Assert.That(response.Response, Has.Count.EqualTo(response.Results));
            Assert.That(response.Response, Is.Not.Empty);
            Assert.That(response.Response.All(bookmaker => bookmaker.Id > 0), Is.True);
            Assert.That(response.Response.Any(bookmaker => !string.IsNullOrWhiteSpace(bookmaker.Name)), Is.True);
        });
    }
}
