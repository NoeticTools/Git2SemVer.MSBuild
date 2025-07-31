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
        var defaultTemplate = ChangelogConstants.GetDefaultTemplate();
        File.WriteAllText(templatePath, defaultTemplate);
        return defaultTemplate;
    }
}