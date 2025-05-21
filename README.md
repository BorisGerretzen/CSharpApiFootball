# CSharpApiFootball

[![Test](https://github.com/BorisGerretzen/CSharpApiFootball/actions/workflows/test.yml/badge.svg?event=push)](https://github.com/BorisGerretzen/CSharpApiFootball/actions/workflows/test.yml)
[![NuGet](https://img.shields.io/nuget/v/ApiFootball.svg)](https://www.nuget.org/packages/ApiFootball/)

DI compatible C# API client for [api-football.com](https://www.api-football.com/). Only v3 endpoints will be supported.
A list of availble endpoints can be found below, not a lot of them are there yet but this is a work in progress. It
should work with both api-sports and rapidapi keys, but because I do not have a rapidapi key I cannot test this.
If you would like to help out, feel free to open a pull request!

This project is not affiliated with api-football or api-sports.

## Example

```Csharp
var key = "your api key";
var host = Host
    .CreateDefaultBuilder(Array.Empty<string>())
    .ConfigureServices(services => {
        services
            .AddApiFootball(key); // Configure ApiFootball client
    }).Build();

var serviceScope = host.Services.CreateScope();
var provider = serviceScope.ServiceProvider;

// Print Dutch Eredivisie games and final scores for the 2022 season
var fixturesClient = provider.GetRequiredService<IFixturesClient>();
var fixtures = await fixturesClient.GetFixtures(league:88, season:2022);
fixtures.Response
    .Select(fix => $"{fix.Teams.Home.Name, 20}  -  {fix.Teams.Away.Name, -20} {" ", 10} {fix.Goals.Home} - {fix.Goals.Away}")
    .ToList()
    .ForEach(Console.WriteLine);
    
// Print all timezones
var timezoneClient = provider.GetRequiredService<ITimezoneClient>();
var timezones = await timezoneClient.GetTimezones();

// To throw an exception in case of errors
timezones.EnsureSuccess();

timezones.Response.ForEach(Console.WriteLine);
```

> [!IMPORTANT]  
> Free API keys are limited to 10 requests per minute at the time of writing.
> Use the `EnsureSuccess()` method to throw an `ApiFootballException` in case of errors or handle them yourself through
> the `Errors` object on the response.

## Supported endpoints

- /countries
- /fixtures
- /fixtures/rounds
- /leagues
- /leagues/seasons
- /standings
- /teams
- /teams/seasons
- /teams/countries
- /timezone
- /venues

