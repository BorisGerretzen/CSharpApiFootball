using ApiFootball.Models;
using ApiFootball.Models.Responses;

namespace ApiFootball.Clients.Interface;

public interface ICountriesClient {
    public Task<BaseResponse<Country>> GetCountries(string? name = null, string? code = null, string? search = null);
}