using ApiFootball.Clients;
using ApiFootball.Models.Responses;
using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Test.Clients;

public class BaseClientTests : BaseEndpointTest
{
    [Test]
    public async Task GetErrorAsync_PagedResponse_ReturnsErrors()
    {
        var factory = MockFactory("test/error", GetExpected(DefaultErrorResponse));
        var client = new TestClient(factory, MockOptions(o => o.ThrowOnError = true));
        var exception =
            await Assert.ThrowsAsync<ApiFootballException>(async () =>
                await client.GetErrorAsync<PagedResponse<string>>());
        AssertDefaultException(exception);
    }

    [Test]
    public async Task GetErrorAsync_ObjectResponse_ReturnsErrors()
    {
        var factory = MockFactory("test/error", GetExpected(DefaultErrorResponse));
        var client = new TestClient(factory, MockOptions(o => o.ThrowOnError = true));
        var exception =
            await Assert.ThrowsAsync<ApiFootballException>(async () =>
                await client.GetErrorAsync<ObjectResponse<string>>());
        AssertDefaultException(exception);
    }

    private sealed class TestClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options)
        : BaseClient(factory, options)
    {
        protected override string Route => "test";

        public async Task<T> GetErrorAsync<T>()
            where T : BaseResponse
        {
            var url = BuildQueryString("/error");
            var res = await GetAsync<T>(url);
            return res;
        }
    }
}