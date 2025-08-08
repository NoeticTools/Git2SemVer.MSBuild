﻿using NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public static class ChangelogConstants
{
    public const string DefaultConvCommitsInfoFilename = "conventionalcommits.data.g.json";

    public const string DefaultDataDirectory = ".git2semver";

    public const string DefaultFilename = "CHANGELOG.md";

    public const string DefaultMarkdownTemplateFilename = "changelog.markdown.template.scriban";

    public const string LastRunDataFileSuffix = ".g2sv.data.g.json";

    public const string ProjectSettingsFilename = "git2semver.changelog.settings.json";

    public const string VersionPlaceholder = TagParsingConstants.VersionPlaceholder;

    public const string IssueLinkFormat = "{0}";

    public const string DefaultLogLevel = "info";

    /// <summary>
    ///     The default url to a version's artifacts using version placeholder '%VERSION%'. No link is generated if an empty string.
    /// </summary>
    public const string DefaultArtifactLinkPattern = "";
}