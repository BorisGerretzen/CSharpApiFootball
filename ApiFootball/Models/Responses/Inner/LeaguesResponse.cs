﻿namespace ApiFootball.Models.Responses.Inner;

public class LeaguesResponse {
    public League League { get; private set; }
    public Country Country { get; private set; }
    public List<Season> Seasons { get; private set; }
}