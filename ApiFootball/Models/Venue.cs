﻿namespace ApiFootball.Models;

public sealed class Venue
{
    public int? Id { get; init; }
    public string? Name { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? Country { get; init; }
    public int? Capacity { get; init; }
    public string? Surface { get; init; }
    public string? Image { get; init; }
}