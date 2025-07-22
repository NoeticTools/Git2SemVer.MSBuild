---
uid: versioning-project-versioning
---


## Project versioning

Project versioning is when a project is versioned independly of any other project in a solution.

To use project versioning add the [NoeticTools.Git2SemVer.MSBuild nuget package](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MSBuild) to the project.

> [!IMPORTANT]
> If the project is open in Visual Studio you must restart Visual Studio
> for the versioning MSBuild tasks to be loaded.
>
> MSBuild tasks are only loaded on project load.