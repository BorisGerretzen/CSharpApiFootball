namespace ApiFootball.Clients.Interface;

/// <summary>Calls the three HTML widget routes documented by API-Football.</summary>
public interface IWidgetsClient
{
    Task<string> GetGames(DateOnly? date = null, int? league = null, int? season = null, int? refresh = null, string? theme = null,
        bool? showToolbar = null, bool? showLogos = null, bool? modalGame = null, bool? modalStandings = null, bool? modalShowLogos = null,
        bool? showErrors = null, CancellationToken cancellationToken = default);
    Task<string> GetGame(int? id = null, int? refresh = null, string? theme = null, bool? showErrors = null, bool? showLogos = null,
        CancellationToken cancellationToken = default);
    Task<string> GetStandings(int season, int? league = null, int? team = null, string? theme = null, bool? showErrors = null, bool? showLogos = null,
        CancellationToken cancellationToken = default);
}
