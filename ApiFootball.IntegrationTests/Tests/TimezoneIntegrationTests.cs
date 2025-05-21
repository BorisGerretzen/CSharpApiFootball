namespace ApiFootball.IntegrationTests.Tests;

public class TimezoneIntegrationTests : BaseIntegrationTest
{
    [Test]
    public async Task GetTimezones_ShouldReturnTimezones()
    {
        var timezoneClient = ServiceProvider.GetRequiredService<ITimezoneClient>();
        var response = await timezoneClient.GetTimezones();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Has.Count.EqualTo(response.Results));
        }

        var timezones = response.Response;
        Assert.That(timezones, Is.Not.Null);
        Assert.That(timezones, Has.Count.GreaterThanOrEqualTo(427));
        Assert.That(timezones.FirstOrDefault(), Is.Not.Null.Or.Empty);
    }
}