namespace ApiFootball.Models.Response; 

public class BaseResponses {
    public string Get { get; private set; }
    public Dictionary<string, string> Parameters { get; private set; }
    public List<string> Errors { get; private set; }
    public int Results { get; private set; }
    public Paging Paging { get; private set; }
}