---
uid: maturity-identifier
---

# Build maturity identifier

The build maturity identifier is a label like `alpha` or `beta` that appears in pre-release version metadata.

For example:

> 1.2.3-beta.7658
>
> 1.2.3-beta.7658+feature-mybranch.6ab397d5

The build maturity is derived from the branch name using a regular express set in the MSBuild property `Git2SemVer_BranchMaturityPattern`.
See [MSBuild properties](xref:msbuild-properties).

The default settings are (first match from top is used):

| Maturity | Branch name regex                         | Matching examples  |
| :---:    |:---                                       |:---                |
| Release  | `^(main|release)[\\/_]?`                  | `main`, `release`, `release/release_name` |
| RC       | `^(?<rc>(main|release)[\\/_]rc.*)[\\/_]?` | `main/rc`, `release/rc5`  |
| Beta     | `^(feature)[\\/_]?`                       | `feature`, `feature/feature_name`  |
| Alpha    | `^((.+))[\\/_]?`                          | `dev/MyBranch`, `tom` |

