using System.Net;
using ApiFootball.Clients.Interface;
using ApiFootball.Models;
using ApiFootball.Models.Responses;
using ApiFootball.Options;
using Microsoft.Extensions.DependencyInjection;

namespace ApiFootball.Test.RateLimiting;

[Parallelizable(ParallelScope.All)]
public sealed class RateLimitingIntegrationTests
{
    private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(5);
    
    [TestCase(true)]
    [TestCase(false)]
    public async Task DisabledOrEmptyLimiter_AllowsConcurrentRequests(bool disableLimiter)
    {
        await using var host = CreateHost(disableLimiter ? null : _ => { });
        var calls = Enumerable.Range(0, 3)
            .Select(_ => StartTimezones(host))
            .ToArray();
        var pending = await Task.WhenAll(Enumerable.Range(0, calls.Length).Select(_ => host.Handler.WaitForRequestAsync())).WaitAsync(TestTimeout);

        Assert.That(host.Handler.MaximumConcurrentRequests, Is.EqualTo(3));

        foreach (var request in pending)
            request.Succeed();
        await Task.WhenAll(calls.Select(call => call.Task)).WaitAsync(TestTimeout);
    }

    [Test]
    public async Task ConcurrencyLimit_QueuesRequestsAndNeverExceedsConfiguredMaximum()
    {
        await using var host = CreateHost(options => options.MaxConcurrentRequests = 2);
        var calls = Enumerable.Range(0, 5).Select(_ => StartTimezones(host)).ToArray();
        var active = new List<PendingRequest>
        {
            await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout),
            await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout)
        };

        Assert.That(host.Handler.RequestsStarted, Is.EqualTo(2));

        for (var i = 0; i < calls.Length; i++)
        {
            active[i % 2].Succeed();
            if (i < calls.Length - 2)
                active[i % 2] = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        }

        await Task.WhenAll(calls.Select(call => call.Task)).WaitAsync(TestTimeout);
        Assert.That(host.Handler.MaximumConcurrentRequests, Is.LessThanOrEqualTo(2));
    }

    [Test]
    public async Task QueueLimit_RejectsRequestWhenQueueIsFull()
    {
        await using var host = CreateHost(options =>
        {
            options.MaxConcurrentRequests = 1;
            options.MaxQueuedRequests = 1;
        });
        var first = StartTimezones(host);
        var active = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        var queued = StartCountries(host);
        await queued.InvocationReturned.WaitAsync(TestTimeout);
        var rejected = StartTimezones(host);
        await rejected.InvocationReturned.WaitAsync(TestTimeout);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await rejected.Task);
        Assert.That(exception!.Message, Is.EqualTo("API-Football rate limit exceeded and request queue is full."));
        Assert.That(host.Handler.RequestsStarted, Is.EqualTo(1));

        active.Succeed();
        var queuedRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        Assert.That(queuedRequest.Path, Is.EqualTo("/countries"));
        queuedRequest.Succeed();
        await Task.WhenAll(first.Task, queued.Task).WaitAsync(TestTimeout);
    }

    [Test]
    public async Task ZeroQueueLimit_RejectsAnyRequestBeyondConcurrencyLimit()
    {
        await using var host = CreateHost(options =>
        {
            options.MaxConcurrentRequests = 1;
            options.MaxQueuedRequests = 0;
        });
        var first = StartTimezones(host);
        var active = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        var rejected = StartCountries(host);

        Assert.That(await Assert.ThrowsAsync<InvalidOperationException>(async () => await rejected.Task), Is.Not.Null);
        Assert.That(host.Handler.RequestsStarted, Is.EqualTo(1));
        active.Succeed();
        await first.Task.WaitAsync(TestTimeout);
    }

    [Test]
    public async Task NullQueueLimit_AllowsSeveralRequestsToWait()
    {
        await using var host = CreateHost(options =>
        {
            options.MaxConcurrentRequests = 1;
            options.MaxQueuedRequests = null;
        });
        var first = StartTimezones(host);
        var active = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        var queuedCountries = StartCountries(host);
        await queuedCountries.InvocationReturned.WaitAsync(TestTimeout);
        var queuedTimezone = StartTimezones(host);
        await queuedTimezone.InvocationReturned.WaitAsync(TestTimeout);
        var queuedCountriesAgain = StartCountries(host);
        await queuedCountriesAgain.InvocationReturned.WaitAsync(TestTimeout);
        var queued = new Task[] { queuedCountries.Task, queuedTimezone.Task, queuedCountriesAgain.Task };

        active.Succeed();
        foreach (var _ in queued)
        {
            var request = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
            request.Succeed();
        }

        await Task.WhenAll(queued.Append(first.Task)).WaitAsync(TestTimeout);
    }

    [Test]
    public async Task QueuedRequests_AreForwardedInFifoOrder()
    {
        await using var host = CreateHost(options =>
        {
            options.MaxConcurrentRequests = 1;
            options.MaxQueuedRequests = 3;
        });
        var first = StartTimezones(host);
        var firstRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        var queuedCountries = StartCountries(host);
        await queuedCountries.InvocationReturned.WaitAsync(TestTimeout);
        var queuedTimezone = StartTimezones(host);
        await queuedTimezone.InvocationReturned.WaitAsync(TestTimeout);
        var queuedCountriesAgain = StartCountries(host);
        await queuedCountriesAgain.InvocationReturned.WaitAsync(TestTimeout);
        var queued = new Task[] { queuedCountries.Task, queuedTimezone.Task, queuedCountriesAgain.Task };

        firstRequest.Succeed();
        var expectedPaths = new[] { "/countries", "/timezone", "/countries" };
        foreach (var expectedPath in expectedPaths)
        {
            var request = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
            Assert.That(request.Path, Is.EqualTo(expectedPath));
            request.Succeed();
        }

        await Task.WhenAll(queued.Append(first.Task)).WaitAsync(TestTimeout);
    }

    [Test]
    public async Task CancellingQueuedRequest_ReleasesQueueSlotForNextRequest()
    {
        await using var host = CreateHost(options =>
        {
            options.MaxConcurrentRequests = 1;
            options.MaxQueuedRequests = 1;
        });
        var first = StartTimezones(host);
        var active = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        using var cancellation = new CancellationTokenSource();
        var cancelled = StartCountries(host, cancellation.Token);
        await cancelled.InvocationReturned.WaitAsync(TestTimeout);
        cancellation.Cancel();
        Assert.That(await Assert.CatchAsync<OperationCanceledException>(async () => await cancelled.Task), Is.Not.Null);

        var replacement = StartCountries(host);
        await replacement.InvocationReturned.WaitAsync(TestTimeout);
        active.Succeed();
        var replacementRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        replacementRequest.Succeed();
        await Task.WhenAll(first.Task, replacement.Task).WaitAsync(TestTimeout);
    }

    [Test]
    public async Task CancellingActiveRequest_ReleasesConcurrencyPermit()
    {
        await using var host = CreateHost(options =>
        {
            options.MaxConcurrentRequests = 1;
            options.MaxQueuedRequests = 1;
        });
        using var cancellation = new CancellationTokenSource();
        var activeCall = StartTimezones(host, cancellation.Token);
        _ = await host.Handler.WaitForRequestAsync(cancellation.Token).WaitAsync(TestTimeout);
        var queued = StartCountries(host);
        await queued.InvocationReturned.WaitAsync(TestTimeout);
        cancellation.Cancel();
        
        var exception = await Assert.CatchAsync<AggregateException>(async () => await activeCall.Task);
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.InnerExceptions, Has.Exactly(1).InstanceOf<OperationCanceledException>());

        var nextRequest = await host.Handler.WaitForRequestAsync(CancellationToken.None).WaitAsync(TestTimeout);
        Assert.That(nextRequest.Path, Is.EqualTo("/countries"));
        nextRequest.Succeed();
        await queued.Task.WaitAsync(TestTimeout);
    }

    [Test]
    public async Task TransportFailure_ReleasesConcurrencyPermit()
    {
        await using var host = CreateHost(options =>
        {
            options.MaxConcurrentRequests = 1;
            options.MaxQueuedRequests = 1;
        });
        var failed = StartTimezones(host);
        var failingRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        var next = StartCountries(host);
        await next.InvocationReturned.WaitAsync(TestTimeout);
        failingRequest.Fail(new HttpRequestException("Simulated transport failure."));

        var exception = await Assert.CatchAsync<AggregateException>(async () => await failed.Task);
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.InnerExceptions, Has.Exactly(1).InstanceOf<HttpRequestException>());
        var succeedingRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        succeedingRequest.Succeed();
        await next.Task.WaitAsync(TestTimeout);
    }

    [Test]
    public async Task HttpFailure_ReleasesConcurrencyPermit()
    {
        await using var host = CreateHost(options =>
        {
            options.MaxConcurrentRequests = 1;
            options.MaxQueuedRequests = 1;
        });
        var failed = StartTimezones(host);
        var failingRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        var next = StartCountries(host);
        await next.InvocationReturned.WaitAsync(TestTimeout);
        failingRequest.Respond(HttpStatusCode.InternalServerError);

        Assert.That(await Assert.CatchAsync<HttpRequestException>(async () => await failed.Task), Is.Not.Null);
        var succeedingRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        succeedingRequest.Succeed();
        await next.Task.WaitAsync(TestTimeout);
    }

    [Test]
    public async Task MinuteLimit_RejectsRequestsBeyondConfiguredAllowance()
    {
        await using var host = CreateHost(options =>
        {
            options.RequestsPerMinute = 2;
            options.MaxQueuedRequests = 0;
        });
        var first = StartTimezones(host);
        var second = StartCountries(host);
        var activeRequests = await Task.WhenAll(
            host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout),
            host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout)).WaitAsync(TestTimeout);
        var rejected = StartTimezones(host);

        Assert.That(await Assert.ThrowsAsync<InvalidOperationException>(async () => await rejected.Task), Is.Not.Null);
        Assert.That(host.Handler.RequestsStarted, Is.EqualTo(2));
        foreach (var request in activeRequests)
            request.Succeed();
        await Task.WhenAll(first.Task, second.Task).WaitAsync(TestTimeout);
    }

    [Test]
    public async Task SecondLimit_QueuesUntilCapacityIsReplenished()
    {
        await using var host = CreateHost(options =>
        {
            options.RequestsPerSecond = 1;
            options.MaxQueuedRequests = 1;
        });

        // Consume the available permit.
        var first = StartTimezones(host);
        var firstRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        firstRequest.Succeed();
        await first.Task.WaitAsync(TestTimeout);

        Assert.That(host.Handler.RequestsStarted, Is.EqualTo(1));

        // Start another request while the permit is exhausted.
        var queued = StartCountries(host);
        await queued.InvocationReturned.WaitAsync(TestTimeout);

        // The second request should not have reached the HTTP handler yet.
        Assert.That(host.Handler.RequestsStarted, Is.EqualTo(1),
            "The second request should be queued until the rate limit replenishes.");

        // Once replenished, the queued request should proceed.
        var secondRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);

        Assert.Multiple(() =>
        {
            Assert.That(secondRequest.Path, Is.EqualTo("/countries"));
            Assert.That(host.Handler.RequestsStarted, Is.EqualTo(2));
        });

        secondRequest.Succeed();
        await queued.Task.WaitAsync(TestTimeout);
    }
    
    [Test]
    public async Task ChainedLimits_ReleaseConcurrencyWhenTimeLimitRejectsRequest()
    {
        await using var host = CreateHost(options =>
        {
            options.RequestsPerSecond = 1;
            options.MaxConcurrentRequests = 1;
            options.MaxQueuedRequests = 0;
        });
        var first = StartTimezones(host);
        var request = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        request.Succeed();
        await first.Task.WaitAsync(TestTimeout);
        var rejectedFirst = StartCountries(host);
        Assert.That(await Assert.ThrowsAsync<InvalidOperationException>(async () => await rejectedFirst.Task), Is.Not.Null);
        Assert.That(host.Handler.RequestsStarted, Is.EqualTo(1));

        await Task.Delay(TimeSpan.FromSeconds(1.1));
        var acceptedSecond = StartTimezones(host);
        var secondRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        secondRequest.Succeed();
        await acceptedSecond.Task;
        
        Assert.That(host.Handler.RequestsStarted, Is.EqualTo(2));
    }

    [Test]
    public async Task ChainedLimits_QueuedRequestsEventuallyPassBothConcurrencyAndSecondLimits()
    {
        await using var host = CreateHost(options =>
        {
            options.RequestsPerSecond = 1;
            options.MaxConcurrentRequests = 1;
            options.MaxQueuedRequests = 2;
        });
        var first = StartTimezones(host);
        var firstRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        var second = StartCountries(host);
        await second.InvocationReturned.WaitAsync(TestTimeout);
        var third = StartTimezones(host);
        await third.InvocationReturned.WaitAsync(TestTimeout);

        firstRequest.Succeed();
        await first.Task.WaitAsync(TestTimeout);
        var secondRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        secondRequest.Succeed();
        await second.Task.WaitAsync(TestTimeout);
        var thirdRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        thirdRequest.Succeed();
        await third.Task.WaitAsync(TestTimeout);

        Assert.That(host.Handler.MaximumConcurrentRequests, Is.EqualTo(1));
    }

    [Test]
    public async Task EndpointClientsShareLimiterState()
    {
        await using var host = CreateHost(options =>
        {
            options.MaxConcurrentRequests = 1;
            options.MaxQueuedRequests = 1;
        });
        var timezone = StartTimezones(host);
        var active = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        var countries = StartCountries(host);
        await countries.InvocationReturned.WaitAsync(TestTimeout);
        Assert.That(host.Handler.RequestsStarted, Is.EqualTo(1));

        active.Succeed();
        var nextRequest = await host.Handler.WaitForRequestAsync().WaitAsync(TestTimeout);
        Assert.That(nextRequest.Path, Is.EqualTo("/countries"));
        nextRequest.Succeed();
        await Task.WhenAll(timezone.Task, countries.Task).WaitAsync(TestTimeout);
    }

    [TestCase("RequestsPerMinute")]
    [TestCase("RequestsPerSecond")]
    [TestCase("MaxConcurrentRequests")]
    [TestCase("MaxQueuedRequests")]
    public void InvalidLimiterOptions_ThrowWhenClientPipelineIsCreated(string optionName)
    {
        using var host = CreateHost(options =>
        {
            switch (optionName)
            {
                case "RequestsPerMinute": options.RequestsPerMinute = 0; break;
                case "RequestsPerSecond": options.RequestsPerSecond = -1; break;
                case "MaxConcurrentRequests": options.MaxConcurrentRequests = 0; break;
                case "MaxQueuedRequests": options.MaxQueuedRequests = -1; break;
            }
        });

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => host.Get<ITimezoneClient>());
        Assert.That(exception!.ParamName, Is.EqualTo(optionName));
    }

    private static TestHost CreateHost(Action<ApiFootballRateLimiterOptions>? configure = null)
    {
        var services = new ServiceCollection();
        var handler = new ControlledHttpMessageHandler();
        var options = new ApiFootballRateLimiterOptions
        {
            RequestsPerMinute = null,
            RequestsPerSecond = null,
            MaxConcurrentRequests = null,
            MaxQueuedRequests = null
        };
        configure?.Invoke(options);

        services.AddApiFootball(apiOptions =>
        {
            apiOptions.ApiKey = "test-key";
            apiOptions.ApiUrl = "https://api-football.test/";
            apiOptions.RateLimiterOptions = configure is null ? null : options;
        }).ConfigurePrimaryHttpMessageHandler(() => handler);

        return new TestHost(services.BuildServiceProvider(), handler);
    }

    private static StartedRequest<PagedResponse<string>> StartTimezones(TestHost host, CancellationToken cancellationToken = default)
    {
        var client = host.Get<ITimezoneClient>();
        return host.Start(client.GetTimezones, cancellationToken);
    }

    private static StartedRequest<PagedResponse<Country>> StartCountries(TestHost host, CancellationToken cancellationToken = default)
    {
        var client = host.Get<ICountriesClient>();
        return host.Start(token => client.GetCountries(cancellationToken: token), cancellationToken);
    }

    private sealed class TestHost(ServiceProvider services, ControlledHttpMessageHandler handler) : IAsyncDisposable, IDisposable
    {
        private readonly CancellationTokenSource _shutdown = new();
        private readonly List<Task> _operations = [];
        private readonly object _lock = new();

        public ControlledHttpMessageHandler Handler { get; } = handler;

        public T Get<T>() where T : notnull => services.GetRequiredService<T>();

        public StartedRequest<T> Start<T>(Func<CancellationToken, Task<T>> request, CancellationToken cancellationToken = default)
        {
            var linkedCancellation = CancellationTokenSource.CreateLinkedTokenSource(_shutdown.Token, cancellationToken);
            var invocationReturned = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var operation = Task.Run(async () =>
            {
                try
                {
                    var requestTask = request(linkedCancellation.Token);
                    invocationReturned.TrySetResult();
                    return await requestTask;
                }
                catch
                {
                    invocationReturned.TrySetResult();
                    throw;
                }
                finally
                {
                    linkedCancellation.Dispose();
                }
            }, CancellationToken.None);

            lock (_lock)
                _operations.Add(operation);

            return new StartedRequest<T>(operation, invocationReturned.Task);
        }

        public void Dispose()
        {
            _shutdown.Cancel();
            services.Dispose();
            _shutdown.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            _shutdown.Cancel();
            Task[] operations;
            lock (_lock)
                operations = _operations.ToArray();

            try
            {
                await Task.WhenAll(operations).WaitAsync(TimeSpan.FromSeconds(5));
            }
            catch
            {
                // Cleanup must continue after expected cancellation or a failed assertion.
            }

            services.Dispose();
            _shutdown.Dispose();
        }
    }

    private sealed record StartedRequest<T>(Task<T> Task, Task InvocationReturned);
}
