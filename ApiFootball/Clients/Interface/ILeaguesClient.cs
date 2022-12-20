using ApiFootball.Models;
using ApiFootball.Models.Responses;
using ApiFootball.Models.Responses.Inner;

namespace ApiFootball.Clients.Interface;

public interface ILeaguesClient {
    public Task<BaseResponse<LeaguesResponse>> GetLeagues(int? id, string? name, string? country, string? code, int? season, int? team, LeagueType? type, bool? current, string? search, int? last);
    public Task<BaseResponse<int>> GetSeasons();
}