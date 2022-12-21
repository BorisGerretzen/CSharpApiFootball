using ApiFootball.Models;
using ApiFootball.Models.Responses;
using ApiFootball.Models.Responses.Inner;

namespace ApiFootball.Clients.Interface;

public interface ILeaguesClient {
    public Task<BaseResponse<LeaguesResponse>> GetLeagues(int? id = null, string? name = null, string? country = null, string? code = null, int? season = null, int? team = null,
        LeagueType? type = null, bool? current = null, string? search = null, int? last = null);

    public Task<BaseResponse<int>> GetSeasons();
}