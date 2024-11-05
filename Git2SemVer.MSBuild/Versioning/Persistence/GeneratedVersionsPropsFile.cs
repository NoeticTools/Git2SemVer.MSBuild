using NoeticTools.Git2Semver.Common;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Persistence;

internal sealed class GeneratedVersionsPropsFile : IGeneratedOutputsPropFile
{
    private const string FileTemplate = """
                                        <Project>
                                          <!-- See https://aka.ms/dotnet/msbuild/customize for more details on customizing your build -->
                                        
                                          <PropertyGroup>
                                        
                                              <Version Condition=" '$(Version)' != '' ">###Version###</Version>
                                              <VersionPrefix Condition=" '$(VersionPrefix)' != '' ">###VersionPrefix###</VersionPrefix>
                                              <VersionSuffix Condition=" '$(VersionSuffix)' != '' ">###VersionSuffix###</VersionSuffix>
                                              <PackageVersion Condition=" '$(PackageVersion)' != '' ">###PackageVersion###</PackageVersion>
                                              <InformationalVersion Condition=" '$(InformationalVersion)' != '' ">###InformationalVersion###</InformationalVersion>
                                              <AssemblyVersion Condition=" '$(AssemblyVersion)' != '' ">###AssemblyVersion###</AssemblyVersion>
                                              <FileVersion Condition=" '$(FileVersion)' != '' ">###FileVersion###</FileVersion>
                                        
                                          </PropertyGroup>

                                        </Project>
                                        """;

    public void Write(string directory, VersionOutputs outputs)
    {
        var content = FileTemplate;
        content = content.Replace("###Version###", outputs.Version?.ToString() ?? "");
        content = content.Replace("###VersionPrefix###", outputs.Version?.WithoutPrereleaseOrMetadata().ToString() ?? "");
        content = content.Replace("###VersionSuffix###", outputs.InformationalVersion?.Prerelease ?? "");
        content = content.Replace("###PackageVersion###", outputs.PackageVersion?.ToString() ?? "");
        content = content.Replace("###InformationalVersion###", outputs.InformationalVersion?.ToString() ?? "");
        content = content.Replace("###AssemblyVersion###", outputs.AssemblyVersion?.ToString() ?? "");
        content = content.Replace("###FileVersion###", outputs.FileVersion?.ToString() ?? "");

        var filePath = GetFilePath(directory);
        if (File.Exists(filePath))
        {
            var priorContent = File.ReadAllText(filePath);
            if (content.Equals(priorContent, StringComparison.Ordinal))
            {
                return;
            }
        }

        File.WriteAllText(filePath, content);
    }

    private static string GetFilePath(string directory)
    {
        return Path.Combine(directory, Git2SemverConstants.SharedVersionPropertiesFilename);
    }
}