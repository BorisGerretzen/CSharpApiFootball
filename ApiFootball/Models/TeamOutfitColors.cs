namespace ApiFootball.Models;

public class TeamOutfitColors
{
    public required OutfitColors Player { get; init; }
    public required OutfitColors GoalKeeper { get; init; }
}