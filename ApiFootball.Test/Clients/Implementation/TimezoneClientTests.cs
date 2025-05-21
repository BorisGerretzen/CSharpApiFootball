using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public class TimezoneClientTests : BaseEndpointTest
{
    private const string Route = "timezone";

    [Test]
    public async Task Test_Timezones_Valid_Response()
    {
        var factory = MockFactory(Route, GetExpected(nameof(Test_Timezones_Valid_Response)));
        var client = new TimezoneClient(factory, MockOptions());
        var response = await client.GetTimezones();
        Assert.That(response.Get, Is.EqualTo(Route));
        Assert.That(response.Errors, Has.Count.EqualTo(0));
        Assert.That(response.Parameters, Has.Count.EqualTo(0));
        Assert.That(response.Results, Is.EqualTo(4));
        Assert.That(response.Response, Has.Count.EqualTo(4));
    }

    [Test]
    public void GetTimezones_WhenError_ThrowsIfEnabled()
    {
        var factory = MockFactory(Route, GetExpected(DefaultErrorResponse));
        var client = new TimezoneClient(factory, MockOptions(o => o.ThrowOnError = true));

        var exception = Assert.ThrowsAsync<ApiFootballException>(async () => await client.GetTimezones());
        AssertDefaultException(exception);
    }
}