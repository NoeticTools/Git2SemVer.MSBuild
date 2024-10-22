using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using NoeticTools.Git2Semver.Common;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Persistence;

internal sealed class GeneratedOutputsFile : IGeneratedOutputsFile
{
    public VersionOutputs Load(string directory)
    {
        var propertiesFilePath = GetPropertiesFilePath(directory);
        if (!File.Exists(propertiesFilePath))
        {
            return new VersionOutputs();
        }

        var json = File.ReadAllText(propertiesFilePath);
        return JsonSerializer.Deserialize<VersionOutputs>(json)!;
    }

    public void Save(string directory, VersionOutputs outputs)
    {
        var json = ToJson(outputs);
        File.WriteAllText(GetPropertiesFilePath(directory), json);
    }

    public static string ToJson(VersionOutputs outputs)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin),
            IncludeFields = false,
        };

        var json = JsonSerializer.Serialize(outputs, options);
        return json;
    }

    private static string GetPropertiesFilePath(string directory)
    {
        var propertiesFilePath = Path.Combine(directory, Git2SemverConstants.SharedVersionPropertiesFilename);
        return propertiesFilePath;
    }
}