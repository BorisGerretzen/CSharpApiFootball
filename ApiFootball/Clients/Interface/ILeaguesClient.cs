using ApiFootball.Models;
using ApiFootball.Models.Responses.Inner;
using ApiFootball.Models.Responses.Outer;

namespace ApiFootball;

public interface ILeaguesClient
{
    public Task<BaseResponse<LeaguesResponse>> GetLeagues(int? id, string? name, string? country, string? code, int? season, int? team, LeagueType? type, bool? current, string? search, int? last);
    public Task<BaseResponse<int>> GetSeasons();
}