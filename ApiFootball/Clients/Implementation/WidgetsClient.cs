using ApiFootball.Options;
using Microsoft.Extensions.Options;

namespace ApiFootball.Clients.Implementation;

public sealed class WidgetsClient : BaseClient, IWidgetsClient
{
    private readonly ApiFootballOptions _options;

    public WidgetsClient(IHttpClientFactory factory, IOptions<ApiFootballOptions> options) : base(factory, options)
    {
        _options = options.Value;
    }

    protected override string Route => "widgets";

    public Task<string> GetGames(DateOnly? date = null, int? league = null, int? season = null, int? refresh = null, string? theme = null,
        bool? showToolbar = null, bool? showLogos = null, bool? modalGame = null, bool? modalStandings = null, bool? modalShowLogos = null,
        bool? showErrors = null, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString("/Games", WidgetBaseParameters(("data-date", date), ("data-league", league), ("data-season", season),
            ("data-refresh", refresh), ("data-theme", theme), ("data-show-toolbar", showToolbar), ("data-show-logos", showLogos),
            ("data-modal-game", modalGame), ("data-modal-standings", modalStandings), ("data-modal-show-logos", modalShowLogos),
            ("data-show-errors", showErrors)));
        return GetContentAsync(query, cancellationToken);
    }

    public Task<string> GetGame(int? id = null, int? refresh = null, string? theme = null, bool? showErrors = null, bool? showLogos = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString("/game", WidgetBaseParameters(("data-id", id), ("data-refresh", refresh), ("data-theme", theme),
            ("data-show-errors", showErrors), ("data-show-logos", showLogos)));
        return GetContentAsync(query, cancellationToken);
    }

    public Task<string> GetStandings(int season, int? league = null, int? team = null, string? theme = null, bool? showErrors = null, bool? showLogos = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString("/standings", WidgetBaseParameters(("data-league", league), ("data-team", team), ("data-season", season),
            ("data-theme", theme), ("data-show-errors", showErrors), ("data-show-logos", showLogos)));
        return GetContentAsync(query, cancellationToken);
    }

    private (string key, object? value)[] WidgetBaseParameters(params (string key, object? value)[] parameters)
    {
        var host = new Uri(_options.ApiUrl).Host;
        return [("data-host", host), ("data-key", _options.ApiKey), .. parameters];
    }
}
