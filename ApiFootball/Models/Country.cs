﻿namespace ApiFootball.Models;

public sealed class Country
{
    public required string Name { get; init; }
    public required string Code { get; init; }
    public required string Flag { get; init; }
}