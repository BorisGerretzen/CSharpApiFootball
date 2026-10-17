using System.Text.Json;
using ApiFootball.Clients.Implementation;
using ApiFootball.Models;

namespace ApiFootball.Test.Clients.Implementation;

public sealed class PlayersClientTests : BaseEndpointTest
{
    [Test]
    public void PenaltyStatistics_MapTheApiFieldSpelling()
    {
        const string json = """{"commited":3}""";
        var playerPenalty = JsonSerializer.Deserialize<PlayerPenaltyStatistics>(json);
        var fixturePenalty = JsonSerializer.Deserialize<FixturePlayerPenalty>(json);
        Assert.Multiple(() =>
        {
            Assert.That(playerPenalty?.Committed, Is.EqualTo(3));
            Assert.That(fixturePenalty?.Committed, Is.EqualTo(3));
        });
    }

    [Test]
    public async Task GetPlayerSeasons_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("players/seasons?player=276", GetExpected("Test_PlayerSeasons_Real_Response"));
        var response = await new PlayersClient(factory, MockOptions()).GetSeasons(276);
        CapturedApiResponseAssertions.AssertPage(response, "players/seasons");
        CapturedApiResponseAssertions.AssertParameter(response, "player", "276");
        Assert.That(response.Response,
            Is.EqualTo(new[]
            {
                2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025,
                2026
            }));
    }

    [Test]
    public async Task GetPlayerProfiles_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("players/profiles?search=Messi", GetExpected("Test_PlayerProfiles_Real_Response"));
        var response = await new PlayersClient(factory, MockOptions()).GetProfiles(search: "Messi");
        CapturedApiResponseAssertions.AssertPage(response, "players/profiles");
        CapturedApiResponseAssertions.AssertParameter(response, "search", "Messi");
        Assert.That(response.Response, Has.Count.EqualTo(89));
        var messi = response.Response[0].Player!;
        Assert.Multiple(() =>
        {
            Assert.That(messi.Id, Is.EqualTo(154));
            Assert.That(messi.Name, Is.EqualTo("L. Messi"));
            Assert.That(messi.Firstname, Is.EqualTo("Lionel Andrés"));
            Assert.That(messi.Lastname, Is.EqualTo("Messi Cuccittini"));
            Assert.That(messi.Age, Is.EqualTo(39));
            Assert.That(messi.Birth?.Date, Is.EqualTo(new DateOnly(1987, 6, 24)));
            Assert.That(messi.Birth?.Place, Is.EqualTo("Rosario"));
            Assert.That(messi.Birth?.Country, Is.EqualTo("Argentina"));
            Assert.That(messi.Nationality, Is.EqualTo("Argentina"));
            Assert.That(messi.Height, Is.EqualTo("170"));
            Assert.That(messi.Weight, Is.EqualTo("67"));
            Assert.That(messi.Injured, Is.Null);
            Assert.That(messi.Number, Is.EqualTo(10));
            Assert.That(messi.Position, Is.EqualTo("Attacker"));
            Assert.That(messi.Photo, Is.EqualTo("https://media.api-sports.io/football/players/154.png"));
        });
    }

    [Test]
    public async Task GetPlayerStatistics_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("players?id=276&season=2024", GetExpected("Test_PlayerStatistics_Real_Response"));
        var response = await new PlayersClient(factory, MockOptions()).GetPlayerStatistics(276, season: 2024);
        CapturedApiResponseAssertions.AssertPage(response, "players");
        CapturedApiResponseAssertions.AssertParameter(response, "id", "276", 2);
        CapturedApiResponseAssertions.AssertParameter(response, "season", "2024", 2);
        Assert.That(response.Response, Has.Count.EqualTo(1));
        var player = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(player.Player?.Id, Is.EqualTo(276));
            Assert.That(player.Player?.Name, Is.EqualTo("Neymar"));
            Assert.That(player.Player?.Firstname, Is.EqualTo("Neymar"));
            Assert.That(player.Player?.Lastname, Is.EqualTo("da Silva Santos Júnior"));
            Assert.That(player.Player?.Age, Is.EqualTo(33));
            Assert.That(player.Player?.Birth?.Date, Is.EqualTo(new DateOnly(1992, 2, 5)));
            Assert.That(player.Player?.Birth?.Place, Is.EqualTo("Mogi das Cruzes"));
            Assert.That(player.Player?.Birth?.Country, Is.EqualTo("Brazil"));
            Assert.That(player.Player?.Nationality, Is.EqualTo("Brazil"));
            Assert.That(player.Player?.Height, Is.EqualTo("175"));
            Assert.That(player.Player?.Weight, Is.EqualTo("68"));
            Assert.That(player.Player?.Injured, Is.False);
            Assert.That(player.Player?.Number, Is.Null);
            Assert.That(player.Player?.Position, Is.Null);
            Assert.That(player.Player?.Photo, Is.EqualTo("https://media.api-sports.io/football/players/276.png"));
            Assert.That(player.Statistics, Has.Count.EqualTo(4));
        });
        var season = player.Statistics![0];
        Assert.Multiple(() =>
        {
            Assert.That(season.Team?.Id, Is.EqualTo(2932));
            Assert.That(season.Team?.Name, Is.EqualTo("Al-Hilal Saudi FC"));
            Assert.That(season.Team?.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/2932.png"));
            Assert.That(season.League?.Id, Is.EqualTo(307));
            Assert.That(season.League?.Name, Is.EqualTo("Pro League"));
            Assert.That(season.League?.Country, Is.EqualTo("Saudi-Arabia"));
            Assert.That(season.League?.Logo, Is.EqualTo("https://media.api-sports.io/football/leagues/307.png"));
            Assert.That(season.League?.Flag, Is.EqualTo("https://media.api-sports.io/flags/sa.svg"));
            Assert.That(season.League?.Season, Is.EqualTo(2024));
            Assert.That(season.Games?.Appearances, Is.EqualTo(0));
            Assert.That(season.Games?.Lineups, Is.EqualTo(0));
            Assert.That(season.Games?.Minutes, Is.EqualTo(0));
            Assert.That(season.Games?.Number, Is.Null);
            Assert.That(season.Games?.Position, Is.EqualTo("Attacker"));
            Assert.That(season.Games?.Rating, Is.Null);
            Assert.That(season.Games?.Captain, Is.False);
            Assert.That(season.Substitutes?.In, Is.EqualTo(0));
            Assert.That(season.Substitutes?.Out, Is.EqualTo(0));
            Assert.That(season.Substitutes?.Bench, Is.EqualTo(0));
            Assert.That(season.Shots?.Total, Is.Null);
            Assert.That(season.Shots?.On, Is.Null);
            Assert.That(season.Goals?.Total, Is.EqualTo(0));
            Assert.That(season.Goals?.Conceded, Is.Null);
            Assert.That(season.Goals?.Assists, Is.Null);
            Assert.That(season.Goals?.Saves, Is.Null);
            Assert.That(season.Passes?.Total, Is.Null);
            Assert.That(season.Passes?.Key, Is.Null);
            Assert.That(season.Passes?.Accuracy, Is.Null);
            Assert.That(season.Tackles?.Total, Is.Null);
            Assert.That(season.Tackles?.Blocks, Is.Null);
            Assert.That(season.Tackles?.Interceptions, Is.Null);
            Assert.That(season.Duels?.Total, Is.Null);
            Assert.That(season.Duels?.Won, Is.Null);
            Assert.That(season.Dribbles?.Attempts, Is.Null);
            Assert.That(season.Dribbles?.Success, Is.Null);
            Assert.That(season.Dribbles?.Past, Is.Null);
            Assert.That(season.Fouls?.Drawn, Is.Null);
            Assert.That(season.Fouls?.Committed, Is.Null);
            Assert.That(season.Cards?.Yellow, Is.EqualTo(0));
            Assert.That(season.Cards?.Yellowred, Is.EqualTo(0));
            Assert.That(season.Cards?.Red, Is.EqualTo(0));
            Assert.That(season.Penalty?.Won, Is.Null);
            Assert.That(season.Penalty?.Committed, Is.Null);
            Assert.That(season.Penalty?.Scored, Is.Null);
            Assert.That(season.Penalty?.Missed, Is.Null);
            Assert.That(season.Penalty?.Saved, Is.Null);
        });
    }

    [Test]
    public async Task GetPlayerSquads_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("players/squads?team=40", GetExpected("Test_PlayerSquads_Real_Response"));
        var response = await new PlayersClient(factory, MockOptions()).GetSquads(40);
        CapturedApiResponseAssertions.AssertPage(response, "players/squads");
        CapturedApiResponseAssertions.AssertParameter(response, "team", "40");
        Assert.That(response.Response, Has.Count.EqualTo(1));
        var squad = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(squad.Team?.Id, Is.EqualTo(40));
            Assert.That(squad.Team?.Name, Is.EqualTo("Liverpool"));
            Assert.That(squad.Team?.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/40.png"));
            Assert.That(squad.Players, Has.Count.EqualTo(82));
        });
        var alisson = squad.Players![0];
        Assert.Multiple(() =>
        {
            Assert.That(alisson.Id, Is.EqualTo(280));
            Assert.That(alisson.Name, Is.EqualTo("Alisson Becker"));
            Assert.That(alisson.Firstname, Is.Null);
            Assert.That(alisson.Lastname, Is.Null);
            Assert.That(alisson.Age, Is.EqualTo(33));
            Assert.That(alisson.Birth, Is.Null);
            Assert.That(alisson.Nationality, Is.Null);
            Assert.That(alisson.Height, Is.Null);
            Assert.That(alisson.Weight, Is.Null);
            Assert.That(alisson.Injured, Is.Null);
            Assert.That(alisson.Number, Is.EqualTo(1));
            Assert.That(alisson.Position, Is.EqualTo("Goalkeeper"));
            Assert.That(alisson.Photo, Is.EqualTo("https://media.api-sports.io/football/players/280.png"));
        });
    }

    [Test]
    public async Task GetPlayerTeams_DeserializesCapturedApiResponse()
    {
        var factory = MockFactory("players/teams?player=276", GetExpected("Test_PlayerTeams_Real_Response"));
        var response = await new PlayersClient(factory, MockOptions()).GetTeams(276);
        CapturedApiResponseAssertions.AssertPage(response, "players/teams");
        CapturedApiResponseAssertions.AssertParameter(response, "player", "276");
        Assert.That(response.Response, Has.Count.EqualTo(8));
        Assert.Multiple(() =>
        {
            Assert.That(response.Response[0].Team?.Id, Is.EqualTo(6));
            Assert.That(response.Response[0].Team?.Name, Is.EqualTo("Brazil"));
            Assert.That(response.Response[0].Team?.Logo,
                Is.EqualTo("https://media.api-sports.io/football/teams/6.png"));
            Assert.That(response.Response[0].Seasons,
                Is.EqualTo(new[]
                    { 2026, 2023, 2022, 2021, 2019, 2018, 2017, 2016, 2015, 2014, 2013, 2012, 2011, 2010 }));
        });
    }

    [TestCase("topscorers", "Test_TopScorers_Real_Response", 1165, "Matheus Cunha", 33, 66, 30, 12, 8, 0)]
    [TestCase("topassists", "Test_TopAssists_Real_Response", 306, "Mohamed Salah", 40, 38, 29, 18, 1, 0)]
    [TestCase("topyellowcards", "Test_TopYellowCards_Real_Response", 2823, "S. Lukić", 36, 30, null, 2, 12, 0)]
    [TestCase("topredcards", "Test_TopRedCards_Real_Response", 1485, "Bruno Fernandes", 33, 36, 8, 10, 5, 2)]
    public async Task GetPlayerLeaders_DeserializesCapturedApiResponse(
        string endpoint, string fixture, int expectedPlayerId, string expectedPlayerName, int expectedTeamId,
        int expectedAppearances,
        int? expectedGoals, int expectedAssists, int expectedYellowCards, int expectedRedCards)
    {
        var route = $"players/{endpoint}?league=39&season=2024";
        var factory = MockFactory(route, GetExpected(fixture));
        var client = new PlayersClient(factory, MockOptions());
        var response = endpoint switch
        {
            "topscorers" => await client.GetTopScorers(39, 2024),
            "topassists" => await client.GetTopAssists(39, 2024),
            "topyellowcards" => await client.GetTopYellowCards(39, 2024),
            "topredcards" => await client.GetTopRedCards(39, 2024),
            _ => throw new ArgumentOutOfRangeException(nameof(endpoint), endpoint, "Unknown player leader endpoint")
        };
        CapturedApiResponseAssertions.AssertPage(response, $"players/{endpoint}");
        CapturedApiResponseAssertions.AssertParameter(response, "league", "39", 2);
        CapturedApiResponseAssertions.AssertParameter(response, "season", "2024", 2);
        Assert.That(response.Response,
            Has.Count.EqualTo(endpoint == "topassists" ? 19 :
                endpoint == "topyellowcards" ? 20 :
                endpoint == "topredcards" ? 20 : 20));
        var leader = response.Response[0];
        Assert.Multiple(() =>
        {
            Assert.That(leader.Player?.Id, Is.EqualTo(expectedPlayerId));
            Assert.That(leader.Player?.Name, Is.EqualTo(expectedPlayerName));
            Assert.That(leader.Player?.Injured, Is.False);
            Assert.That(leader.Statistics, Has.Count.EqualTo(1));
            Assert.That(leader.Statistics![0].Team?.Id, Is.EqualTo(expectedTeamId));
            Assert.That(leader.Statistics[0].League?.Id, Is.EqualTo(39));
            Assert.That(leader.Statistics[0].League?.Name, Is.EqualTo("Premier League"));
            Assert.That(leader.Statistics[0].League?.Country, Is.EqualTo("England"));
            Assert.That(leader.Statistics[0].League?.Logo,
                Is.EqualTo("https://media.api-sports.io/football/leagues/39.png"));
            Assert.That(leader.Statistics[0].League?.Flag, Is.EqualTo("https://media.api-sports.io/flags/gb-eng.svg"));
            Assert.That(leader.Statistics[0].League?.Season, Is.EqualTo(2024));
            Assert.That(leader.Statistics[0].Games?.Appearances, Is.EqualTo(expectedAppearances));
            Assert.That(leader.Statistics[0].Goals?.Total, Is.EqualTo(expectedGoals));
            Assert.That(leader.Statistics[0].Goals?.Assists, Is.EqualTo(expectedAssists));
            Assert.That(leader.Statistics[0].Cards?.Yellow, Is.EqualTo(expectedYellowCards));
            Assert.That(leader.Statistics[0].Cards?.Red, Is.EqualTo(expectedRedCards));
        });
        if (endpoint == "topscorers")
        {
            var stats = leader.Statistics![0];
            Assert.Multiple(() =>
            {
                Assert.That(leader.Player?.Firstname, Is.EqualTo("Matheus Santos"));
                Assert.That(leader.Player?.Lastname, Is.EqualTo("Carneiro da Cunha"));
                Assert.That(leader.Player?.Age, Is.EqualTo(26));
                Assert.That(leader.Player?.Birth?.Date, Is.EqualTo(new DateOnly(1999, 5, 27)));
                Assert.That(leader.Player?.Birth?.Place, Is.EqualTo("João Pessoa"));
                Assert.That(leader.Player?.Birth?.Country, Is.EqualTo("Brazil"));
                Assert.That(leader.Player?.Nationality, Is.EqualTo("Brazil"));
                Assert.That(leader.Player?.Height, Is.EqualTo("183"));
                Assert.That(leader.Player?.Weight, Is.EqualTo("76"));
                Assert.That(leader.Player?.Number, Is.Null);
                Assert.That(leader.Player?.Position, Is.Null);
                Assert.That(leader.Player?.Photo, Is.EqualTo("https://media.api-sports.io/football/players/1165.png"));
                Assert.That(stats.Team?.Name, Is.EqualTo("Manchester United"));
                Assert.That(stats.Team?.Logo, Is.EqualTo("https://media.api-sports.io/football/teams/33.png"));
                Assert.That(stats.Games?.Lineups, Is.EqualTo(61));
                Assert.That(stats.Games?.Minutes, Is.EqualTo(5203));
                Assert.That(stats.Games?.Number, Is.Null);
                Assert.That(stats.Games?.Position, Is.EqualTo("Attacker"));
                Assert.That(stats.Games?.Rating, Is.EqualTo("7.630303"));
                Assert.That(stats.Games?.Captain, Is.False);
                Assert.That(stats.Substitutes?.In, Is.EqualTo(5));
                Assert.That(stats.Substitutes?.Out, Is.EqualTo(10));
                Assert.That(stats.Substitutes?.Bench, Is.EqualTo(4));
                Assert.That(stats.Shots?.Total, Is.EqualTo(160));
                Assert.That(stats.Shots?.On, Is.EqualTo(88));
                Assert.That(stats.Goals?.Conceded, Is.EqualTo(0));
                Assert.That(stats.Goals?.Saves, Is.Null);
                Assert.That(stats.Passes?.Total, Is.EqualTo(1898));
                Assert.That(stats.Passes?.Key, Is.EqualTo(116));
                Assert.That(stats.Passes?.Accuracy, Is.Null);
                Assert.That(stats.Tackles?.Total, Is.EqualTo(76));
                Assert.That(stats.Tackles?.Blocks, Is.EqualTo(2));
                Assert.That(stats.Tackles?.Interceptions, Is.EqualTo(34));
                Assert.That(stats.Duels?.Total, Is.EqualTo(788));
                Assert.That(stats.Duels?.Won, Is.EqualTo(366));
                Assert.That(stats.Dribbles?.Attempts, Is.EqualTo(244));
                Assert.That(stats.Dribbles?.Success, Is.EqualTo(122));
                Assert.That(stats.Dribbles?.Past, Is.Null);
                Assert.That(stats.Fouls?.Drawn, Is.EqualTo(142));
                Assert.That(stats.Fouls?.Committed, Is.EqualTo(84));
                Assert.That(stats.Cards?.Yellowred, Is.EqualTo(0));
                Assert.That(stats.Penalty?.Won, Is.Null);
                Assert.That(stats.Penalty?.Committed, Is.Null);
                Assert.That(stats.Penalty?.Scored, Is.EqualTo(0));
                Assert.That(stats.Penalty?.Missed, Is.EqualTo(0));
                Assert.That(stats.Penalty?.Saved, Is.Null);
            });
        }
    }
}