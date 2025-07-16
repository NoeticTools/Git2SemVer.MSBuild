---
uid: maturity-identifier
---

# Build maturity identifier

By default, the first identifier in a version's prelease identifiers is always the build maturity label like `alpha` or `beta`.
Not used for release versioning.

For example:

> 1.2.3-beta.7658
>
> 1.2.3-beta.7658+feature-mybranch.6ab397d5

The build maturity is derived from the branch name using a regular express set in the MSBuild property `Git2SemVer_BranchMaturityPattern`.
See [MSBuild properties](xref:versioning-msbuild-properties).

The default settings are (first match from top is used):

| Maturity | Branch name regex                         | Matching examples  |
| :---:    |:---                                       |:---                |
| Release  | `^(main|release)[\\/_]?`                  | `main`, `release`, `release/release_name` |
| RC       | `^(?<rc>(main|release)[\\/_]rc.*)[\\/_]?` | `main/rc`, `release/rc5`  |
| Beta     | `^(feature)[\\/_]?`                       | `feature`, `feature/feature_name`  |
| Alpha    | `^((.+))[\\/_]?`                          | `dev/MyBranch`, `tom` |


## Related topics

* [Workflow](xref:workflow)
* [Release tagging](xref:release-tagging)
* [Versioning](xref:versioning)
* [Branch naming](xref:branch-naming)
* [Build properties](xref:versioning-msbuild-properties)
