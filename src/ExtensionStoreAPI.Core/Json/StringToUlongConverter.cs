using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExtensionStoreAPI.Core.Json;

public class StringToUlongConverter : JsonConverter<ulong>
{
    public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            return 0;
        }
        
        return ulong.TryParse(reader.GetString(), out var value) ? value : 0;
    }

    public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}