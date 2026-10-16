using ApiFootball.Options;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;

namespace ApiFootball.Test;

public class BaseEndpointTest
{
    private const string ApiKey = "myapikey";
    protected const string DefaultErrorResponse = "DefaultErrorResponse";

    protected static IHttpClientFactory MockFactory(string route, string expectedResponse)
    {
        var handlerMock = new MockHttpMessageHandler();
        var expectedPathAndQuery = new Uri($"{ApiFootballGlobals.ApiUrl}{route}").PathAndQuery;
        var assertQuery = route.Contains('?', StringComparison.Ordinal);
        handlerMock
            .When(HttpMethod.Get, $"{ApiFootballGlobals.ApiUrl}{route}")
            .With(x =>
            {
                return (!assertQuery || x.RequestUri?.PathAndQuery == expectedPathAndQuery) &&
                       x.Headers.Any(h => h.Key == "x-apisports-key" && h.Value.First() == ApiKey);
            })
            .Respond("application/json", expectedResponse);
        var httpClient = new HttpClient(handlerMock)
        {
            DefaultRequestHeaders =
            {
                { "x-apisports-key", ApiKey }
            },
            BaseAddress = new Uri(ApiFootballGlobals.ApiUrl)
        };
        var factory = Mock.Of<IHttpClientFactory>();
        Mock.Get(factory).Setup(x => x.CreateClient(ApiFootballGlobals.HttpClientName)).Returns(httpClient);
        return factory;
    }

    protected static IOptions<ApiFootballOptions> MockOptions(Action<ApiFootballOptions>? configure = null)
    {
        var options = new ApiFootballOptions();
        configure?.Invoke(options);
        var optionsMock = new Mock<IOptions<ApiFootballOptions>>();
        optionsMock.Setup(x => x.Value).Returns(options);
        return optionsMock.Object;
    }

    protected static string GetExpected(string method)
    {
        return File.ReadAllText($"Responses/{method}.json");
    }

    protected void AssertDefaultException(ApiFootballException? exception)
    {
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception!.Message, Is.Not.Empty);
        Assert.That(exception!.Errors, Has.Count.EqualTo(1));
        var error = exception.Errors.First();
        Assert.That(error.Code, Is.EqualTo("search"));
        Assert.That(error.Message, Is.EqualTo("The Search field must be at least 3 characters in length."));
    }
}
