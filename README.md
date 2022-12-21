# CSharpApiFootball
[![Test](https://github.com/BorisGerretzen/CSharpApiFootball/actions/workflows/test.yml/badge.svg?event=push)](https://github.com/BorisGerretzen/CSharpApiFootball/actions/workflows/test.yml)
[![NuGet](https://img.shields.io/nuget/v/ApiFootball.svg)](https://www.nuget.org/packages/ApiFootball/)

DI compatible C# API client for [api-football.com](https://www.api-football.com/). Only v3 endpoints will be supported. A list of availble endpoints can be found below, not a lot of them are there yet but this is a work in progress.
If you would like to help out, feel free to open a pull request!

## Example
```Csharp
var host = Host
    .CreateDefaultBuilder(Array.Empty<string>())
    .ConfigureServices(services => {
        services
            .AddApiFootball(key); // Configure ApiFootball clients
    }).Build();

var serviceScope = host.Services.CreateScope();
var provider = serviceScope.ServiceProvider;
var client = provider.GetRequiredService<ITimezoneClient>();
var result = await client.GetTimezones();

```

## Supported endpoints
- /countries
- /fixtures
- /fixtures/rounds
- /leagues
- /leagues/seasons
- /timezone

## Todo 
- More endpoints
