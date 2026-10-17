namespace ApiFootball.Json;

public sealed class EmptyArrayAsNullConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return !typeToConvert.IsValueType;
    }

    public override JsonConverter CreateConverter(
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var converterType = typeof(EmptyArrayAsNullConverter<>)
            .MakeGenericType(typeToConvert);

        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }

    private sealed class EmptyArrayAsNullConverter<T> : JsonConverter<T>
        where T : class
    {
        public override T? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read();

                if (reader.TokenType != JsonTokenType.EndArray)
                    throw new JsonException(
                        "Expected an empty array.");

                return null;
            }

            return JsonSerializer.Deserialize<T>(ref reader, options);
        }

        public override void Write(
            Utf8JsonWriter writer,
            T value,
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}