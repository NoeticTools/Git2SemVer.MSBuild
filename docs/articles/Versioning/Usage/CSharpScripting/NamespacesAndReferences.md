---
uid: csharp-script-namespaces
---

# Namespaces and references

## Namespaces

> [!TIP]
> At run-time (build-time) the C# scipt does not need `using` directives for the namespaces listed here.
> However using directives are necessary to get intellisence and syntax checking when editing
> a script in an IDE project.

The following namespaces are imported in the C# script's global context:

```
Semver
NuGet.Versioning
System
System.IO
System.Text.RegularExpressions
System.Numerics
System.Linq
System.Runtime.Serialization
System.Globalization
System.Runtime.CompilerServices
System.Runtime.InteropServices
Microsoft.Extensions.Primitives
System.Diagnostics.CodeAnalysis
System.Diagnostics
System.Collections
System.Collections.Generic
NoeticTools.Common.Tools.DotnetCli
NoeticTools.Common.Tools.Git
NoeticTools.Git2SemVer.MSBuild.Tools.CI
NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting
NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting
NoeticTools.Common.Logging
NoeticTools.MSBuild.Tasking
```

## References

The following assemblies are references in the C# script's context: 

```
Semver
NuGet.Versioning
System.Private.CoreLib
System.Text.RegularExpressions
System.Runtime.Numerics
System.Linq
Microsoft.Extensions.Primitives
NoeticTools.Common
NoeticTools.Git2SemVer.MSBuild
NoeticTools.MSBuild.Tasking
```

> [!NOTE]
> For the dotnet and Visual Studio compatibility references must be .NET Standard 2.0 compatible.
