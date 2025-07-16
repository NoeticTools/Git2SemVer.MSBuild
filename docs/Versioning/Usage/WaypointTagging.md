---
uid: waypoint-tagging
---

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.MSBuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MsBuild)
<a href="https://github.com/NoeticTools/Git2SemVer">
  ![Static Badge](https://img.shields.io/badge/GitHub%20project-944248?logo=github)
</a>

<style>
th {
  text-align: left;
}
</style>

# Versioning waypoint tagging

Waypoint tagging is a workaround, that should not be required, and is
provided to handle possible extreme corner cases.

A waypoint tag provides versioning state up to a commit.
The versioning algorithm stops walking commits in the repository,
much like when it reaches a release, and uses the state read from the tag as a
versioning starting point.

Hard to think of a good reason for using a waypoint tag but it could be used to either:

* Override state from prior commits.
* Avoid walking a very large number of commits (10,000?) to a prior release to reduce time taken.

> [!NOTE]
> If you find in necessary to use this tag please
> raise an issue or discussion thread on GitHub.

A versioning waypoint tag's name formatis:

```
.git2semver.waypoint(<version>).<changeType>
```

Where:

* `<version>` is a release version.
* `<changeType>` is one of:
  * break
  * feat
  * fix

> [!IMPORTANT]
> The `<version>` given in the tag name MUST match [release tag format](xref:release-tagging) formating.

For example, the following waypoint tag will tell the versioning algorithm that the prior release was `1.2.3`
and most significant change, before the tagged commit, was a feature:

```
.git2semver.waypoint(v1.2.3).feat
```

> [!NOTE]
> The waypoint tag format is intentionally distinctive to ensure it is not confused with release tags.
