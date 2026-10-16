namespace ApiFootball.Clients.Interface;

public interface ISidelinedClient
{
    Task<PagedResponse<SidelinedRecord>> GetSidelined(int? player = null, IEnumerable<int>? players = null, int? coach = null, IEnumerable<int>? coachs = null,
        CancellationToken cancellationToken = default);
}
