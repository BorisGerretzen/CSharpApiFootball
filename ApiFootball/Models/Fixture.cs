using Newtonsoft.Json;

namespace ApiFootball; 

[JsonObject(MemberSerialization.OptOut)]
public class Fixture {
    public int Id { get; private set; }
    public string? Referee { get; private set; }
    public string Timezone { get; private set; }
    public DateTime Date { get; private set; }
    public long Timestamp { get; private set; }
    public Periods Periods { get; private set; }
    public Venue Venue { get; private set; }
    public Status Status { get; private set; }
}