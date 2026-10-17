using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public sealed class TrophiesClientTests : BaseEndpointTest
{
    [Test]
    public async Task GetTrophies_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("trophies?player=276", GetExpected("Test_Trophies_Player_Real_Response"));
        var response = await new TrophiesClient(factory, MockOptions()).GetTrophies(276);
        CapturedApiResponseAssertions.AssertPage(response, "trophies");
        CapturedApiResponseAssertions.AssertParameter(response, "player", "276");
        Assert.That(response.Response, Has.Count.EqualTo(80));
        Assert.Multiple(() =>
        {
            Assert.That(response.Response[0].League, Is.EqualTo("Saudi League"));
            Assert.That(response.Response[0].Country, Is.EqualTo("Saudi Arabia"));
            Assert.That(response.Response[0].Season, Is.EqualTo("2023/2024"));
            Assert.That(response.Response[0].Place, Is.EqualTo("Winner"));
            Assert.That(response.Response[1].League, Is.EqualTo("King's Cup"));
            Assert.That(response.Response[1].Country, Is.EqualTo("Saudi Arabia"));
            Assert.That(response.Response[1].Season, Is.EqualTo("2023/2024"));
            Assert.That(response.Response[1].Place, Is.EqualTo("Winner"));
        });
    }
}