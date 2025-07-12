using System.Text.Json.Serialization;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

/// <summary>
///     Flags indicating breaking changes, new features, or fixes are present.
/// </summary>
public sealed class ApiChangeFlags : IEquatable<ApiChangeFlags>
{
    public ApiChangeFlags() : this(false, false, false)
    {
    }

    private ApiChangeFlags(ApiChangeFlags changeFlags)
        : this(changeFlags.BreakingChange, changeFlags.FunctionalityChange, changeFlags.Fix)
    {
    }

    public ApiChangeFlags(bool breakingChange, bool functionalityChange, bool fix)
    {
        BreakingChange = breakingChange;
        Fix = fix;
        FunctionalityChange = functionalityChange;
    }

    /// <summary>
    ///     A change has been made since last release.
    /// </summary>
    [JsonIgnore]
    public bool Any => BreakingChange || FunctionalityChange || Fix;

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
    [JsonPropertyOrder(1)]
    public bool BreakingChange { get; private set; }

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
    [JsonPropertyOrder(3)]
    // ReSharper disable once MemberCanBePrivate.Global
    public bool Fix { get; private set; }

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
    [JsonPropertyOrder(2)]
    public bool FunctionalityChange { get; private set; }

    public void Aggregate(ApiChangeFlags changeFlags)
    {
        var result = new ApiChangeFlags(this);
        if (!changeFlags.Any)
        {
            return;
        }

        BreakingChange |= changeFlags.BreakingChange;
        FunctionalityChange |= changeFlags.FunctionalityChange;
        Fix |= changeFlags.Fix;
    }

    public bool Equals(ApiChangeFlags? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return BreakingChange == other.BreakingChange && Fix == other.Fix && FunctionalityChange == other.FunctionalityChange;
    }

    public override string ToString()
    {
        return $"{(BreakingChange ? "B" : "-")}{(FunctionalityChange ? "F" : "-")}{(Fix ? "P" : "-")}";
    }
}