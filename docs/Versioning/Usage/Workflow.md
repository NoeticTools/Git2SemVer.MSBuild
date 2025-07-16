---
uid: workflow
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

## Workflow

### The release versioning cycle

If the Git code repository does not have any [release tags](xref:release-tagging) then the project is in the [Semver initial development phase](https://semver.org/#how-should-i-deal-with-revisions-in-the-0yz-initial-development-phase).
The starting version is `0.1.0` (See also: [Versioning](xref:versioning)).

If there have been prior releases, the most recent release that is directly reachable from the head commit 
is the starting version for the current release cycle caculations. 
Git2SemVer stops walking Git commit history when it reaches a [release tags](xref:release-tagging).

The versioning cycle restarts when there is a new release.
Normally the release tag will have the same version as that commit's build version.
The tag does not, normally, change the version but ends the current versioning cycle and starts a new one.

### Version incrementing

Semantic versioning indicates if release has, since the last release, one or more fixes, features, or breaking changes.
So if the start release is, say, `1.2.3` then the next release will be one of:

* `1.2.4` if there was only bug fixes between the releases.
* `1.3.0` if there was one or more features added but no breaking changes ([Semver spec, item 7](https://semver.org#spec-item-7)).
* `2.0.0` if the was one or more breaking changes ([Semver spec, item 8](https://semver.org/#spec-item-8)).

The patch number is automatically bumped after a release to keep the version unique ([Semver spec, item 3](https://semver.org/#spec-item-3)).

The version is bumped by either [Conventionial Commits](https://www.conventionalcommits.org/en/v1.0.0/) messages indicating that a feature or breaking change has been added.
In exceptional cirumstances, such as to correct an error, a release's version can be manipulated by a release tag and then rebuilding the commit.

### Guava example

An example workflow:

| Step    | Action                                        | Version | Explanation                                                 |
| :--:    |:--                                            | :--:    |:--                                                          |
| &#9312; | Start at a release (`1.2.3`) marked by tag `v1.2.3`. | **1.2.3**  | The tag defines this commit as the released version 1.2.3. The versioning release cycle starts from a tagget release.  |
| &#9313; | A commit without API changes                  | 1.2.4 | Bumped as a released version may not be reused. See [Semver spec, item 6](<see href="https://semver.org/#spec-item-6">)
| &#9314; | In a developer branch fix 2 bugs (e.g: [conventional commits](https://www.conventionalcommits.org/en/v1.0.0/) `fix:bug1` and `fix:bug2`). | 1.2.4 | No change as the patch number has already been bumped. Note: [Semver spec, item 6](https://semver.org/#spec-item-6) refers to "fixes" (plural). |
| &#9315; | Add a feature `feat:x`.                       | 1.3.0 | See [Semver spec, item 7](https://semver.org#spec-item-7)     |
| &#9316; | Fix another bug `fix:bug3`.                   | 1.3.0 | See [Semver spec, item 7](https://semver.org/#spec-item-7). It is about changes between releases, not order of changes. |
| &#9317; | Merge the work to a feature branch and then the main branch and this build is released (commit is tagged with used version). | **1.3.0** | A release, the versioning release cycle starts again. |

An example of this worklow's git diagram:
```mermaid
gitGraph
       commit id:"1.2.3+100" tag:"v1.2.3"
       branch feature/guava
       checkout feature/guava
       commit id:"1.2.4-beta.101"
       branch develop/guava
       checkout develop/guava
       commit id:"fix:bug1 1.2.5-alpha.102"
       checkout main
       commit id:"1.2.4+103"
       checkout develop/guava
       commit id:"fix:bug2 1.2.5+104"
       commit id:"feat:x: 1.3.0+105"
       commit id:"fix:bug3 1.3.0+106"
       checkout feature/guava
       merge develop/guava id:"1.3.0-beta.107"
       checkout main
       merge feature/guava id:"1.3.0+108" tag:"v1.3.0"
       commit id:"1.3.1+109"
```

### FAQ

**Can I force a release version with a release tag?**

Yes, but not recommended - Adding a release tag and then rebuilding that build will force the 
release version and start a new release cycle at that version. Useful to correct errors.

**Can I rebuild a release tagged commit?**

Yes - Rebuilds of the commit with a release version tag will produce builds with the same released base version 
but with different build numbers in the full version metadata. So each build is uniquely identifiable.
However only one of these builds may be released as the released version number cannot be reused
and the contents of a release cannot be changed ([Semver spec, item 6](<see href="https://semver.org/#spec-item-6">)).

**Does the release need to be the most recent commit on the release branch**

No - If it is not the most recent then, after adding a [release tag](xref:release-tagging), rebuilding following commits will result in their versions following the released version according to semantic versioning rules.

## Related topics

* [Versioning](xref:versioning)
* [Branch naming](xref:branch-naming)
* [Build maturity identifier](xref:maturity-identifier)
* [Build properties](xref:versioning-msbuild-properties)
