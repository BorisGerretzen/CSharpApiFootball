using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public sealed class InjuriesClientTests : BaseEndpointTest
{
    [Test]
    public async Task GetInjuries_DeserializesCapturedApiResponse()
    {
        var route = "injuries?league=39&season=2026&team=40";
        var factory = MockFactory(route, GetExpected("Test_Injuries_EPL_Real_Response"));
        var response = await new InjuriesClient(factory, MockOptions()).GetInjuries(39, 2026, team: 40);
        CapturedApiResponseAssertions.AssertPage(response, "injuries");
        CapturedApiResponseAssertions.AssertParameter(response, "league", "39", 3);
        CapturedApiResponseAssertions.AssertParameter(response, "season", "2026", 3);
        CapturedApiResponseAssertions.AssertParameter(response, "team", "40", 3);
        Assert.That(response.Response, Has.Count.EqualTo(62));
        var injury = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(injury.Player?.Id, Is.EqualTo(180317));
            Assert.That(injury.Player?.Name, Is.EqualTo("C. Bradley"));
            Assert.That(injury.Player?.Photo, Is.EqualTo("https://media.api-sports.io/football/players/180317.png"));
            Assert.That(injury.Player?.Type, Is.EqualTo("Missing Fixture"));
            Assert.That(injury.Player?.Reason, Is.EqualTo("Knee Injury"));
            Assert.That(injury.Team?.Id, Is.EqualTo(40));
            Assert.That(injury.Team?.Name, Is.EqualTo("Liverpool"));
            Assert.That(injury.Team?.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/40.png"));
            Assert.That(injury.Fixture?.Id, Is.EqualTo(1557375));
            Assert.That(injury.Fixture?.Timezone, Is.EqualTo("UTC"));
            Assert.That(injury.Fixture?.Date, Is.EqualTo(new DateTimeOffset(2026, 8, 23, 15, 30, 0, TimeSpan.Zero)));
            Assert.That(injury.Fixture?.Timestamp, Is.EqualTo(1787499000));
            Assert.That(injury.League?.Id, Is.EqualTo(39));
            Assert.That(injury.League?.Season, Is.EqualTo(2026));
            Assert.That(injury.League?.Name, Is.EqualTo("Premier League"));
            Assert.That(injury.League?.Country, Is.EqualTo("England"));
            Assert.That(injury.League?.Logo, Is.EqualTo("https://media.api-sports.io/football/leagues/39.png"));
            Assert.That(injury.League?.Flag, Is.EqualTo("https://media.api-sports.io/flags/gb-eng.svg"));
        });
    }
}