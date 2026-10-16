namespace ApiFootball.Clients.Interface;

public interface ITrophiesClient
{
    Task<PagedResponse<Trophy>> GetTrophies(int? player = null, IEnumerable<int>? players = null, int? coach = null, IEnumerable<int>? coachs = null,
        CancellationToken cancellationToken = default);
}
