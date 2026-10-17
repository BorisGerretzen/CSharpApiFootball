using System.Text.Json;
using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

public sealed class OddsClientTests : BaseEndpointTest
{
    [Test]
    public async Task GetBookmakers_DeserializesCapturedLiveResponse()
    {
        var factory = MockFactory("odds/bookmakers", GetExpected("Test_OddsBookmakers_Live_Response"));
        var client = new OddsClient(factory, MockOptions());
        var response = await client.GetBookmakers();
        Assert.Multiple(() =>
        {
            Assert.That(response.Get, Is.EqualTo("odds/bookmakers"));
            Assert.That(response.Errors, Is.Empty);
            Assert.That(response.Response, Has.Count.EqualTo(response.Results));
            Assert.That(response.Response, Is.Not.Empty);
            Assert.That(response.Response[0].Id, Is.EqualTo(1));
            Assert.That(response.Response[0].Name, Is.EqualTo("10Bet"));
            Assert.That(response.Response[1].Id, Is.EqualTo(2));
            Assert.That(response.Response[1].Name, Is.EqualTo("Marathonbet"));
            Assert.That(response.Response[2].Id, Is.EqualTo(3));
            Assert.That(response.Response[2].Name, Is.EqualTo("Betfair"));
            Assert.That(response.Response[3].Id, Is.EqualTo(4));
            Assert.That(response.Response[3].Name, Is.EqualTo("Pinnacle"));
        });
    }

