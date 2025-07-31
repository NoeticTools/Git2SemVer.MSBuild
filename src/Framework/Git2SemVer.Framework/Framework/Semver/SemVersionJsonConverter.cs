using System.Text.Json;
using System.Text.Json.Serialization;
using Semver;


namespace NoeticTools.Git2SemVer.Framework.Framework.Semver;

public class SemVersionJsonConverter : JsonConverter<SemVersion?>
{
    public override SemVersion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return SemVersion.Parse(value, SemVersionStyles.Strict);
    }

    public override void Write(Utf8JsonWriter writer, SemVersion? value, JsonSerializerOptions options)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer), "JSON writer is required.");
        }

        writer.WriteStringValue(value == null ? "" : value.ToString());
    }
}