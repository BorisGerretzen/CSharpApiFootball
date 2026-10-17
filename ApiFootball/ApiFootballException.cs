using System.Net;
using ApiFootball.Quota;

namespace ApiFootball;

public class ApiFootballException(List<ApiFootballError> errors, HttpStatusCode? statusCode, ApiFootballQuota? quota)
    : Exception($"ApiFootball error: {string.Join(", ", errors)}")
{
    public List<ApiFootballError> Errors { get; } = errors;
    public HttpStatusCode? StatusCode { get; } = statusCode;
    public ApiFootballQuota? Quota { get; } = quota;
}