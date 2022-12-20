namespace ApiFootball.Models.Responses.Outer; 

public class BaseResponse<T> {
    public string Get { get; private set; }
    public Dictionary<string, string> Parameters { get; private set; }
    public Dictionary<string, string> Errors { get; private set; }
    public int Results { get; private set; }
    public Paging Paging { get; private set; }
    public List<T> Response { get; private set; }
}