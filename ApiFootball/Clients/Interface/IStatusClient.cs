namespace ApiFootball.Clients.Interface;

/// <summary>Client for the API account status endpoint.</summary>
public interface IStatusClient
{
    /// <summary>Gets the API account, subscription, and request quota status.</summary>
    Task<ObjectResponse<ApiStatusResponse>> GetStatus(CancellationToken cancellationToken = default);
}