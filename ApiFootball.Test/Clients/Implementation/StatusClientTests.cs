using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public sealed class StatusClientTests : BaseEndpointTest
{
    [Test]
    public async Task GetStatus_DeserializesCapturedApiResponseWithoutAccountIdentity()
    {
        var factory = MockFactory("status", GetExpected("Test_Status_Real_Response"));
        var response = await new StatusClient(factory, MockOptions()).GetStatus();
        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo("status"));
            Assert.That(response.Errors, Is.Empty);
            Assert.That(response.Parameters, Is.Empty);
            Assert.That(response.Results, Is.EqualTo(0));
            Assert.That(response.Response.Subscription?.Plan, Is.EqualTo("Free"));
            Assert.That(response.Response.Subscription?.End,
                Is.EqualTo(new DateTimeOffset(2026, 10, 21, 9, 0, 3, TimeSpan.Zero)));
            Assert.That(response.Response.Subscription?.Active, Is.True);
            Assert.That(response.Response.Requests?.Current, Is.EqualTo(36));
            Assert.That(response.Response.Requests?.LimitDay, Is.EqualTo(100));
            Assert.That(response.Response.Account?.Firstname, Is.Null);
            Assert.That(response.Response.Account?.Lastname, Is.Null);
            Assert.That(response.Response.Account?.Email, Is.Null);
        });
    }
}