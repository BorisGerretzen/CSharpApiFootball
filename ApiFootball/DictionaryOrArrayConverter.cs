namespace ApiFootball;

/// <summary>
/// Inspired by the following StackOverflow post but modified for System.Text.Json.
/// https://stackoverflow.com/questions/74762080/how-to-deserialize-json-which-is-sometimes-a-dictionary-and-sometimes-an-empty-a
/// </summary>
public class DictionaryOrArrayConverter<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>>
    where TKey : notnull
{
    public override Dictionary<TKey, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject) return JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(ref reader, options);

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.EndArray)
                throw new JsonException("Expected an empty array to represent an empty dictionary.");
            return new Dictionary<TKey, TValue>();
        }

        throw new JsonException($"Unexpected token {reader.TokenType}, expected StartObject or StartArray.");
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}