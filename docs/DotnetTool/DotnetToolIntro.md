---
uid: dotnet-tool-intro
---

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.Msbuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MSBuild)
[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.Tool?label=Git2SemVer.Tool)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.Tool)

# Git2SemVer.Tool

<div style="margin-left:0px; margin-top:-5px; margin-bottom:35px; font-family:Calibri; font-size:1.3em;">
No limits .NET solution versioning.</div>

**Git2SemVer.Tool** is a dotnet tool to configure a .NET for **Git2SemVer** solution semmantic versioning.

**Git2SemVer** is a Visual Studio and developer friendly <a href="https://semver.org">Semantic Versioning</a> framework for .NET solution and project versioning.
It works the same with Visual Studio builds and dotnet CLI builds. 
Every build, both developer boxes and the build system, get traceable build numbering (no commit counting).

Version determined by Git release tags and <a href="https://www.conventionalcommits.org/en/v1.0.0/">Conventional Commits</a> compliant message elements.
Use the same commit message elements you also use for changelog generation.

For no limits customisation, **Git2SemVer** detects and executes an optional [C# script](xref:csharp-script) that can change any part of the versioning.

It can be configured for any mix of solution versioning and individual project versioning without external build-time tools.
No build system version generation steps are needed, keeps developer and build environments simple and aligned.


## Quick links

* [Getting Started](xref:dotnet-tool-getting-started)
* Usage
  * [Build Hosts](xref:build-hosts)
  * [C# Script](xref:csharp-script)

 
## License

**Git2SemVer** uses the [MIT license](https://choosealicense.com/licenses/mit/).


## Acknowledgments

This project uses the following tools and libraries. Many thanks to those who created and manage them.

* [Spectre.Console](https://github.com/spectreconsole/spectre.console)
* [Injectio](https://github.com/loresoft/Injectio)
* [JetBrains Annotations](https://www.jetbrains.com/help/resharper/Code_Analysis__Code_Annotations.html)
* [TeamCity.ServiceMessages](https://github.com/JetBrains/TeamCity.ServiceMessages)
* [Semver](https://www.nuget.org/packages/Semver) - files copied to create subset
* [NuGet.Versioning](https://www.nuget.org/packages/NuGet.Versioning)
* [NUnit](https://www.nuget.org/packages/NUnit)
* [Moq](https://github.com/devlooped/moq)
* [docfx](https://dotnet.github.io/docfx/)
* [JsonPeek](https://www.clarius.org/json/)
* <a href="https://www.flaticon.com/free-icons/brain" title="brain icons">Brain icons created by Freepik - Flaticon</a>
* <a href="https://www.flaticon.com/free-icons/consistent" title="consistent icons">Consistent icons created by Freepik - Flaticon</a>
* <a href="https://www.flaticon.com/free-icons/programmer" title="programmer icons">Programmer icons created by Flowicon - Flaticon</a>
