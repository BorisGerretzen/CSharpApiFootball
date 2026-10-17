using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public sealed class SidelinedClientTests : BaseEndpointTest
{
    [Test]
    public async Task GetSidelined_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("sidelined?player=276", GetExpected("Test_Sidelined_Player_Real_Response"));
        var response = await new SidelinedClient(factory, MockOptions()).GetSidelined(276);
        CapturedApiResponseAssertions.AssertPage(response, "sidelined");
        CapturedApiResponseAssertions.AssertParameter(response, "player", "276");
        Assert.That(response.Response, Has.Count.EqualTo(32));
        Assert.Multiple(() =>
        {
            Assert.That(response.Response[0].Type, Is.EqualTo("Thigh Injury"));
            Assert.That(response.Response[0].Start, Is.EqualTo(new DateOnly(2025, 9, 19)));
            Assert.That(response.Response[0].End, Is.Null);
            Assert.That(response.Response[1].Type, Is.EqualTo("Thigh Injury"));
            Assert.That(response.Response[1].Start, Is.EqualTo(new DateOnly(2025, 4, 18)));
            Assert.That(response.Response[1].End, Is.EqualTo(new DateOnly(2025, 5, 22)));
        });
    }
}