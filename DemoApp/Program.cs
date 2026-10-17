using ApiFootball;
using ApiFootball.Clients.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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