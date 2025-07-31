# NoeticTools.Git2SemVer.MSBuild

Automatic true [Semmantic Versioning](https://semver.org/) for both **Visual Studio** and donet CLI builds of .NET solutions and projects.
Provides versioning _out of box_. It is fully customisable by by built-in C# scripting and versioning API.

Just works on every **Visual Studio** or dotnet CLI build on every box.

Go to [documentation](https://noetictools.github.io/Git2SemVer/) for more information.

## Features

* _Out of box_ automatic true [Semmantic Versioning](https://semver.org/).
* **No limits** customisable by built-in C# scripting and versioning API.
* Every build (& rebuild) is traceable (default versioning):
  * Incrementing build number on each build system or developer's box build or rebuild (not reliant on commit depth).
  * Version identifies its build host as build system or developer's box and which developer's box.
  * Automatically uses build system (e.g: TeamCity) provided build number.
* No extra build system version generation step or tools.
* Automatic versioning of developer and build system builds in both **Visual Studio** and donet CLI builds. Just build.
* Uses [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) message elements to manage version.
* Branching model (such as GitHub Flow or GitFlow) agnostic. No model configuration.
* Easy solution setup with dotnet tool Git2SemVer.Tool.

See more about features [here](https://noetictools.github.io/Git2SemVer).
