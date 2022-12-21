using ApiFootball;
using ApiFootball.Clients.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var key = File.ReadAllText("key"); // put your api key in a file called 'key' 

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
result.Response.ForEach(Console.WriteLine);