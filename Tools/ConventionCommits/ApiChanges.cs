namespace NoeticTools.Common.ConventionCommits;

public sealed class ApiChanges
{
    /// <summary>
    ///     A breaking change has been made since last release.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         From <see href="https://semver.org/#spec-item-8">Semantic Version spec item 8</see>:
    ///         "Major version X (X.y.z | X > 0) MUST be incremented if any backward incompatible changes are introduced to the
    ///         public API.
    ///         It MAY also include minor and patch level changes. Patch and minor versions MUST be reset to 0 when major
    ///         version is incremented."
    ///     </para>
    /// </remarks>
    public bool BreakingChange { get; set; }

    /// <summary>
    ///     A backward compatible bug fix has been made since last release.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         From <see href="https://semver.org/#spec-item-6">Semantic Version spec item 6</see>:
    ///         "Patch version Z (x.y.Z | x > 0) MUST be incremented if only backward compatible bug fixes are introduced.
    ///         A bug fix is defined as an internal change that fixes incorrect behavior."
    ///     </para>
    /// </remarks>
    public bool Fix { get; set; }

    /// <summary>
    ///     A backward compatible functional change has been made since last release.
    ///     This includes feature depreciation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         From <see href="https://semver.org/#spec-item-7">Semantic Version spec item 7</see>:
    ///         "Minor version Y (x.Y.z | x > 0) MUST be incremented if new, backward compatible functionality is introduced to
    ///         the public API.
    ///         It MUST be incremented if any public API functionality is marked as deprecated.
    ///         It MAY be incremented if substantial new functionality or improvements are introduced within the private code.
    ///         It MAY include patch level changes. Patch version MUST be reset to 0 when minor version is incremented."
    ///     </para>
    /// </remarks>
    public bool FunctionalityChange { get; set; }

    public void Aggregate(ApiChanges changes)
    {
        BreakingChange |= changes.BreakingChange;
        FunctionalityChange |= changes.FunctionalityChange;
        Fix |= changes.Fix;
    }

    public override string ToString()
    {
        return $"{(BreakingChange ? "B" : "-")}{(FunctionalityChange ? "F" : "-")}{(Fix ? "P" : "-")}";
    }
}