namespace ApiFootball.IntegrationTests.Tests;

[TestFixture]
public sealed class FixtureDetailsLiveTests : BaseLiveTest
{
    private const int CompletedFixture = 1213546;

    [Test]
    public async Task FixtureDetails_ShouldReturnStatisticsEventsLineupsAndPlayers()
    {
        var fixtures = ServiceProvider.GetRequiredService<IFixturesClient>();

        var statistics = await fixtures.GetFixtureStatistics(CompletedFixture);
        Assert.That(statistics.Errors, Is.Empty);
        Assert.That(statistics.Response, Is.Not.Empty);
        Assert.That(statistics.Response.All(item => item.Statistics is { Count: > 0 }), Is.True);

        var events = await fixtures.GetFixtureEvents(CompletedFixture);
        Assert.That(events.Errors, Is.Empty);
        Assert.That(events.Response, Is.Not.Empty);
        Assert.That(events.Response.Any(item => !string.IsNullOrWhiteSpace(item.Type)), Is.True);

        var lineups = await fixtures.GetFixtureLineups(CompletedFixture);
        Assert.That(lineups.Errors, Is.Empty);
        Assert.That(lineups.Response, Has.Count.EqualTo(2));
        Assert.That(lineups.Response.All(item => item.StartXI is { Count: > 0 }), Is.True);

        var players = await fixtures.GetFixturePlayers(CompletedFixture);
        Assert.That(players.Errors, Is.Empty);
        Assert.That(players.Response, Has.Count.EqualTo(2));
        Assert.That(players.Response.All(item => item.Players is { Count: > 0 }), Is.True);
    }
}

[TestFixture]
public sealed class AdditionalClientsLiveTests : BaseLiveTest
{
    [Test]
    public async Task GetCoachs_ShouldReturnTeamCareerData()
    {
        var client = ServiceProvider.GetRequiredService<ICoachesClient>();
        var response = await client.GetCoaches(team: 40);

        Assert.That(response.Errors, Is.Empty);
        Assert.That(response.Response, Is.Not.Empty);
        Assert.That(response.Response.Any(coach => coach.Id > 0 && coach.Career is { Count: > 0 }), Is.True);
    }

    [Test]
    public async Task GetPlayerProfiles_ShouldFindMessi()
    {
        var client = ServiceProvider.GetRequiredService<IPlayersClient>();
        var response = await client.GetProfiles(search: "Messi");

        Assert.That(response.Errors, Is.Empty);
        Assert.That(
            response.Response.Any(item =>
                item.Player?.Name?.Contains("Messi", StringComparison.OrdinalIgnoreCase) == true), Is.True);
    }

    [Test]
    public async Task GetTopScorers_ShouldReturnLeaguePlayerStatistics()
    {
        var client = ServiceProvider.GetRequiredService<IPlayersClient>();
        var response = await client.GetTopScorers(39, 2024);

        Assert.That(response.Errors, Is.Empty);
        Assert.That(response.Response, Is.Not.Empty);
        Assert.That(response.Response.All(item => item.Player?.Id > 0 && item.Statistics is { Count: > 0 }), Is.True);
    }

    [Test]
    public async Task GetTeamStatistics_ShouldReturnSeasonTotals()
    {
        var client = ServiceProvider.GetRequiredService<ITeamsClient>();
        var response = await client.GetTeamStatistics(39, 2024, 40);

        Assert.That(response.Errors, Is.Empty);
        Assert.That(response.GetResponse().Team?.Id, Is.EqualTo(40));
        Assert.That(response.GetResponse().League?.Season, Is.EqualTo(2024));
        Assert.That(response.GetResponse().Fixtures?.Played?.Total, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetInjuries_ShouldReturnCurrentLeagueAvailability()
    {
        var now = DateTime.UtcNow;
        var season = now.Month >= 7 ? now.Year : now.Year - 1;
        var response = await ServiceProvider.GetRequiredService<IInjuriesClient>()
            .GetInjuries(39, season, team: 40);

        Assert.That(response.Errors, Is.Empty);
        Assert.That(response.Response.All(item => item.League?.Id == 39 && item.Team?.Id == 40), Is.True);
    }

    [Test]
    public async Task PlayerHistory_ShouldReturnTransfersTrophiesAndSidelinedRecords()
    {
        var transfers = await ServiceProvider.GetRequiredService<ITransfersClient>().GetTransfers(276);
        Assert.That(transfers.Errors, Is.Empty);
        Assert.That(transfers.Response.Any(item => item.Player?.Id == 276 && item.Transfers is { Count: > 0 }),
            Is.True);

        var trophies = await ServiceProvider.GetRequiredService<ITrophiesClient>().GetTrophies(276);
        Assert.That(trophies.Errors, Is.Empty);
        Assert.That(trophies.Response, Is.Not.Empty);

        var sidelined = await ServiceProvider.GetRequiredService<ISidelinedClient>().GetSidelined(276);
        Assert.That(sidelined.Errors, Is.Empty);
        Assert.That(sidelined.Response.Any(item => !string.IsNullOrWhiteSpace(item.Type) && item.Start.HasValue),
            Is.True);
    }

    [Test]
    public async Task GetPredictions_ShouldReturnForecastForNextPremierLeagueFixture()
    {
        var now = DateTime.UtcNow;
        var season = now.Month >= 7 ? now.Year : now.Year - 1;
        var fixtures = await ServiceProvider.GetRequiredService<IFixturesClient>()
            .GetFixtures(league: 39, season: season, next: 1);

        Assert.That(fixtures.Errors, Is.Empty);
        Assert.That(fixtures.Response, Is.Not.Empty);
        var fixtureId = fixtures.Response[0].Fixture.Id;

        var predictions = await ServiceProvider.GetRequiredService<IPredictionsClient>().GetPredictions(fixtureId);
        Assert.That(predictions.Errors, Is.Empty);
        Assert.That(predictions.Response.Any(item => item.Predictions?.Percent is not null), Is.True);
    }
}