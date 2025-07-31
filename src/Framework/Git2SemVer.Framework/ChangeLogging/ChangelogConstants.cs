using NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public static class ChangelogConstants
{
    public const string VersionPlaceholder = TagParsingConstants.VersionPlaceholder;

    public const string DefaultConvCommitsInfoFilename = "conventionalcommits.data.g.json";

    public const string DefaultMarkdownTemplateFilename = "markdown.template.scriban";

    public const string LastRunDataFileSuffix = ".g2sv.data.g.json";

    public const string ProjectSettingsFilename = "git2semver.changelog.settings.json";

    public const string DefaultDataDirectory = ".git2semver/changelog";

    public const string DefaultFilename = "CHANGELOG.md";

    public static readonly CategorySettings[] DefaultCategories =
    [
        new(1, "Added", "feat"),
        new(2, "Changed", "change"),
        new(3, "Depreciated", "deprecate"),
        new(4, "Removed", "remove"),
        new(5, "Fixed", "fix"),
        new(6, "Security", "security"),
        new(7, "Other", "^(?!dev|Dev|refactor).*$")
    ];

    public static string GetDefaultTemplate()
    {
        var assembly = typeof(ChangelogGenerator).Assembly;
        var resourcePath = assembly.GetManifestResourceNames()
                                   .Single(str => str.EndsWith(DefaultMarkdownTemplateFilename))!;
        using var stream = assembly.GetManifestResourceStream(resourcePath)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}