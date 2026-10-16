using ApiFootball.Clients.Implementation;

namespace ApiFootball.Test.Clients.Implementation;

[TestFixture]
public sealed class WidgetsClientTests : BaseEndpointTest
{
    private const string Html = "<div class=\"api-football-widget\"></div>";

    [Test]
    public async Task GetGames_BuildsDocumentedWidgetAttributes()
    {
        var route = "widgets/Games?data-host=v3.football.api-sports.io&data-key=myapikey&data-date=2026-10-17&data-league=39&data-season=2026&data-refresh=15&data-theme=dark&data-show-toolbar=true&data-show-logos=false&data-modal-game=true&data-modal-standings=false&data-modal-show-logos=true&data-show-errors=true";
        var factory = MockFactory(route, Html);
        var response = await new WidgetsClient(factory, MockOptions(options => options.ApiKey = "myapikey"))
            .GetGames(new DateOnly(2026, 10, 17), 39, 2026, 15, "dark", true, false, true, false, true, true);

        Assert.That(response, Is.EqualTo(Html));
    }

    [Test]
    public async Task GetGame_BuildsDocumentedWidgetAttributes()
    {
        const string route = "widgets/game?data-host=v3.football.api-sports.io&data-key=myapikey&data-id=1557430&data-refresh=15&data-theme=dark&data-show-errors=true&data-show-logos=false";
        var factory = MockFactory(route, Html);
        var response = await new WidgetsClient(factory, MockOptions(options => options.ApiKey = "myapikey"))
            .GetGame(1557430, 15, "dark", true, false);

        Assert.That(response, Is.EqualTo(Html));
    }

    [Test]
    public async Task GetStandings_BuildsDocumentedWidgetAttributes()
    {
        const string route = "widgets/standings?data-host=v3.football.api-sports.io&data-key=myapikey&data-league=39&data-team=40&data-season=2024&data-theme=dark&data-show-errors=true&data-show-logos=false";
        var factory = MockFactory(route, Html);
        var response = await new WidgetsClient(factory, MockOptions(options => options.ApiKey = "myapikey"))
            .GetStandings(2024, 39, 40, "dark", true, false);

        Assert.That(response, Is.EqualTo(Html));
    }
}
