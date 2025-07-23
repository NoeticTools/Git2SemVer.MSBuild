---
uid: working-with-the-code
---

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.MSBuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MsBuild)


# Working with the code

## Contributing

Contributions are very welcome. Here are guidelines to help.

Guidelines:

1. Raise a feature request early. That allows discussion to refine and power the idea.
So much easier to flesh out ideas early rather than at pull request review.
Get agreement first then power forward. Consensus is empowering.
1. Run a [Resharper](https://www.jetbrains.com/resharper/) cleanup first.
If you do not have access to Resharper ask for somebody to do it for you.
Reduces reviewers being distracted from style issues.
1. Ensure that the PR describes what is being offered for review.
Take the time to describe the why and how, write a story.
Be sure to mention if automated tools were used, that can change review focus.
Tell reviewers were the highest risks are.
1. Refactoring is great, and required, but it can make it harder to review functionality.
Consider raise a separate PR to a dev branch if you need a lot of refactoring to
make it easier to add the functionality (pre-factoring).
Ask for help if needd.
1. Asking for help is devine. The earlier the better, we all want to win :-).
1. Provide unit, and or integration, tests. Helps reviewers spend less time looking for bugs so
they can spend more time looking at functionality.

Also - early (or incremental) reviews/PRs often help a lot.

## Branching strategy

The project uses the [GitHub Flow](https://githubflow.github.io/) branching strategy.

## Testing

The code has a set of automated unit and integration tests.
The TeamCity build system runs these tests on every push to the repository.
GitHub will only run the tests on pushes to a release branch.

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
> **Git2SemVer** uses a [TeamCity](https://www.jetbrains.com/teamcity/) build system.

## Versioning

Git2SemVer uses a previously release Git2SemVer.MSBuild package to version itself.
As a project cannot reference a package with the assemblies it builds this is done in a non-standard manner with.

Release tag formatting is:

* **Git2SemVer.MSBuild** - `v<Major>.<Minor>.<Patch>`. e.g: `v1.2.3`.
* **Git2SemVer.Tool** - `tool.v<Major>.<Minor>.<Patch>`. e.g: `tool.v1.2.3`.

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

Microsoft's code analysis use is limited and largely disabled due to many false negatives.
