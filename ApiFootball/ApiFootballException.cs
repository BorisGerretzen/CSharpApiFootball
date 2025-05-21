namespace ApiFootball;

public class ApiFootballException(List<ApiFootballError> errors) : Exception($"ApiFootball error: {string.Join(", ", errors)}")
{
    public List<ApiFootballError> Errors { get; } = errors;
}