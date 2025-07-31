﻿// ReSharper disable InconsistentNaming

namespace NoeticTools.Git2SemVer.Core.Diagnostics;

[DiagnosticCode]
public sealed class GSV003 : DiagnosticCodeBase
{
    public GSV003()
        : base(3,
               "Versioning",
               """
               This occurs when using .NET SDK 8.0 or later and IncludeSourceRevisionInInformationalVersion is true.
               It is used  warn users that Source Link will appended an extra Git SHA to the informational version.
               """,
               """
               To prevent Source Link from appending an extra git SHA to the information version either add the following
               to the solution's `Directory.Build.props` (if it exists) or the project's file.

               ```xml
               <PropertyGroup>
                 <IncludeSourceRevisionInInformationalVersion>false</IncludeSourceRevisionInInformationalVersion>
               </PropertyGroup>
               ```

               Alternatively remove the Git SHA added by Git2SemVer in the custom C# script file (csx) and disable this
               warning by adding:

               ```xml
               <PropertyGroup>
                   <NoWarn>$(NoWarn);GSV003</NoWarn>
               </PropertyGroup>
               ```

               See: [Source Link included in the .NET SDK](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/8.0/source-link)
               """,
               "Source Link will append the Git SHA to the informational version.")
    {
    }
}