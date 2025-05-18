using Moq;
using RichardSzalay.MockHttp;

namespace ApiFootball.Test;

public class BaseEndpointTest
{
    private const string ApiKey = "myapikey";

    protected static IHttpClientFactory MockFactory(string route, string expectedResponse)
    {
        var handlerMock = new MockHttpMessageHandler();
        handlerMock
            .When(HttpMethod.Get, $"{Globals.ApiUrl}{route}")
            .With(x =>
            {
                return
                    x.Headers.Any(h => h.Key == "x-rapidapi-key" && h.Value.First() == ApiKey) &&
                    x.Headers.Any(h => h.Key == "x-rapidapi-host" && h.Value.First() == Globals.ApiUrl);
            })
            .Respond("application/json", expectedResponse);
        var httpClient = new HttpClient(handlerMock)
        {
            DefaultRequestHeaders =
            {
                { "x-rapidapi-key", ApiKey },
                { "x-rapidapi-host", Globals.ApiUrl }
            },
            BaseAddress = new Uri(Globals.ApiUrl)
        };
        var factory = Mock.Of<IHttpClientFactory>();
        Mock.Get(factory).Setup(x => x.CreateClient(Globals.HttpClientName)).Returns(httpClient);
        return factory;
    }

    protected static string GetExpected(string method)
    {
        return File.ReadAllText($"Responses/{method}.json");
    }
}