namespace ApiFootball.IntegrationTests.Tests;

[TestFixture]
public sealed class OddsLiveTests : BaseLiveTest
{
    [Test]
    public async Task GetBookmakers_ShouldReturnLookupData()
    {
        var oddsClient = ServiceProvider.GetRequiredService<IOddsClient>();
        var response = await oddsClient.GetBookmakers();

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Errors, Is.Empty);
        Assert.That(response.Results, Is.GreaterThan(0));
        Assert.That(response.Response, Has.Count.EqualTo(response.Results));
        Assert.That(response.Response, Is.Not.Empty);
    }
}