namespace NoeticTools.Git2SemVer.Core.Diagnostics;

public abstract class DiagnosticCodeBase
{
    protected DiagnosticCodeBase(int id,
                                 string subcategory,
                                 string description,
                                 string resolution,
                                 string message, params object[] messageArgs)
    {
        Code = $"GSV{id:D3}";
        Message = string.Format(message, messageArgs);
        Description = string.Format(description, messageArgs);
        Resolution = string.Format(resolution, messageArgs);
        Subcategory = subcategory;
    }

    public string Code { get; }

    /// <summary>
    ///     Description that will appear on the documentation page only.
    /// </summary>
    public string Description { get; protected set; }

    public string HelpLink => $"https://noetictools.github.io/Git2SemVer.MSBuild/{DocFolders.ErrorsAndWarnings}/{Code}.html";

    /// <summary>
    ///     Message that will appear in the compiler output as well as on the documentation page.
    /// </summary>
    public string Message { get; }

    public string MessageWithCode => $"{Code}: {Message}";

    /// <summary>
    ///     Resolution that will appear on the documentation page only.
    /// </summary>
    public string Resolution { get; }

    public string Subcategory { get; }

    public string SubCategory { get; } = "Versioning";
}