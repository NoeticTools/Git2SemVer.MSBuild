namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public static class ChangelogResources
{
    public static readonly ChangelogCategorySettings[] DefaultCategories =
    [
        new(1, "Added", "feat"),
        new(2, "Changed", "change"),
        new(3, "Depreciated", "deprecate"),
        new(4, "Removed", "remove"),
        new(5, "Fixed", "fix"),
        new(6, "Security", "security"),
        new(7, "Other", ".*")
    ];

    public static readonly string DefaultMarkdownTemplateFilename = "markdown.template.scriban";

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