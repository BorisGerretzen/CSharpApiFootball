using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public sealed class PredictionsClientTests : BaseEndpointTest
{
    [Test]
    public async Task GetPredictions_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("predictions?fixture=1557430",
            GetExpected("Test_Predictions_UpcomingFixture_Real_Response"));
        var response = await new PredictionsClient(factory, MockOptions()).GetPredictions(1557430);
        CapturedApiResponseAssertions.AssertPage(response, "predictions");
        CapturedApiResponseAssertions.AssertParameter(response, "fixture", "1557430");
        Assert.That(response.Response, Has.Count.EqualTo(1));
        var prediction = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(prediction.Predictions?.Winner?.Id, Is.EqualTo(49));
            Assert.That(prediction.Predictions?.Winner?.Name, Is.EqualTo("Chelsea"));
            Assert.That(prediction.Predictions?.Winner?.Comment, Is.EqualTo("Win or draw"));
            Assert.That(prediction.Predictions?.WinOrDraw, Is.True);
            Assert.That(prediction.Predictions?.UnderOver, Is.Null);
            Assert.That(prediction.Predictions?.Goals?.Home, Is.EqualTo("-2.5"));
            Assert.That(prediction.Predictions?.Goals?.Away, Is.EqualTo("-2.5"));
            Assert.That(prediction.Predictions?.Advice, Is.EqualTo("Double chance : draw or Chelsea"));
            Assert.That(prediction.Predictions?.Percent?.Home, Is.EqualTo("10%"));
            Assert.That(prediction.Predictions?.Percent?.Draw, Is.EqualTo("45%"));
            Assert.That(prediction.Predictions?.Percent?.Away, Is.EqualTo("45%"));
            Assert.That(prediction.League?.Id, Is.EqualTo(39));
            Assert.That(prediction.League?.Name, Is.EqualTo("Premier League"));
            Assert.That(prediction.League?.Country, Is.EqualTo("England"));
            Assert.That(prediction.League?.Logo, Is.EqualTo("https://media.api-sports.io/football/leagues/39.png"));
            Assert.That(prediction.League?.Flag, Is.EqualTo("https://media.api-sports.io/flags/gb-eng.svg"));
            Assert.That(prediction.League?.Season, Is.EqualTo(2026));
            Assert.That(prediction.Teams?.Home?.Id, Is.EqualTo(45));
            Assert.That(prediction.Teams?.Home?.Name, Is.EqualTo("Everton"));
            Assert.That(prediction.Teams?.Home?.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/45.png"));
            Assert.That(prediction.Teams?.Home?.Last5?.Form, Is.EqualTo("47%"));
            Assert.That(prediction.Teams?.Home?.Last5?.Att, Is.EqualTo("33%"));
            Assert.That(prediction.Teams?.Home?.Last5?.Def, Is.EqualTo("72%"));
            Assert.That(prediction.Teams?.Home?.Last5?.Goals?.For?.Total, Is.EqualTo(6));
            Assert.That(prediction.Teams?.Home?.Last5?.Goals?.For?.Average, Is.EqualTo("1.2"));
            Assert.That(prediction.Teams?.Home?.Last5?.Goals?.Against?.Total, Is.EqualTo(5));
            Assert.That(prediction.Teams?.Home?.Last5?.Goals?.Against?.Average, Is.EqualTo("1.0"));
            Assert.That(prediction.Teams?.Away?.Id, Is.EqualTo(49));
            Assert.That(prediction.Teams?.Away?.Name, Is.EqualTo("Chelsea"));
            Assert.That(prediction.Teams?.Away?.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/49.png"));
            Assert.That(prediction.Teams?.Away?.Last5?.Form, Is.EqualTo("47%"));
            Assert.That(prediction.Teams?.Away?.Last5?.Att, Is.EqualTo("67%"));
            Assert.That(prediction.Teams?.Away?.Last5?.Def, Is.EqualTo("39%"));
            Assert.That(prediction.Teams?.Away?.Last5?.Goals?.For?.Total, Is.EqualTo(12));
            Assert.That(prediction.Teams?.Away?.Last5?.Goals?.For?.Average, Is.EqualTo("2.4"));
            Assert.That(prediction.Teams?.Away?.Last5?.Goals?.Against?.Total, Is.EqualTo(11));
            Assert.That(prediction.Teams?.Away?.Last5?.Goals?.Against?.Average, Is.EqualTo("2.2"));
            Assert.That(prediction.Comparison?.Form?.Home, Is.EqualTo("50%"));
            Assert.That(prediction.Comparison?.Form?.Away, Is.EqualTo("50%"));
            Assert.That(prediction.Comparison?.Att?.Home, Is.EqualTo("33%"));
            Assert.That(prediction.Comparison?.Att?.Away, Is.EqualTo("67%"));
            Assert.That(prediction.Comparison?.Def?.Home, Is.EqualTo("69%"));
            Assert.That(prediction.Comparison?.Def?.Away, Is.EqualTo("31%"));
            Assert.That(prediction.Comparison?.PoissonDistribution?.Home, Is.EqualTo("78%"));
            Assert.That(prediction.Comparison?.PoissonDistribution?.Away, Is.EqualTo("22%"));
            Assert.That(prediction.Comparison?.H2h?.Home, Is.EqualTo("29%"));
            Assert.That(prediction.Comparison?.H2h?.Away, Is.EqualTo("71%"));
            Assert.That(prediction.Comparison?.Goals?.Home, Is.EqualTo("25%"));
            Assert.That(prediction.Comparison?.Goals?.Away, Is.EqualTo("75%"));
            Assert.That(prediction.Comparison?.Total?.Home, Is.EqualTo("47.3%"));
            Assert.That(prediction.Comparison?.Total?.Away, Is.EqualTo("52.7%"));
            Assert.That(prediction.H2h, Has.Count.EqualTo(10));
            Assert.That(prediction.H2h![0].Fixture.Id, Is.EqualTo(1379272));
        });
    }
}