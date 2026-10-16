namespace ApiFootball.Clients.Interface;

/// <summary>Client for the API-Football /coachs endpoint (the route uses this spelling).</summary>
public interface ICoachesClient
{
    Task<PagedResponse<CoachProfile>> GetCoachs(int? id = null, int? team = null, string? search = null, CancellationToken cancellationToken = default);
}
