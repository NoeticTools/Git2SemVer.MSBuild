using LibGit2Sharp;
using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Framework.ChangeLogging.Exceptions;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;
using Scriban;
using Semver;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Commit = NoeticTools.Git2SemVer.Core.Tools.Git.Commit;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public class ChangelogGenerator(ChangelogSettings config)
{
    /// <summary>
    ///     Generate a new changelog document.
    /// </summary>
    /// <param name="releaseUrl"></param>
    /// <param name="versioning"></param>
    /// <param name="contributing"></param>
    /// <param name="scribanTemplate"></param>
    /// <param name="incremental"></param>
    /// <param name="lastRunData"></param>
    /// <returns></returns>
    public string Create(string releaseUrl,
                         IVersionOutputs versioning,
                         ContributingCommits contributing,
                         string scribanTemplate,
                         bool incremental,
                         LastRunData lastRunData)
    {
        Git2SemVerArgumentException.ThrowIfNull(releaseUrl, nameof(releaseUrl));
        Git2SemVerArgumentException.ThrowIfNullOrEmpty(scribanTemplate, nameof(scribanTemplate));

        var version = versioning.Version!;
        var changes = GetChanges(contributing, lastRunData, incremental);
        return Create(releaseUrl, contributing, scribanTemplate, incremental, version, changes);
    }

    public string Update(string releaseUrl,
                         IVersionOutputs versioning,
                         ContributingCommits contributing,
                         string scribanTemplate,
                         string changelogToUpdate,
                         LastRunData lastRunData)
    {
        Git2SemVerArgumentException.ThrowIfNull(releaseUrl, nameof(releaseUrl));
        Git2SemVerArgumentException.ThrowIfNullOrEmpty(scribanTemplate, nameof(scribanTemplate));
        Git2SemVerArgumentException.ThrowIfNullOrEmpty(scribanTemplate, nameof(changelogToUpdate));

        var version = versioning.Version!;
        var changes = GetChanges(contributing, lastRunData, true);
        if (changes.Count == 0)
        {
            return changelogToUpdate;
        }

        var createdContent = Create(releaseUrl, contributing, scribanTemplate, true, version, changes);
        var sourceDocument = new ChangelogDocument("generated", createdContent);
        var destinationDocument = new ChangelogDocument("existing", changelogToUpdate);

        CopyFoundChangesToExistingChangelogToReviewSection(changes, sourceDocument, destinationDocument);

        // Copy version section as it may have changed to a release
        destinationDocument["version"].Content = sourceDocument["version"].Content;

        return destinationDocument.Content;
    }

    private static void CopyFoundChangesToExistingChangelogToReviewSection(IReadOnlyList<CategoryChanges> changes,
                                                                           ChangelogDocument sourceDocument,
                                                                           ChangelogDocument destinationDocument)
    {
        foreach (var category in changes)
        {
            // todo - what if category not found in file?
            

            var sourceContent = sourceDocument[category.Settings.Name + " changes"].Content;
            if (sourceContent.Trim().Length > 0)
            {
                var destinationSection = destinationDocument[category.Settings.Name + " changes, for manual review"];
                destinationSection.Content += sourceContent;
            }
        }
    }

    private static string Create(string releaseUrl,
                                 ContributingCommits contributing,
                                 string scribanTemplate,
                                 bool incremental,
                                 SemVersion version,
                                 IReadOnlyList<CategoryChanges> changes)
    {
        var model = new ChangelogModel(version,
                                       contributing,
                                       changes,
                                       releaseUrl,
                                       incremental);
        try
        {
            var template = Template.Parse(scribanTemplate);
            var content = template.Render(model, member => member.Name);
            if (content.Trim().Length == 0)
            {
                throw new Git2SemVerScribanFileParsingException("The Scriban template file rendered no content.");
            }
            return content;
        }
        catch (Git2SemVerScribanFileParsingException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new Git2SemVerScribanFileParsingException("There was a problem parsing or rendering a Scriban template file.", exception);
        }
    }

    private static List<Commit> Extract(List<Commit> remainingCommits, string changeType)
    {
        var regex = new Regex(changeType);
        var extracted = remainingCommits.Where(x => regex.IsMatch(x.MessageMetadata.ChangeTypeText)).ToList();
        extracted.ForEach(x => remainingCommits.Remove(x));
        return extracted;
    }

    private static CategoryChanges GetCategoryChanges(ChangelogCategorySettings categorySettings,
                                                      List<Commit> remainingCommits)
    {
        var commits = Extract(remainingCommits, categorySettings.ChangeType);
        var categoryChanges = new CategoryChanges(categorySettings);
        categoryChanges.AddRange(GetUniqueChangelogEntries(commits));
        return categoryChanges;
    }

    private IReadOnlyList<CategoryChanges> GetChanges(ContributingCommits contributing, LastRunData lastRunData, bool incremental)
    {
        var remainingCommits = new List<Commit>(contributing.Commits.Where(x => x.MessageMetadata.ApiChangeFlags.Any));
        if (incremental)
        {
            TrimHandledChanges(remainingCommits, lastRunData);
        }
        var orderedCategories = config.Categories.OrderBy(x => x.Order);
        return orderedCategories.Select(category => GetCategoryChanges(category, remainingCommits)).ToList();
    }

    private static void TrimHandledChanges(List<Commit> commits, LastRunData lastRunData)
    {
        var remainingCommitsLookup = new ChangeMessageDictionary<Commit>();
        remainingCommitsLookup.AddRangeUnique(commits);

        foreach (var handledChange in lastRunData.HandledChanges)
        {
            if (!remainingCommitsLookup.TryGet((handledChange.ChangeType, handledChange.Description), out var commit))
            {
                continue;
            }

            var commitIssues = commit!.MessageMetadata.Issues;
            var newIssues = commitIssues.Where(x => !handledChange.Issues.Contains(x)).ToList();
            if (newIssues.Any())
            {
                handledChange.Issues.AddRange(newIssues);
            }
            else
            {
                remainingCommitsLookup.Remove(commit!);
            }
        }

        var remainingCommits = remainingCommitsLookup.GetAll();
        foreach (var commit in remainingCommits)
        {
            var commitMetadata = commit.MessageMetadata;

            lastRunData.HandledChanges.Add(new HandledChange()
            {
                ChangeType = commitMetadata.ChangeTypeText,
                Description = commitMetadata.ChangeDescription,
                Issues = commitMetadata.Issues.ToList()
            });
        }

        commits.Clear();
        commits.AddRange(remainingCommits);
    }


    private static IReadOnlyList<ChangeLogEntry> GetUniqueChangelogEntries(IReadOnlyList<Commit> commits)
    {
        var changeEntries = new List<ChangeLogEntry>();
        foreach (var commit in commits)
        {
            // >>> todo - incremental change. reject change if commitId is recorded in LastRun. Add commits to ChangeLogEntry

            var entry = changeEntries.SingleOrDefault(x => x.Equals(commit.MessageMetadata));
            if (entry == null)
            {
                entry = new ChangeLogEntry(commit.MessageMetadata);
                changeEntries.Add(entry);
            }
            else
            {
                // todo - incremental update will need to check issues count
                entry.AddIssues(commit.MessageMetadata.FooterKeyValues["issues"]);
            }
            logEntry.AddCommitId(commit.CommitId.ShortSha);
        }

        return changeEntries;
    }
}