using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public sealed class TransfersClientTests : BaseEndpointTest
{
    [Test]
    public async Task GetTransfers_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("transfers?player=276", GetExpected("Test_Transfers_Player_Real_Response"));
        var response = await new TransfersClient(factory, MockOptions()).GetTransfers(276);
        CapturedApiResponseAssertions.AssertPage(response, "transfers");
        CapturedApiResponseAssertions.AssertParameter(response, "player", "276");
        Assert.That(response.Response, Has.Count.EqualTo(1));
        var history = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(history.Player?.Id, Is.EqualTo(276));
            Assert.That(history.Player?.Name, Is.EqualTo("Neymar"));
            Assert.That(history.Update, Is.EqualTo(new DateTimeOffset(2026, 2, 9, 0, 43, 31, TimeSpan.Zero)));
            Assert.That(history.Transfers, Has.Count.EqualTo(5));
        });
        var transfer = history.Transfers![0];
        Assert.Multiple(() =>
        {
            Assert.That(transfer.Date, Is.EqualTo(new DateOnly(2023, 8, 15)));
            Assert.That(transfer.Type, Is.EqualTo("€ 80M"));
            Assert.That(transfer.Teams?.In?.Id, Is.EqualTo(2932));
            Assert.That(transfer.Teams?.In?.Name, Is.EqualTo("Al-Hilal Saudi FC"));
            Assert.That(transfer.Teams?.In?.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/2932.png"));
            Assert.That(transfer.Teams?.Out?.Id, Is.EqualTo(85));
            Assert.That(transfer.Teams?.Out?.Name, Is.EqualTo("Paris Saint Germain"));
            Assert.That(transfer.Teams?.Out?.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/85.png"));
            Assert.That(history.Transfers[1].Date, Is.EqualTo(new DateOnly(2017, 8, 3)));
            Assert.That(history.Transfers[1].Type, Is.EqualTo("€ 222M"));
            Assert.That(history.Transfers[1].Teams?.In?.Name, Is.EqualTo("Paris Saint Germain"));
            Assert.That(history.Transfers[1].Teams?.Out?.Name, Is.EqualTo("Barcelona"));
        });
    }
}