using ApiFootball.Models.Responses;
using ApiFootball.Models.Responses.Inner;

namespace ApiFootball.Clients.Interface;

public interface IFixturesClient {
    public Task<BaseResponse<string>> GetRounds(int league, int season, bool? current);

    public Task<BaseResponse<FixturesResponse>> GetFixtures(int? id = null, IEnumerable<string>? ids = null, string? live = null, DateOnly? date = null, int? league = null, int? season = null,
        int? team = null, int? last = null, int? next = null, DateOnly? from = null, DateOnly? to = null, string? round = null, string? status = null, int? venue = null, string? timezone = null);
}