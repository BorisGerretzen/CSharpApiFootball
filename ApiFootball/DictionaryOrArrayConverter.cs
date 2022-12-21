using Newtonsoft.Json;

namespace ApiFootball;

/// <summary>
///     https://stackoverflow.com/questions/74762080/how-to-deserialize-json-which-is-sometimes-a-dictionary-and-sometimes-an-empty-a
/// </summary>
public class DictionaryOrArrayConverter<TKey, TValue> : JsonConverter
    where TKey : notnull {
    public override bool CanConvert(Type objectType) => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
        switch (reader.TokenType) {
            case JsonToken.StartObject:
                return (Dictionary<TKey, TValue>?)serializer.Deserialize(reader, objectType);
            case JsonToken.StartArray:
                reader.Read();
                if (reader.TokenType != JsonToken.EndArray) throw new JsonReaderException("Empty array expected");

                return new Dictionary<TKey, TValue>();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
        serializer.Serialize(writer, value);
    }
}