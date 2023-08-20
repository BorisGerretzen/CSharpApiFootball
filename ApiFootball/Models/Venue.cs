namespace ApiFootball.Models;

public class Venue {
    public int? Id { get; private set; }
    public string? Name { get; private set; }
    public string? Address { get; private set; }
    public string? City { get; private set; }
    public int? Capacity { get; private set; }
    public string? Surface { get; private set; }
    public string? Image { get; private set; }
}