namespace ApiFootball;

public sealed record ApiFootballError(string Code, string Message)
{
    public override string ToString()
    {
        return $"{Code}: {Message}";
    }
}