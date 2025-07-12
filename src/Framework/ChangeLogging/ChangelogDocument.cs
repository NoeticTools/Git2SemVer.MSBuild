namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

internal sealed class ChangelogDocument(string name, string content)
{
    public string Content { get; set; } = content;

    /// <summary>
    ///     Get section from document. Sections are marked by metadata within the document.
    /// </summary>
    /// <param name="name">Section name</param>
    /// <returns>Section object</returns>
    public ChangelogSection this[string name] => new(name, this);

    /// <summary>
    ///     The document's name.
    /// </summary>
    public string Name { get; } = name;
}