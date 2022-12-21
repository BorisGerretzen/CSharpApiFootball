using ApiFootball.Models;
using ApiFootball.Models.Responses;
using ApiFootball.Models.Responses.Inner;

namespace ApiFootball.Clients.Interface;

public interface ITeamsClient
{
    public Task<BaseResponse<TeamInformationResponse>> GetTeamsInformation(int? id = null, string? name = null, int? league = null, int? season = null, string? country = null, string? code = null, int? venue = null, string? search = null);
    public Task<BaseResponse<Season>> GetTeamsSeasons(int team);
    public Task<BaseResponse<Country>> GetTeamsCountries();
}