using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Framework.ChangeLogging.Exceptions;
using NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;
using Scriban;
using Semver;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public class ChangelogGenerator(ChangelogLocalSettings projectSettings)
{
    /// <summary>
    ///     Generate changelog document.
    /// </summary>
    /// <param name="inputs"></param>
    /// <param name="lastRunData"></param>
    /// <param name="scribanTemplate"></param>
    /// <param name="changelogToUpdate"></param>
    /// <param name="releaseUrl"></param>
    /// <param name="incremental"></param>
    /// <param name="createNewChangelog"></param>
    /// <returns>
    ///     Created or updated changelog content.
    /// </returns>
    public string Execute(ConventionalCommitsVersionInfo inputs,
                          LastRunData lastRunData,
                          string scribanTemplate,
                          string changelogToUpdate,
                          string releaseUrl,
                          bool incremental,
                          bool createNewChangelog)
    {
        Git2SemVerArgumentException.ThrowIfNull(releaseUrl, nameof(releaseUrl));
        Git2SemVerArgumentException.ThrowIfNullOrEmpty(scribanTemplate, nameof(scribanTemplate));
        if (!createNewChangelog)
        {
            Git2SemVerArgumentException.ThrowIfNullOrEmpty(changelogToUpdate, nameof(changelogToUpdate));
        }

        var messagesWithChanges = new List<ConventionalCommit>(inputs.ConventionalCommits);
        if (incremental)
        {
            messagesWithChanges = GetUnhandledChanges(inputs.ConventionalCommits, lastRunData.HandledChanges);
        }

        var issueMarkdownFormatter = new MarkdownLinkFormatter(projectSettings.IssueLinkFormat);
        var orderedCategories = projectSettings.Categories.OrderBy(x => x.Order);
        var changeCategories = orderedCategories.Select(category => ExtractChangeCategory(category, messagesWithChanges, issueMarkdownFormatter)).ToList();



        if (!createNewChangelog && changeCategories.Count == 0)
        {
            return changelogToUpdate;
        }

        var newChangesContent = CreateNewContent(inputs, scribanTemplate, releaseUrl, incremental, changeCategories);

        if (createNewChangelog)
        {
            return newChangesContent;
        }

        var newChangesDocument = new ChangelogDocument("new_changes", newChangesContent);
        var destinationDocument = new ChangelogDocument("existing", changelogToUpdate);

        CopyChangesToReviewSection(changeCategories, newChangesDocument, destinationDocument);

        destinationDocument["version"].Content = newChangesDocument["version"].Content;

        return destinationDocument.Content;
    }

    private static ChangeCategory ExtractChangeCategory(CategorySettings categorySettings,
                                                        List<ConventionalCommit> changeMessages, 
                                                        ITextFormatter markdownIssueFormatter)
    {
        var changeCategory = new ChangeCategory(categorySettings, markdownIssueFormatter);
        changeCategory.ExtractChangeLogsFrom(changeMessages);
        return changeCategory;
    }

    /// <summary>
    ///     Method to reduce metadata down to new change metadata only, and update the handled changes collection.
    /// </summary>
    /// <param name="changeMessages"></param>
    /// <param name="handledChanges"></param>
    private static List<ConventionalCommit> GetUnhandledChanges(IReadOnlyList<ConventionalCommit> changeMessages,
                                                                    List<HandledChange> handledChanges)
    {
        var unhandledMessages = new List<ConventionalCommit>(changeMessages);
        var handledChangesLookup = new ChangeLookup<HandledChange>(handledChanges, v => v);
        foreach (var changeMessage in unhandledMessages.ToArray())
        {
            if (handledChangesLookup.TryGet(changeMessage, out var handledChange))
            {
                if (!handledChange!.TryAddIssues(changeMessage.Issues))
                {
                    unhandledMessages.Remove(changeMessage);
                }
            }
            else
            {
                var newHandledChange = new HandledChange
                {
                    ChangeType = changeMessage.ChangeType,
                    Description = changeMessage.Description,
                    Issues = changeMessage.Issues.ToList()
                };
                handledChangesLookup.Add(newHandledChange);
                handledChanges.Add(newHandledChange);
            }
        }

        return unhandledMessages;
    }
    private static void CopyChangesToReviewSection(IEnumerable<ChangeCategory> changes,
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

    private static string CreateNewContent(ConventionalCommitsVersionInfo inputs, string scribanTemplate, string releaseUrl,
                                           bool incremental, IReadOnlyList<ChangeCategory> changeCategories)
    {
        var newChangesContent = "";
        try
        {
            var model = new ChangelogScribanModel(inputs,
                                           changeCategories,
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
}