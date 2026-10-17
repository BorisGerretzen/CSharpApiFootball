using ApiFootball.Options;
using Microsoft.Extensions.Configuration;

namespace ApiFootball.IntegrationTests;

public abstract class BaseLiveTest
{
    protected IServiceProvider ServiceProvider { get; private set; } = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.development.json", true)
            .Build();
        var apiKey = configuration["ApiFootball:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
            throw new InvalidOperationException(
                "API key is not set. Please set the ApiFootball:ApiKey in the environment variables or appsettings.development.json.");

        var services = new ServiceCollection();
        services.AddApiFootball(o =>
        {
            o.ApiKey = apiKey;
            o.ThrowOnError = true;
            o.RateLimiterOptions = ApiFootballRateLimiterOptions.FreePlan;
        });
        ServiceProvider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        if (ServiceProvider is IDisposable disposable) disposable.Dispose();
    }
}