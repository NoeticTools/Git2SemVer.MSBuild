using NoeticTools.Git2SemVer.MSBuild.Tasks;


namespace NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;

/// <summary>
///     The detected or set build host's properties.
/// </summary>
public interface IBuildHost
{
    /// <summary>
    ///     A context/extension of the build number property to better support GitHub and uncontrolled hosts.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         For optional C# script use.
    ///     </para>
    /// </remarks>
    string BuildContext { get; set; }

    /// <summary>
    ///     The host's build ID.
    /// </summary>
    /// <remarks>
    ///     <p>
    ///         The build ID is constructed from the host's <c>BuildNumber</c> and <c>BuildContext</c> using format return from
    ///         <c>BuildIdFormat</c>.
    ///         On build systems that provide a build number for every build it is the <c>BuildNumber</c>.
    ///     </p>
    /// </remarks>
    IReadOnlyList<string> BuildId { get; }

    /// <summary>
    ///     The format for generating the host's <c>BuildId</c>.
    /// </summary>
    /// <remarks>
    ///     <p>
    ///         The build ID is constructed from the host's <c>BuildNumber</c> and <c>BuildContext</c> using format return from
    ///         <c>BuildIdFormat</c>.
    ///         On build systems that provide a build number for every build it is the <c>BuildNumber</c>.
    ///     </p>
    ///     <p>
    ///         The default value is host dependent.
    ///         This property can be set by the MSBuild <c>Git2SemVer_BuildIDFormat</c> property.
    ///     </p>
    /// </remarks>
    string BuildIdFormat { get; set; }

    /// <summary>
    ///     Host provided build number.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         It is best practice for a <b>build number</b> to be an increasing integer value that uniquely identifies a
    ///         build on the host.
    ///         For builds made on a build system (a controlled host) the build number provides traceability to the build.
    ///     </para>
    ///     <para>
    ///         On uncontrolled hosts, such as developer machines, cannot provide a build number that gives reliable
    ///         traceability.
    ///         On these hosts the build number is often replaced with an uncontrolled <b>build ID</b> that is often generated
    ///         using a count of commits since a point or similar.
    ///         These build IDs cannot be reliably unique.
    ///         To mitigate duplication issues additional information such as branch name, commit ID, and/or host name are
    ///         included.
    ///     </para>
    ///     <para>
    ///         This build number is a string do accomodate both build numbers and build IDs and will be an empty string if the
    ///         host does not provide a build number.
    ///     </para>
    /// </remarks>
    string BuildNumber { get; set; }

    /// <summary>
    ///     The host's type ID. Gives an ID for the type of host detected or the type used if
    ///     <see cref="Git2SemVerGenerateVersionTask.HostType" /> is set.
    /// </summary>
    HostTypeIds HostTypeId { get; }

    /// <summary>
    ///     Build host type name defined by <see cref="HostTypeIds" />. e.g: "TeamCity".
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Used internally to bump the host's build number.
    ///     Not supported on most hosts.
    /// </summary>
    /// <remarks>
    ///     Do not use in C# script. Internal use only.
    /// </remarks>
    string BumpBuildNumber();

    /// <summary>
    ///     Report a build statistic to the host. Does nothing if the host does not support build statistics.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Currently only supported for TeamCity hosts.
    ///         See
    ///         <see href="https://www.jetbrains.com/help/teamcity/service-messages.html#Reporting+Build+Statistics">
    ///             TeamCity -
    ///             Reporting build statistics
    ///         </see>
    ///     </para>
    /// </remarks>
    void ReportBuildStatistic(string key, int value);

    /// <summary>
    ///     Report a build statistic to the host. Does nothing if the host does not support build statistics.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Currently only supported for TeamCity hosts.
    ///         See
    ///         <see href="https://www.jetbrains.com/help/teamcity/service-messages.html#Reporting+Build+Statistics">
    ///             TeamCity -
    ///             Reporting build statistics
    ///         </see>
    ///     </para>
    /// </remarks>
    void ReportBuildStatistic(string key, double value);

    /// <summary>
    ///     Set the host's build label. Does nothing if not supported on the host.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use to display semantic version on a build system.
    ///     </para>
    ///     <para>
    ///         Currently only supported for TeamCity hosts.
    ///         See
    ///         <see href="https://www.jetbrains.com/help/teamcity/service-messages.html#Reporting+Build+Number">
    ///             TeamCity -
    ///             Reporting build number
    ///         </see>
    ///     </para>
    /// </remarks>
    void SetBuildLabel(string label);
}