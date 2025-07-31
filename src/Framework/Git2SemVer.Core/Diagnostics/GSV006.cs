namespace NoeticTools.Git2SemVer.Core.Diagnostics;

[DiagnosticCode]
public sealed class GSV006 : DiagnosticCodeBase
{
    public GSV006(string tagFormat)
        : base(id: 6,
               subcategory: "Versioning",
               description: """
                            The occurs when the build property `Git2SemVer_ReleaseTagFormat`'s value is missing the required placeholder text: ``%VERSION%``.
                            This placeholder text will be replaced with the regular expression `(?<version>\d+\.\d+\.\d+)` to read any tag version number.
                            """,
               resolution: """
                           Correct the `Git2SemVer_ScriptPath` build property value to include `%VERSION%`.

                           The `Git2SemVer_ScriptPath` build property is set the project file or in a directory build properties file like `Directory.Build.props`.

                           For example:
                           ```xml
                           <PropertyGroup>
                             <Git2SemVer_ReleaseTagFormat>MyRelease %VERSION%</Git2SemVer_ReleaseTagFormat>
                           </PropertyGroup>
                           ```
                           """,
               message: "The build property `Git2SemVer_ReleaseTagFormat` value `{0}` must include the placeholder `%VERSION%` text.",
               tagFormat)

    {
    }
}