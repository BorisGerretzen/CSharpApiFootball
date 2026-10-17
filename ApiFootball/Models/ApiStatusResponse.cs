namespace ApiFootball.Models;

/// <summary>Account and quota data returned by the status endpoint.</summary>
public sealed class ApiStatusResponse
{
    public ApiAccountStatus? Account { get; init; }
    public ApiSubscriptionStatus? Subscription { get; init; }
    public ApiRequestStatus? Requests { get; init; }
}