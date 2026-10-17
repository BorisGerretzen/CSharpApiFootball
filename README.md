# CSharpApiFootball

[![Test](https://github.com/BorisGerretzen/CSharpApiFootball/actions/workflows/test.yml/badge.svg?event=push)](https://github.com/BorisGerretzen/CSharpApiFootball/actions/workflows/test.yml)
[![NuGet](https://img.shields.io/nuget/v/ApiFootball.svg)](https://www.nuget.org/packages/ApiFootball/)

DI compatible C# API client for [api-football.com](https://www.api-football.com/) supporting all documented v3 endpoints
except widgets. If you find any issues or missing endpoints, please open an issue or submit a pull request.

This project is not affiliated with api-football or api-sports.

## Example

```Csharp
var key = "your api key";
var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(services => services.AddApiFootball(key))
    .Build();

var serviceScope = host.Services.CreateScope();
var provider = serviceScope.ServiceProvider;

// Print Dutch Eredivisie games and final scores for the 2022 season
var fixturesClient = provider.GetRequiredService<IFixturesClient>();
var fixtures = await fixturesClient.GetFixtures(league: 88, season: 2022);

// To throw an exception in case of errors
fixtures.EnsureSuccess();

foreach (var fixture in fixtures.Response)
{
    Console.WriteLine($"{fixture.Teams.Home.Name,20}  -  {fixture.Teams.Away.Name,-20} {" ",10} {fixture.Goals.Home} - {fixture.Goals.Away}");
}
```

## Configuration

Configure the library with the `AddApiFootball` options overload. The simpler `AddApiFootball(apiKey, apiUrl)` overload
sets just the API key and base URL.

```csharp
// Example, default options shown in table below
services.AddApiFootball(options =>
{
    options.ApiKey = key;
    options.ApiUrl = "https://v3.football.api-sports.io/";
    options.ThrowOnError = true; 
    options.RateLimiterOptions = ApiFootballRateLimiterOptions.ProPlan;
    options.ConfigureHttpClient = http => http.ConfigureHttpClient(client =>
        client.Timeout = TimeSpan.FromSeconds(30));
});
```

| Option                | Default                              | Description                                                                                                                                                   |
|-----------------------|--------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `ApiKey`              | Empty                                | Your API-Football key. Set this to authenticate requests.                                                                                                     |
| `ApiUrl`              | `https://v3.football.api-sports.io/` | Base URL, including a trailing slash. For RapidAPI, provide your RapidAPI base URL; the library selects the authentication headers based on the host.         |
| `ThrowOnError`        | `false`                              | Automatically throw `ApiFootballException` when the API response contains errors. When disabled, inspect `Errors` or call `EnsureSuccess()` on each response. |
| `RateLimiterOptions`  | `null`                               | Client-side request rate and concurrency limits. Set to `null` to disable rate limiting.                                                                      |
| `ConfigureHttpClient` | `null`                               | Optional `IHttpClientBuilder` configuration for the library's named `HttpClient`, such as setting its timeout or configuring Polly.                             |

### Rate limiter presets

`RateLimiterOptions` accepts one of four built-in presets. They control requests per second and minute, concurrent
requests, and the maximum number of requests waiting for a permit.
These rate limiters do not enforce daily quotas.

| Preset      | Requests per second | Requests per minute | Max concurrent requests | Max queued requests |
|-------------|--------------------:|--------------------:|------------------------:|--------------------:|
| `FreePlan`  |            No limit |                  10 |                      10 |                 100 |
| `ProPlan`   |                   5 |                 300 |                       5 |                 300 |
| `UltraPlan` |                   7 |                 450 |                       7 |                 450 |
| `MegaPlan`  |                  15 |                 900 |                      15 |                 900 |

You can also create your own instance of `ApiFootballRateLimiterOptions` to set custom limits, this table shows the
available options and their defaults.

| Rate limiter option     | Default | Description                                                                                                                            |
|-------------------------|--------:|----------------------------------------------------------------------------------------------------------------------------------------|
| `RequestsPerSecond`     |  `null` | Maximum requests in a one-second sliding window. Set to `null` for no per-second limit.                                                |
| `RequestsPerMinute`     |    `10` | Maximum requests in a one-minute sliding window. Set to `null` for no per-minute limit.                                                |
| `MaxConcurrentRequests` |  `null` | Maximum number of simultaneous requests. Set to `null` for no concurrency limit.                                                       |
| `MaxQueuedRequests`     |   `100` | Maximum number of waiting requests when a limit is reached. Set to `0` to allow no waiting requests, or `null` for an unbounded queue. |

The client-side limiter applies across the registered API-Football clients. API quota headers, when returned, are also
available on each response through `Quota`.

## Supported endpoints

Here you can find a list of all the endpoints that are currently supported and which interface to use to call them.

| Endpoint                  | C# method                                                                |
|---------------------------|--------------------------------------------------------------------------|
| `/coachs`                 | `ICoachesClient.GetCoaches`                                              |
| `/countries`              | `ICountriesClient.GetCountries`                                          |
| `/fixtures`               | `IFixturesClient.GetFixtures`                                            |
| `/fixtures/events`        | `IFixturesClient.GetFixtureEvents`                                       |
| `/fixtures/headtohead`    | `IFixturesClient.GetHeadToHead`                                          |
| `/fixtures/lineups`       | `IFixturesClient.GetFixtureLineups`                                      |
| `/fixtures/players`       | `IFixturesClient.GetFixturePlayers`                                      |
| `/fixtures/rounds`        | `IFixturesClient.GetRounds`, `GetRoundsWithDates`, `GetRoundsInTimezone` |
| `/fixtures/statistics`    | `IFixturesClient.GetFixtureStatistics`                                   |
| `/injuries`               | `IInjuriesClient.GetInjuries`                                            |
| `/leagues`                | `ILeaguesClient.GetLeagues`                                              |
| `/leagues/seasons`        | `ILeaguesClient.GetSeasons`                                              |
| `/odds`                   | `IOddsClient.GetOdds`                                                    |
| `/odds/bets`              | `IOddsClient.GetBets`                                                    |
| `/odds/bookmakers`        | `IOddsClient.GetBookmakers`                                              |
| `/odds/live`              | `IOddsClient.GetLiveOdds`                                                |
| `/odds/live/bets`         | `IOddsClient.GetLiveBets`                                                |
| `/odds/mapping`           | `IOddsClient.GetOddsMapping`                                             |
| `/players`                | `IPlayersClient.GetPlayerStatistics`                                     |
| `/players/profiles`       | `IPlayersClient.GetProfiles`                                             |
| `/players/seasons`        | `IPlayersClient.GetSeasons`                                              |
| `/players/squads`         | `IPlayersClient.GetSquads`                                               |
| `/players/teams`          | `IPlayersClient.GetTeams`                                                |
| `/players/topassists`     | `IPlayersClient.GetTopAssists`                                           |
| `/players/topredcards`    | `IPlayersClient.GetTopRedCards`                                          |
| `/players/topscorers`     | `IPlayersClient.GetTopScorers`                                           |
| `/players/topyellowcards` | `IPlayersClient.GetTopYellowCards`                                       |
| `/predictions`            | `IPredictionsClient.GetPredictions`                                      |
| `/sidelined`              | `ISidelinedClient.GetSidelined`                                          |
| `/standings`              | `IStandingsClient.GetStandings`                                          |
| `/status`                 | `IStatusClient.GetStatus`                                                |
| `/teams`                  | `ITeamsClient.GetTeamsInformation`                                       |
| `/teams/countries`        | `ITeamsClient.GetTeamsCountries`                                         |
| `/teams/seasons`          | `ITeamsClient.GetTeamsSeasons`                                           |
| `/teams/statistics`       | `ITeamsClient.GetTeamStatistics`                                         |
| `/timezone`               | `ITimezoneClient.GetTimezones`                                           |
| `/transfers`              | `ITransfersClient.GetTransfers`                                          |
| `/trophies`               | `ITrophiesClient.GetTrophies`                                            |
| `/venues`                 | `IVenuesClient.GetVenues`                                                |
