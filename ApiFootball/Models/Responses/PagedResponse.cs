namespace ApiFootball.Models.Responses;

public class PagedResponse<T> : BaseResponse
{
    public required Paging Paging { get; init; }
    public required List<T> Response { get; init; }
}