using ApiFootball.Models;
using ApiFootball.Models.Responses;
using ApiFootball.Models.Responses.Inner;

namespace ApiFootball.Clients.Interface;
public interface IStandingsClient
{
    Task<BaseResponse<StandingsResponse>> GetStandings(
        int season,
        int? league = null,
        int? team = null
    );
}
