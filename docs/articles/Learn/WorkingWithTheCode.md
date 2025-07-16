---
uid: working-with-the-code
---

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.MSBuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MsBuild)


# Working with the code

## Branching strategy

The project uses the [GitHub Flow](https://githubflow.github.io/) branching strategy.

## Testing

The code has a set of automated unit and integration tests.

## Build systems

### TeamCity

Currently all commits are built and all test run on a private TeamCity server.
TeamCity is the source of all builds published to NuGet.

It is intended to make the server public in 2025.

### GitHub Workflow

Each commit to the `main` branch triggers GitHub workflows that builds and tests the code
and also build and deploy the documentation which is hosted on GitHub.

> [!IMPORTANT]
> GitHub builds are not released to NuGet.

## Versioning

Git2SemVer uses a previously release Git2SemVer.MSBuild package to version itself.
As a project cannot reference a package with the assemblies it builds this is done in a non-standard manner with.

> [!NOTE]
> The Git2SemVer's build number only increments on solution rebuild.
> This is a limitation caused by limitations in Git2SemVer referencing itself.

## Building documentation

The documentation is hosted on GitHub and built using [docfx](https://dotnet.github.io/docfx/).

Build a local preview from project's root folder with the command line:

```console
docfx docs/docfx.json --serve
```

When completed a link will be shown (usually http://localhost:8080).

The documentation website can, optionally, be built locally using the command:

```console
docfx docs/docfx.json
```

Documentation is published from the main branch automatically by a GitHub action.

## Coding standard

The coding standard is defined by Resharper's clean-up settings found in `Git2SemVer.sln.DotSettings`.
