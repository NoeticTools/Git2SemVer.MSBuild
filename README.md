![](docs/Images/Git2SemVer_banner_840x70.png)
[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.MSBuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MsBuild) 

# NoeticTools.GitSemVer

Automatic true [Semmantic Versioning](https://semver.org/) for both **Visual Studio** and donet CLI builds of .NET solutions and projects.
Provides versioning _out of box_ and is fully customisable by by built-in C# scripting with a versioning API.

Just works on every <b>Visual Studio</b> or dotnet CLI build on every box.

[![Documentation Shield]](https://noetictools.github.io/Git2SemVer/)

An example git workflow from a release `1.2.3` to the next release `2.0.0`:

```mermaid
gitGraph
        commit id:"1.2.3+99"
        
        commit id:"1.2.3+100" tag:"1.2.3"
        branch feature/berry order: 1
        checkout feature/berry
        commit id:"1.2.4-beta.101"

        checkout main
        commit id:"1.2.4+102"
        branch feature/peach order: 3
        checkout feature/berry

        branch develop/berry order: 2
        checkout develop/berry
        commit id:"feat:berry 1.3.0-alpha.103"
        checkout feature/berry
        merge develop/berry id:"1.3.0-beta.104"
        checkout main
        merge feature/berry id:"1.3.0+105"

        checkout feature/peach
        commit id:"fix:bug1 1.2.4-beta.106"
        commit id:"feat!:peach 2.0.0-beta.107"
        checkout main
        merge feature/peach id:"2.0.0+108" tag:"v2.0.0"

        commit id:"2.0.1-beta.108"
```
<br/>

Git2SemVer can also generate the changelog. 

Example generated draft (pre-release) changelog fragment:

![](docs/Images/draft_changelog_fragment.png)

<br/>

## License

`Git2SemVer` uses the [MIT license](https://choosealicense.com/licenses/mit/).

## Acknowledgments

This project uses the following tools and libraries. Many thanks to those who created and manage them.

* [Spectre.Console](https://github.com/spectreconsole/spectre.console)
* [Injectio](https://github.com/loresoft/Injectio)
* [JetBrains Annotations](https://www.jetbrains.com/help/resharper/Code_Analysis__Code_Annotations.html)
* [Semver](https://www.nuget.org/packages/Semver) - files copied to create subset
* [NuGet.Versioning](https://www.nuget.org/packages/NuGet.Versioning)
* [NUnit](https://www.nuget.org/packages/NUnit)
* [Moq](https://github.com/devlooped/moq)
* [docfx](https://dotnet.github.io/docfx/)
* <a href="https://www.flaticon.com/free-icons/brain" title="brain icons">Brain icons created by Freepik - Flaticon</a>


<!---------------------------------------------------------------------------->

[Documentation Shield]: https://img.shields.io/badge/See_the_full_documentation_here-37a779?style=for-the-badge

