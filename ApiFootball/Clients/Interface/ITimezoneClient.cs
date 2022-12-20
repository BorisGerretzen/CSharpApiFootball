using ApiFootball.Models.Responses.Inner;
using ApiFootball.Models.Responses.Outer;

namespace ApiFootball;

public interface ITimezoneClient
{
    public Task<BaseResponse<string>> GetTimezones();
}