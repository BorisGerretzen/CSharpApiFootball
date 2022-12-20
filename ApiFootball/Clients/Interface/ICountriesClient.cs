using ApiFootball.Models.Responses;

namespace ApiFootball.Clients.Interface;

public interface ICountriesClient {
    public Task<BaseResponse<string>> GetCountries(string? name, string? code, string? search);
}