using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public sealed class CoachesClientTests : BaseEndpointTest
{
    [Test]
    public async Task GetCoaches_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("coachs?team=40", GetExpected("Test_Coachs_Team40_Real_Response"));
        var response = await new CoachesClient(factory, MockOptions()).GetCoaches(team: 40);
        CapturedApiResponseAssertions.AssertPage(response, "coachs");
        CapturedApiResponseAssertions.AssertParameter(response, "team", "40");
        Assert.That(response.Response, Has.Count.EqualTo(4));
        var coach = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(coach.Id, Is.EqualTo(1));
            Assert.That(coach.Name, Is.EqualTo("J. Klopp"));
            Assert.That(coach.Firstname, Is.EqualTo("Jürgen Norbert"));
            Assert.That(coach.Lastname, Is.EqualTo("Klopp"));
            Assert.That(coach.Age, Is.EqualTo(58));
            Assert.That(coach.Birth?.Date, Is.EqualTo(new DateOnly(1967, 6, 16)));
            Assert.That(coach.Birth?.Place, Is.EqualTo("Stuttgart"));
            Assert.That(coach.Birth?.Country, Is.EqualTo("Germany"));
            Assert.That(coach.Nationality, Is.EqualTo("Germany"));
            Assert.That(coach.Height, Is.EqualTo("191 cm"));
            Assert.That(coach.Weight, Is.EqualTo("83 kg"));
            Assert.That(coach.Photo, Is.EqualTo("https://media.api-sports.io/football/coachs/1.png"));
            Assert.That(coach.Team?.Id, Is.EqualTo(40));
            Assert.That(coach.Team?.Name, Is.EqualTo("Liverpool"));
            Assert.That(coach.Team?.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/40.png"));
            Assert.That(coach.Career, Has.Count.EqualTo(3));
            Assert.That(coach.Career![0].Team?.Id, Is.EqualTo(40));
            Assert.That(coach.Career[0].Team?.Name, Is.EqualTo("Liverpool"));
            Assert.That(coach.Career[0].Team?.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/40.png"));
            Assert.That(coach.Career[0].Start, Is.EqualTo(new DateOnly(2015, 10, 1)));
            Assert.That(coach.Career[0].End, Is.EqualTo(new DateOnly(2024, 5, 1)));
            Assert.That(coach.Career[1].Team?.Id, Is.EqualTo(165));
            Assert.That(coach.Career[1].Team?.Name, Is.EqualTo("Borussia Dortmund"));
            Assert.That(coach.Career[1].Start, Is.EqualTo(new DateOnly(2008, 7, 1)));
            Assert.That(coach.Career[1].End, Is.EqualTo(new DateOnly(2015, 7, 1)));
            Assert.That(coach.Career[2].Team?.Id, Is.EqualTo(164));
            Assert.That(coach.Career[2].Team?.Name, Is.EqualTo("Mainz"));
            Assert.That(coach.Career[2].Start, Is.EqualTo(new DateOnly(2001, 2, 1)));
            Assert.That(coach.Career[2].End, Is.EqualTo(new DateOnly(2008, 6, 1)));
        });
    }
}