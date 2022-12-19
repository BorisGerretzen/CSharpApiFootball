namespace ApiFootball.Models;

public class Team {
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Code { get; private set; }
    public string? Country { get; private set; }
    public int? Founded { get; private set; }
    public bool? National { get; private set; }
    public bool? Winner { get; private set; }
    public string Logo { get; private set; }
}