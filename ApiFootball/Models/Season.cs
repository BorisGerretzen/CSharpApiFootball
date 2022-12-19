namespace ApiFootball; 

public class Season {
    public int Year { get; private set; }
    public DateOnly Start { get; private set; }
    public DateOnly End { get; private set; }
    public bool Current { get; private set; }
    public Coverage Coverage { get; private set; }
}