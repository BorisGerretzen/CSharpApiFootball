using Newtonsoft.Json;

namespace ApiFootball; 

public class Coverage {
    public Fixtures Fixtures { get; private set; }
    public bool Standings { get; private set; }
    public bool Players { get; private set; }
    [JsonProperty("top_scorers")] public bool TopScorers { get; private set; }
    [JsonProperty("top_assists")] public bool TopAssists { get; private set; }
    [JsonProperty("top_cards")] public bool TopCards { get; private set; }
    public bool Injuries { get; private set; }
    public bool Predictions { get; private set; }
    public bool Odds { get; private set; }
}