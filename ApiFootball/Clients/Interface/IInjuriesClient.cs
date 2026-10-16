namespace ApiFootball.Clients.Interface;

public interface IInjuriesClient
{
    Task<PagedResponse<InjuryResponse>> GetInjuries(int? league = null, int? season = null, int? fixture = null, int? team = null, int? player = null,
        DateOnly? date = null, IEnumerable<int>? ids = null, string? timezone = null, CancellationToken cancellationToken = default);
}
