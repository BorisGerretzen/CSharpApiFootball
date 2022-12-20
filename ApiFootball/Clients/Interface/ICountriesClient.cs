using ApiFootball.Models.Responses.Outer;

namespace ApiFootball;

public interface ICountriesClient
{
    public Task<BaseResponse<string>> GetCountries(string? name, string? code, string? search);
}