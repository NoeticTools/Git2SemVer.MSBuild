namespace NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;

public interface ITagParser
{
    /// <summary>
    ///     Parse tags to identify release and waypoint tags.
    /// </summary>
    TagMetadata ParseTagName(string friendlyName);
}