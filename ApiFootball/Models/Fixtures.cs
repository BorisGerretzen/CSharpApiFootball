using Newtonsoft.Json;

namespace ApiFootball; 

public class Fixtures {
    public bool Events { get; private set; }
    public bool Lineups { get; private set; }
    [JsonProperty("statistics_fixtures")] public bool StatisticsFixtures { get; private set; }
    [JsonProperty("statistics_players")] public bool StatisticsPlayers { get; private set; }
}