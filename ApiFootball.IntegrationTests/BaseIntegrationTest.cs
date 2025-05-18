using Microsoft.Extensions.Configuration;

namespace ApiFootball.IntegrationTests;

public abstract class BaseIntegrationTest
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
        if (string.IsNullOrEmpty(apiKey)) throw new InvalidOperationException("API key is not set. Please set the ApiFootball:ApiKey in the environment variables or appsettings.development.json.");

        var services = new ServiceCollection();
        services.AddApiFootball(apiKey);
        ServiceProvider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        if (ServiceProvider is IDisposable disposable) disposable.Dispose();
    }
}