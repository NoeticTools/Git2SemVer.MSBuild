using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;
using NoeticTools.Git2SemVer.MSBuild.Framework.Semver;
using Semver;
// ReSharper disable UnusedMemberInSuper.Global


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;

/// <summary>
///     Task outputs for C# script use and source for MSBuild output properties.
/// </summary>
public interface IVersionOutputs
{
    /// <summary>
    ///     The Microsoft assembly version.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         To conform to common usage Git2SemVer's default approach is to make this appear as a three part Semantic
    ///         Version.
    ///     </para>
    ///     <para>
    ///         This a Microsoft four part version with the format &lt;major>.&lt;minor>[.&lt;build>[.&lt;revision>]].
    ///         It is not a semantic version (notice that the build number is the third part).
    ///     </para>
    ///     <para>
    ///         See
    ///         <see href="https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-version">System.Version</see>
    ///         .
    ///     </para>
    /// </remarks>
    Version? AssemblyVersion { get; set; }

    /// <summary>
    ///     Build number context.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A build host (<see cref="IBuildHost">IBuildHost</see>) may include BuildContext in the
    ///         <see cref="IBuildHost.BuildId" /> if a unique build number is not available.
    ///     </para>
    /// </remarks>
    string BuildContext { get; set; }

    /// <summary>
    ///     Build number.
    /// </summary>
    string BuildNumber { get; set; }

    /// <summary>
    ///     A version suitable for showing on the build system's build. Null if not set.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The C# script may use this property update the build system's label
    ///         using <see cref="IBuildHost.SetBuildLabel">IBuildHost.SetBuildLabel</see>
    ///         method.
    ///         See also <see cref="IVersionGeneratorInputs.UpdateHostBuildLabel" />.
    ///     </para>
    ///     <para>
    ///         Git2SemVer's default behaviour is to generate this label as:
    ///     </para>
    ///     <code>
    ///     &lt;major>.&lt;minor>.&lt;patch>
    ///     &lt;major>.&lt;minor>.&lt;patch>-&lt;label>.&lt;build number>[.&lt;build context>]
    /// </code>
    ///     <para>
    ///         For a prerelease build this label is shorter than <see cref="Version" />
    ///         as commit ID and branch name are usually already displayed on the build system.
    ///         Having a shorter label helps readability.
    ///     </para>
    /// </remarks>
    SemVersion? BuildSystemVersion { get; set; }

    /// <summary>
    ///     The Microsoft assembly file version.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         To conform to common usage Git2SemVer's default approach is to make this appear as a three part Semantic
    ///         Version.
    ///     </para>
    ///     <para>
    ///         This a Microsoft four part version with the format &lt;major>.&lt;minor>[.&lt;build>[.&lt;revision>]].
    ///         It is not a semantic version (notice that the build number is the third part).
    ///     </para>
    ///     <para>
    ///         See
    ///         <see href="https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-version">System.Version</see>
    ///         .
    ///     </para>
    /// </remarks>
    Version? FileVersion { get; set; }

    /// <summary>
    ///     Git repository outputs for optional C# script (csx) use.
    /// </summary>
    IGitOutputs Git { get; }

    /// <summary>
    ///     The calculated informational version.
    /// </summary>
    [JsonConverter(typeof(SemVersionJsonConverter))]
    SemVersion? InformationalVersion { get; set; }

    /// <summary>
    ///     The code is in initial development phase as defined by
    ///     <see href="https://semver.org/#spec-item-4">Semantic Versioning spec 4</see>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see href="https://semver.org/#spec-item-4">Semantic Versioning spec 4</see> states that a 0 major version
    ///         number indicates that the code is in initial development.
    ///         The first stable release will be 1.0.0.
    ///         Hence, Git2SemVer's default behaviour is to make all 0.x.x builds "InitialDev" prereleases.
    ///     </para>
    ///     <para>
    ///         So, making a first release 1.0.0 tells the consumer that it is a stable (production) release.
    ///     </para>
    /// </remarks>
    bool IsInInitialDevelopment { get; set; }

    /// <summary>
    ///     Optional script output to MSBuild property <c>Git2SemVer_Output1</c>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    string Output1 { get; set; }

    /// <summary>
    ///     Optional script output to MSBuild property <c>Git2SemVer_Output2</c>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    string Output2 { get; set; }

    /// <summary>
    ///     NuGet package version.
    /// </summary>
    [JsonConverter(typeof(SemVersionJsonConverter))]
    SemVersion? PackageVersion { get; set; }

    /// <summary>
    ///     The prerelease label.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If a release version this property is an empty string.
    ///         Otherwise, it holds the prerelease label identifier such as 'beta' or 'alpha'.
    ///         By convention this is used as the first Semantic Versioning identifier in the prerelease.
    ///     </para>
    /// </remarks>
    string PrereleaseLabel { get; set; }

    /// <summary>
    ///     The calculated version without metadata identifiers.
    /// </summary>
    [JsonConverter(typeof(SemVersionJsonConverter))]
    SemVersion? Version { get; set; }

    /// <summary>
    ///     Set all version properties from provided informational version.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         When there is a consistent version formating used across all version properties,
    ///         the informational version holds all required elements.
    ///     </para>
    ///     <para>
    ///         <seealso cref="BuildNumber"/> and <seealso cref="BuildContext"/> properties are updated.
    ///     </para>
    ///     <para>
    ///         This method will:
    ///         <list type="bullet">
    ///             <item>0.x.x versions are 'InitialDev' prereleases</item>
    ///             <item>
    ///                 Set <seealso cref="BuildSystemVersion" /> to <seealso cref="Version" />. Consider modifying
    ///                 BuildSystemLabel after this method is called.
    ///             </item>
    ///         </list>
    ///     </para>
    /// </remarks>
    void SetAllVersionPropertiesFrom(SemVersion informationalVersion, string buildNumber, string buildContext);

    /// <summary>
    ///     Set all version properties from provided informational version.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         When there is a consistent version formating used across all version properties,
    ///         the informational version holds all required elements.
    ///     </para>
    ///     <para>
    ///         Does not update <seealso cref="BuildNumber"/> and <seealso cref="BuildContext"/> properties.
    ///     </para>
    ///     <para>
    ///         This method will:
    ///         <list type="bullet">
    ///             <item>0.x.x versions are 'InitialDev' prereleases</item>
    ///             <item>
    ///                 Set <seealso cref="BuildSystemVersion" /> to <seealso cref="Version" />. Consider modifying
    ///                 BuildSystemLabel after this method is called.
    ///             </item>
    ///         </list>
    ///     </para>
    /// </remarks>
    void SetAllVersionPropertiesFrom(SemVersion informationalVersion);
}