using System.Net;
using System.Text;
using System.Threading.Channels;

namespace ApiFootball.Test.RateLimiting;

internal sealed class ControlledHttpMessageHandler : HttpMessageHandler
{
    private readonly Channel<PendingRequest> _requests = Channel.CreateUnbounded<PendingRequest>();
    private int _activeRequests;
    private int _maximumConcurrentRequests;

    private int _requestsStarted;

    public int MaximumConcurrentRequests => Volatile.Read(ref _maximumConcurrentRequests);

    public int RequestsStarted => Volatile.Read(ref _requestsStarted);

    public async Task<PendingRequest> WaitForRequestAsync(CancellationToken cancellationToken = default)
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(5));

        try
        {
            return await _requests.Reader.ReadAsync(timeout.Token);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new TimeoutException("Timed out waiting for an HTTP request.");
        }
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var active = Interlocked.Increment(ref _activeRequests);

        try
        {
            UpdateMaximum(active);

            var pending = new PendingRequest(request.RequestUri?.AbsolutePath ?? string.Empty);

            await _requests.Writer.WriteAsync(pending, cancellationToken);
            Interlocked.Increment(ref _requestsStarted);

            return await pending.Response.Task.WaitAsync(cancellationToken);
        }
        finally
        {
            Interlocked.Decrement(ref _activeRequests);
        }
    }

    private void UpdateMaximum(int active)
    {
        while (true)
        {
            var maximum = Volatile.Read(ref _maximumConcurrentRequests);
            if (active <= maximum ||
                Interlocked.CompareExchange(ref _maximumConcurrentRequests, active, maximum) == maximum)
                return;
        }
    }
}

internal sealed class PendingRequest(string path)
{
    public string Path { get; } = path;

    public TaskCompletionSource<HttpResponseMessage> Response { get; } =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    public void Succeed(
        string responseJson =
            "{\"get\":\"test\",\"parameters\":[],\"errors\":[],\"results\":0,\"paging\":{\"current\":1,\"total\":1},\"response\":[]}")
    {
        Respond(HttpStatusCode.OK, responseJson);
    }

    public void Respond(HttpStatusCode statusCode,
        string responseJson =
            "{\"get\":\"test\",\"parameters\":[],\"errors\":[],\"results\":0,\"paging\":{\"current\":1,\"total\":1},\"response\":[]}")
    {
        Response.TrySetResult(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        });
    }

    public void Fail(Exception exception)
    {
        Response.TrySetException(exception);
    }
}