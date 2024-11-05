using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using NoeticTools.Git2Semver.Common;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Persistence;

internal sealed class GeneratedVersionsJsonFile : IGeneratedOutputsJsonFile
{
    private static JsonSerializerOptions SerialiseOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin),
        IncludeFields = false
    };

    public static string GetContent(VersionOutputs outputs)
    {
        var versionInfo = new VersioningInfo { Git2SemVerVersionInfo = outputs };
        return JsonSerializer.Serialize(versionInfo, SerialiseOptions);
    }

    public VersionOutputs Load(string directory)
    {
        var propertiesFilePath = GetFilePath(directory);
        if (!File.Exists(propertiesFilePath))
        {
            return new VersionOutputs();
        }

        var json = File.ReadAllText(propertiesFilePath);
        return JsonSerializer.Deserialize<VersioningInfo>(json)!.Git2SemVerVersionInfo!;
    }

    public void Write(string directory, VersionOutputs outputs)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = GetContent(outputs);
        var existingJson = LoadJson(directory);
        if (json.Equals(existingJson, StringComparison.Ordinal))
        {
            return;
        }

        File.WriteAllText(GetFilePath(directory), json);
    }

    private static string GetFilePath(string directory)
    {
        return Path.Combine(directory, Git2SemverConstants.SharedVersionJsonPropertiesFilename);
    }

    private static string LoadJson(string directory)
    {
        var propertiesFilePath = GetFilePath(directory);
        if (!File.Exists(propertiesFilePath))
        {
            return "";
        }

        return File.ReadAllText(propertiesFilePath);
    }

    private sealed class VersioningInfo
    {
        [JsonPropertyOrder(2)]
        public VersionOutputs? Git2SemVerVersionInfo { get; set; }

        /// <summary>
        ///     This version info's schema version.
        /// </summary>
        [JsonPropertyOrder(1)]
        public int Rev { get; set; } = 1;
    }
}