using ApiFootball;
using ApiFootball.Clients.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var key = File.ReadAllText("key"); // put your api key in a file called 'key' 

var host = Host
    .CreateDefaultBuilder(Array.Empty<string>())
    .ConfigureServices(services =>
    {
        services
            .AddApiFootball(key); // Configure ApiFootball clients
    }).Build();

var serviceScope = host.Services.CreateScope();
var provider = serviceScope.ServiceProvider;

// Print Dutch Eredivisie games and final scores for the 2022 season
var fixturesClient = provider.GetRequiredService<IFixturesClient>();
var fixtures = await fixturesClient.GetFixtures(league: 88, season: 2022);
fixtures.Response
    .Select(fix => $"{fix.Teams.Home.Name,20}  -  {fix.Teams.Away.Name,-20} {" ",10} {fix.Goals.Home} - {fix.Goals.Away}")
    .ToList()
    .ForEach(Console.WriteLine);

// Print all timezones
var timezoneClient = provider.GetRequiredService<ITimezoneClient>();
var timezones = await timezoneClient.GetTimezones();
timezones.Response.ForEach(Console.WriteLine);