namespace ApiFootball.Models.Response; 

public class FixturesResponse {
    public Fixture Fixture { get; private set; }
    public League League { get; private set; }
    public Teams Teams { get; private set; }
    public Goals Goals { get; private set; }
    public Score Score { get; private set; }
}