    [Test]
    public async Task GetOdds_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("odds?fixture=1557430", GetExpected("Test_Odds_UpcomingFixture_Real_Response"));
        var response = await new OddsClient(factory, MockOptions()).GetOdds(1557430);
        CapturedApiResponseAssertions.AssertPage(response, "odds");
        CapturedApiResponseAssertions.AssertParameter(response, "fixture", "1557430");
        Assert.That(response.Response, Has.Count.EqualTo(1));
        var odds = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(odds.League?.Id, Is.EqualTo(39));
            Assert.That(odds.League?.Name, Is.EqualTo("Premier League"));
            Assert.That(odds.League?.Country, Is.EqualTo("England"));
            Assert.That(odds.League?.Logo, Is.EqualTo("https://media.api-sports.io/football/leagues/39.png"));
            Assert.That(odds.League?.Flag, Is.EqualTo("https://media.api-sports.io/flags/gb-eng.svg"));
            Assert.That(odds.League?.Season, Is.EqualTo(2026));
            Assert.That(odds.Fixture?.Id, Is.EqualTo(1557430));
            Assert.That(odds.Fixture?.Timezone, Is.EqualTo("UTC"));
            Assert.That(odds.Fixture?.Date, Is.EqualTo(new DateTimeOffset(2026, 10, 17, 11, 30, 0, TimeSpan.Zero)));
            Assert.That(odds.Fixture?.Timestamp, Is.EqualTo(1792236600));
            Assert.That(odds.Update, Is.EqualTo(new DateTimeOffset(2026, 10, 15, 12, 30, 56, TimeSpan.Zero)));
            Assert.That(odds.Bookmakers, Has.Count.EqualTo(10));
        });
        var bookmaker = odds.Bookmakers![0];
        Assert.Multiple(() =>
        {
            Assert.That(bookmaker.Id, Is.EqualTo(7));
            Assert.That(bookmaker.Name, Is.EqualTo("William Hill"));
            Assert.That(bookmaker.Bets, Has.Count.EqualTo(48));
            Assert.That(bookmaker.Bets![0].Id, Is.EqualTo(1));
            Assert.That(bookmaker.Bets[0].Name, Is.EqualTo("Match Winner"));
            Assert.That(bookmaker.Bets[0].Values, Has.Count.EqualTo(3));
        });
        var homeOdd = bookmaker.Bets![0].Values![0];
        Assert.Multiple(() =>
        {
            Assert.That(homeOdd.Value?.ValueKind, Is.EqualTo(JsonValueKind.String));
            Assert.That(homeOdd.Value?.GetString(), Is.EqualTo("Home"));
            Assert.That(homeOdd.Odd, Is.EqualTo("3.10"));
            Assert.That(homeOdd.Handicap, Is.Null);
            Assert.That(homeOdd.Main, Is.Null);
            Assert.That(homeOdd.Suspended, Is.Null);
            Assert.That(bookmaker.Bets[0].Values[1].Value?.GetString(), Is.EqualTo("Draw"));
            Assert.That(bookmaker.Bets[0].Values[1].Odd, Is.EqualTo("3.50"));
            Assert.That(bookmaker.Bets[0].Values[2].Value?.GetString(), Is.EqualTo("Away"));
            Assert.That(bookmaker.Bets[0].Values[2].Odd, Is.EqualTo("2.10"));
        });
        var exactGoals = bookmaker.Bets.Single(bet => bet.Id == 38);
        Assert.That(exactGoals.Name, Is.EqualTo("Exact Goals Number"));
        Assert.That(exactGoals.Values, Has.Count.EqualTo(7));
        Assert.Multiple(() =>
        {
            Assert.That(exactGoals.Values![0].Value?.ValueKind, Is.EqualTo(JsonValueKind.Number));
            Assert.That(exactGoals.Values[0].Value?.GetInt32(), Is.EqualTo(0));
            Assert.That(exactGoals.Values[0].Odd, Is.EqualTo("13.00"));
            Assert.That(exactGoals.Values[6].Value?.ValueKind, Is.EqualTo(JsonValueKind.String));
            Assert.That(exactGoals.Values[6].Value?.GetString(), Is.EqualTo("more 6"));
            Assert.That(exactGoals.Values[6].Odd, Is.EqualTo("9.00"));
        });
    }

    [Test]
    public async Task GetOddsMapping_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("odds/mapping?page=1", GetExpected("Test_OddsMapping_Page1_Real_Response"));
        var response = await new OddsClient(factory, MockOptions()).GetOddsMapping(1);
        CapturedApiResponseAssertions.AssertPage(response, "odds/mapping", 40);
        CapturedApiResponseAssertions.AssertParameter(response, "page", "1");
        Assert.That(response.Response, Has.Count.EqualTo(100));
        var mapping = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(mapping.League?.Id, Is.EqualTo(253));
            Assert.That(mapping.League?.Season, Is.EqualTo(2026));
            Assert.That(mapping.Fixture?.Id, Is.EqualTo(1490511));
            Assert.That(mapping.Fixture?.Date, Is.EqualTo(new DateTimeOffset(2026, 10, 10, 18, 30, 0, TimeSpan.Zero)));
            Assert.That(mapping.Fixture?.Timestamp, Is.EqualTo(1791657000));
            Assert.That(mapping.Update, Is.EqualTo(new DateTimeOffset(2026, 10, 10, 18, 0, 21, TimeSpan.Zero)));
        });
    }

    [Test]
    public async Task GetBookmakers_WithSearch_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("odds/bookmakers?search=Bet365",
            GetExpected("Test_OddsBookmakers_Search_Real_Response"));
        var response = await new OddsClient(factory, MockOptions()).GetBookmakers(search: "Bet365");
        CapturedApiResponseAssertions.AssertPage(response, "odds/bookmakers");
        CapturedApiResponseAssertions.AssertParameter(response, "search", "Bet365");
        Assert.That(response.Response, Has.Count.EqualTo(1));
        Assert.That(response.Response[0].Id, Is.EqualTo(8));
        Assert.That(response.Response[0].Name, Is.EqualTo("Bet365"));
    }

    [Test]
    public async Task GetBets_WithSearch_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("odds/bets?search=Winner", GetExpected("Test_OddsBets_Search_Real_Response"));
        var response = await new OddsClient(factory, MockOptions()).GetBets(search: "Winner");
        CapturedApiResponseAssertions.AssertPage(response, "odds/bets");
        CapturedApiResponseAssertions.AssertParameter(response, "search", "Winner");
        Assert.That(response.Response, Has.Count.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(response.Response[0].Id, Is.EqualTo(1));
            Assert.That(response.Response[0].Name, Is.EqualTo("Match Winner"));
            Assert.That(response.Response[1].Id, Is.EqualTo(3));
            Assert.That(response.Response[1].Name, Is.EqualTo("Second Half Winner"));
            Assert.That(response.Response[2].Id, Is.EqualTo(13));
            Assert.That(response.Response[2].Name, Is.EqualTo("First Half Winner"));
        });
    }

    [Test]
    public async Task GetLiveOdds_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("odds/live", GetExpected("Test_LiveOdds_Real_Response"));
        var response = await new OddsClient(factory, MockOptions()).GetLiveOdds();
        CapturedApiResponseAssertions.AssertPage(response, "odds/live");
        Assert.That(response.Parameters, Is.Empty);
        Assert.That(response.Response, Has.Count.EqualTo(7));
        var live = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(live.Fixture?.Id, Is.EqualTo(1549659));
            Assert.That(live.Fixture?.Status?.Long, Is.EqualTo("Second Half"));
            Assert.That(live.Fixture?.Status?.Elapsed, Is.EqualTo(78));
            Assert.That(live.Fixture?.Status?.Seconds, Is.EqualTo("78:04"));
            Assert.That(live.League?.Id, Is.EqualTo(240));
            Assert.That(live.League?.Season, Is.EqualTo(2026));
            Assert.That(live.Teams?.Home?.Id, Is.EqualTo(1466));
            Assert.That(live.Teams?.Home?.Goals, Is.EqualTo(2));
            Assert.That(live.Teams?.Away?.Id, Is.EqualTo(1145));
            Assert.That(live.Teams?.Away?.Goals, Is.EqualTo(1));
            Assert.That(live.Status?.Stopped, Is.False);
            Assert.That(live.Status?.Blocked, Is.False);
            Assert.That(live.Status?.Finished, Is.False);
            Assert.That(live.Update, Is.EqualTo(new DateTimeOffset(2026, 10, 15, 22, 37, 0, TimeSpan.Zero)));
            Assert.That(live.Odds, Has.Count.EqualTo(25));
        });
        var handicap = live.Odds![0];
        Assert.Multiple(() =>
        {
            Assert.That(handicap.Id, Is.EqualTo(33));
            Assert.That(handicap.Name, Is.EqualTo("Asian Handicap"));
            Assert.That(handicap.Values, Has.Count.EqualTo(10));
            Assert.That(handicap.Values![0].Value?.GetString(), Is.EqualTo("Home"));
            Assert.That(handicap.Values[0].Odd, Is.EqualTo("1.3"));
            Assert.That(handicap.Values[0].Handicap, Is.EqualTo("-0.75"));
            Assert.That(handicap.Values[0].Main, Is.False);
            Assert.That(handicap.Values[0].Suspended, Is.False);
            Assert.That(handicap.Values[2].Value?.GetString(), Is.EqualTo("Home"));
            Assert.That(handicap.Values[2].Odd, Is.EqualTo("1.725"));
            Assert.That(handicap.Values[2].Handicap, Is.EqualTo("-1"));
            Assert.That(handicap.Values[2].Main, Is.True);
            Assert.That(handicap.Values[2].Suspended, Is.False);
        });
    }

    [Test]
    public async Task GetLiveBets_WithSearch_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("odds/live/bets?search=Winner", GetExpected("Test_LiveBets_Search_Real_Response"));
        var response = await new OddsClient(factory, MockOptions()).GetLiveBets(search: "Winner");
        CapturedApiResponseAssertions.AssertPage(response, "odds/live/bets");
        CapturedApiResponseAssertions.AssertParameter(response, "search", "Winner");
        Assert.That(response.Response, Has.Count.EqualTo(1));
        Assert.That(response.Response[0].Id, Is.EqualTo(107));
        Assert.That(response.Response[0].Name, Is.EqualTo("Penalties Shootout Winner"));
    }
}