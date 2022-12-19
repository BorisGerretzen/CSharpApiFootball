namespace ApiFootball.Models;

public class Score {
    public Goals Halftime { get; private set; }
    public Goals Fulltime { get; private set; }
    public Goals Extratime { get; private set; }
    public Goals Penalty { get; private set; }
}