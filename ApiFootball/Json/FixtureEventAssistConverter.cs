namespace ApiFootball.Json;

public sealed class FixtureEventAssistConverter : JsonConverter<FixtureEventPlayer>
{
    public override FixtureEventPlayer? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var element = document.RootElement;

        if (element.ValueKind == JsonValueKind.Null)
            return null;

        if (element.ValueKind != JsonValueKind.Object)
            throw new JsonException("Expected a player object.");

        if (element.TryGetProperty("id", out var id) &&
            element.TryGetProperty("name", out var name) &&
            id.ValueKind == JsonValueKind.Null &&
            name.ValueKind == JsonValueKind.Null)
            return null;

        var player = element.Deserialize<FixtureEventPlayer>(options) ?? throw new JsonException("Invalid player.");

        if (player.Name is null)
            throw new JsonException("Player name cannot be null.");

        return player;
    }

    public override void Write(Utf8JsonWriter writer, FixtureEventPlayer value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}