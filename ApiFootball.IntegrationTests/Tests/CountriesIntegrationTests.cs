namespace ApiFootball.IntegrationTests.Tests;

[TestFixture]
public class CountriesIntegrationTests : BaseIntegrationTest
{
    [Test]
    public async Task GetCountries_ShouldReturnCountries()
    {
        var countriesClient = ServiceProvider.GetRequiredService<ICountriesClient>();
        var response = await countriesClient.GetCountries();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Results, Is.GreaterThan(0));
            Assert.That(response.Response, Has.Count.EqualTo(response.Results));
        }

        var netherlands = response.Response.FirstOrDefault(c => c.Code == "NL");
        Assert.That(netherlands, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(netherlands.Name, Is.EqualTo("Netherlands"));
            Assert.That(netherlands.Flag, Is.Not.Empty);
        }
    }
}