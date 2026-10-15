namespace ApiFootball.Models.Responses;

public sealed class ObjectResponse<T> : BaseResponse
{
    public required T Response { get; init; }
}