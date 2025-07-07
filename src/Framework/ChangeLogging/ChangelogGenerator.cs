using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Framework.ChangeLogging.Exceptions;
using NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;
using Scriban;
using Semver;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public class ChangelogGenerator(ChangelogLocalSettings localSettings)
{
    /// <summary>
    ///     Generate changelog document.
    /// </summary>
    /// <param name="version"></param>
    /// <param name="contributing"></param>
    /// <param name="lastRunData"></param>
    /// <param name="scribanTemplate"></param>
    /// <param name="changelogToUpdate"></param>
    /// <param name="releaseUrl"></param>
    /// <param name="incremental"></param>
    /// <param name="newChangelog"></param>
    /// <returns>
    ///     Created or updated changelog content.
    /// </returns>
    public string Execute(SemVersion version,
                          ContributingCommits contributing,
                          LastRunData lastRunData,
                          string scribanTemplate,
                          string changelogToUpdate,
                          string releaseUrl,
                          bool incremental,
                          bool newChangelog)
    {
        Git2SemVerArgumentException.ThrowIfNull(releaseUrl, nameof(releaseUrl));
        Git2SemVerArgumentException.ThrowIfNullOrEmpty(scribanTemplate, nameof(scribanTemplate));
        if (!newChangelog)
        {
            Git2SemVerArgumentException.ThrowIfNullOrEmpty(changelogToUpdate, nameof(changelogToUpdate));
        }

        var changes = new ChangeCategoryCollection(contributing.Commits, lastRunData, incremental, localSettings);
        if (!newChangelog && changes.Count == 0)
        {
            return changelogToUpdate;
        }

        var newChangesContent = CreateNewContent(version, contributing, scribanTemplate, releaseUrl, incremental, changes);

        if (newChangelog)
        {
            return newChangesContent;
        }

        var newChangesDocument = new ChangelogDocument("new_changes", newChangesContent);
        var destinationDocument = new ChangelogDocument("existing", changelogToUpdate);

        CopyChangesToReviewSection(changes, newChangesDocument, destinationDocument);

        destinationDocument["version"].Content = newChangesDocument["version"].Content;

        return destinationDocument.Content;
    }

    private static string CreateNewContent(SemVersion version, ContributingCommits contributing, string scribanTemplate, string releaseUrl,
                                           bool incremental, ChangeCategoryCollection changes)
    {
        var newChangesContent = "";
        try
        {
            var model = new ChangelogModel(version,
                                           contributing,
                                           changes,
                                           releaseUrl,
                                           incremental);
            var template = Template.Parse(scribanTemplate);
            newChangesContent = template.Render(model, member => member.Name);
            if (newChangesContent.Trim().Length == 0)
            {
                throw new Git2SemVerScribanFileParsingException("The Scriban template file rendered no content.");
            }
        }
        catch (Git2SemVerScribanFileParsingException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new Git2SemVerScribanFileParsingException("There was a problem parsing or rendering a Scriban template file.", exception);
        }

        return newChangesContent;
    }

    private static void CopyChangesToReviewSection(IReadOnlyList<ChangeCategory> changes,
                                                   ChangelogDocument sourceDocument,
                                                   ChangelogDocument destinationDocument)
    {
        foreach (var category in changes)
        {
            var sourceContent = sourceDocument[category.Settings.Name + " changes"].Content;
            if (sourceContent.Trim().Length <= 0)
            {
                continue;
            }

            var destinationSection = destinationDocument[category.Settings.Name + " changes, for manual review"];
            if (destinationSection.Exists)
            {
                destinationSection.Content += sourceContent;
            }
        }
    }
}