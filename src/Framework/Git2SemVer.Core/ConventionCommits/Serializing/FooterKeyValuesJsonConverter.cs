using System.Text.Json;
using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core.Exceptions;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits.Serializing;

public class FooterKeyValuesJsonConverter : JsonConverter<FooterKeyValues?>
{
    public override FooterKeyValues? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            return null;
        }

        List<KeyValue> items = [];
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var keyValueText = reader.GetString();
                var keyValue = KeyValue.Parse(keyValueText!);
                if (keyValue == null)
                {
                    return null;
                }

                items.Add(keyValue);
            }
            else if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }
            else
            {
                return null;
            }
        }

        return new FooterKeyValues(items);
    }

    public override void Write(Utf8JsonWriter writer, FooterKeyValues? value, JsonSerializerOptions options)
    {
        Git2SemVerArgumentException.ThrowIfNull(writer, nameof(writer));
        Git2SemVerArgumentException.ThrowIfNull(value, nameof(value));

        writer.WriteStartArray();
        foreach (var item in value!)
        {
            writer.WriteStringValue(item.ToString());
        }

        writer.WriteEndArray();
    }
}