using ApiFootball.Models.Responses;

namespace ApiFootball.Clients.Interface;

public interface ITimezoneClient {
    public Task<BaseResponse<string>> GetTimezones();
}