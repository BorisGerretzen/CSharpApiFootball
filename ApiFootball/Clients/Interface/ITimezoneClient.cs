namespace ApiFootball.Clients.Interface;

public interface ITimezoneClient
{
    /// <summary>
    /// Get the list of available timezones to be used in the fixtures endpoint.
    /// Recommended Calls: 1 call when you need.
    /// </summary>
    public Task<BaseResponse<string>> GetTimezones(CancellationToken cancellationToken = default);
}