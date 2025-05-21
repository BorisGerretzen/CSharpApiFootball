namespace ApiFootball.IntegrationTests;

public class RateLimitingHandler : DelegatingHandler
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await Semaphore.WaitAsync(cancellationToken);
        try
        {
            var startTime = DateTime.UtcNow;
            var response = await base.SendAsync(request, cancellationToken);
            var timeSpent = DateTime.UtcNow - startTime;
            var waitTime = TimeSpan.FromMilliseconds(6100) - timeSpent;
            if (waitTime > TimeSpan.Zero) await Task.Delay(waitTime, cancellationToken);

            return response;
        }
        finally
        {
            Semaphore.Release();
        }
    }
}