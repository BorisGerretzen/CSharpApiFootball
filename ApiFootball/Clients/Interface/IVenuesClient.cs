using ApiFootball.Models;
using ApiFootball.Models.Responses;

namespace ApiFootball.Clients.Interface;

public interface IVenuesClient
{
    public Task<BaseResponse<Venue>> GetVenues(int? id, string? name, string? city, string? country, string? search);
}