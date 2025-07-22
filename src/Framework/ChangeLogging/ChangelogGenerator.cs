using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.ChangeLogging.Exceptions;
using Scriban;
using Semver;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public class ChangelogGenerator(ChangelogLocalSettings projectSettings, ILogger logger)
{
    /// <summary>
    ///     Generate changelog document.
    /// </summary>
    /// <param name="inputs"></param>
    /// <param name="lastRunData"></param>
    /// <param name="scribanTemplate"></param>
    /// <param name="changelogToUpdate"></param>
    /// <param name="releaseUrl"></param>
    /// <param name="releaseAs"></param>
    /// <returns>
    ///     Created or updated changelog content.
    /// </returns>
    public string Execute(ConventionalCommitsVersionInfo inputs,
                          LastRunData lastRunData,
                          string scribanTemplate,
                          string changelogToUpdate,
                          string releaseUrl,
                          string releaseAs)
    {
        Git2SemVerArgumentException.ThrowIfNull(releaseUrl, nameof(releaseUrl));
        Git2SemVerArgumentException.ThrowIfNullOrEmpty(scribanTemplate, nameof(scribanTemplate));
        Git2SemVerArgumentException.ThrowIfNull(changelogToUpdate, nameof(changelogToUpdate));

        var contributingReleases = inputs.ContributingReleases.Select(x => SemVersion.Parse(x, SemVersionStyles.Strict)).ToArray();
        var addNewRelease = lastRunData.ContributingReleasesChanged(contributingReleases);
        if (addNewRelease)
        {
            logger.LogInfo("New release.");
            lastRunData = new LastRunData();
        }

        var messagesWithChanges = GetUnhandledChanges(inputs.ConventionalCommits, lastRunData.HandledChanges);

        var issueMarkdownFormatter = new MarkdownLinkFormatter(projectSettings.IssueLinkFormat);
        var orderedCategories = projectSettings.Categories.OrderBy(x => x.Order);
        var changeCategories = orderedCategories.Select(category => ExtractChangeCategory(category, messagesWithChanges, issueMarkdownFormatter))
                                                .ToList();
        if (changelogToUpdate.Length > 0 && changeCategories.Count == 0)
        {
            return changelogToUpdate;
        }

        var newChangesContent = RenderContent(inputs, scribanTemplate, releaseUrl, releaseAs, changeCategories);

        if (changelogToUpdate.Length == 0)
        {
            return newChangesContent;
        }

        var newChangesDocument = new ChangelogDocument("new_changes", newChangesContent, logger);
        var destinationDocument = new ChangelogDocument("existing", changelogToUpdate, logger);
        var priorForcedReleaseTitle = lastRunData.ForcedReleasedTitle.Length > 0 &&
                                      !lastRunData.ForcedReleasedTitle.Equals(releaseAs, StringComparison.InvariantCulture);
        if (addNewRelease || priorForcedReleaseTitle)
        {
            destinationDocument.AddNewRelease(newChangesDocument);
        }
        else
        {
            destinationDocument.AppendChanges(changeCategories, newChangesDocument);
        }

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

        return unhandledMessages.OrderBy(x => x.Description).ToList();
    }

    private static string RenderContent(ConventionalCommitsVersionInfo inputs,
                                        string scribanTemplate,
                                        string releaseUrl,
                                        string releaseAs,
                                        IReadOnlyList<ChangeCategory> changeCategories)
    {
        var newChangesContent = "";
        try
        {
            var model = new ChangelogScribanModel(inputs,
                                                  changeCategories,
                                                  releaseUrl,
                                                  releaseAs);
            var template = Template.Parse(scribanTemplate);
            newChangesContent = template.Render(model, member => member.Name);
        }
        catch (Exception exception)
        {
            throw new Git2SemVerScribanFileParsingException("There was a problem parsing or rendering a Scriban template file.", exception);
        }

        if (newChangesContent.Trim().Length == 0)
        {
            throw new Git2SemVerScribanFileParsingException("The Scriban template must render content.");
        }

        return newChangesContent;
    }
}