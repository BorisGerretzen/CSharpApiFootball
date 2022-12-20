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
                var d1 = serializer.Deserialize(reader, objectType);
                Console.WriteLine(d1.GetType().FullName);
                var d2 = (Dictionary<TKey, TValue>)d1;
                Console.WriteLine($"In ReadJson: dictionary has {d2.Count} entries");
                return d2;
            case JsonToken.StartArray:
                reader.Read();
                if (reader.TokenType != JsonToken.EndArray) throw new JsonReaderException("Empty array expected");

                return new Dictionary<TKey, TValue>();
        }

        throw new JsonReaderException("Expected object or empty array");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
        serializer.Serialize(writer, value);
    }
}