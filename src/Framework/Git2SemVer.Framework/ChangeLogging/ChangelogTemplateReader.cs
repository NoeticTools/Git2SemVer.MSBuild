using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Logging;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class ChangelogTemplateReader(ILogger logger)
{
    public string Load(string directory)
    {
        var templatePath = Path.Combine(directory, ChangelogConstants.DefaultMarkdownTemplateFilename);
        if (File.Exists(templatePath))
        {
            return File.ReadAllText(templatePath);
        }

        logger.LogDebug($"Creating default template file: {templatePath}");
        var defaultTemplate = GetDefaultTemplate();
        File.WriteAllText(templatePath, defaultTemplate);
        return defaultTemplate;
    }

    private static string GetDefaultTemplate()
    {
        const string resourceFilename = ChangelogConstants.DefaultMarkdownTemplateFilename;
        var assembly = typeof(ChangelogGenerator).Assembly;
        var resourcePath = assembly.GetManifestResourceNames()
                                   .SingleOrDefault(str => str.EndsWith(resourceFilename))!;
        if (resourcePath == null)
        {
            throw new Git2SemVerOperationException($"The code resource file '{resourceFilename}' is required but not found.");
        }
        using var stream = assembly.GetManifestResourceStream(resourcePath!)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}