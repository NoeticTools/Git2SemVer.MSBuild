---
uid: build-id
---

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.MSBuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MsBuild)
<a href="https://github.com/NoeticTools/Git2SemVer">
  ![Static Badge](https://img.shields.io/badge/GitHub%20project-944248?logo=github)
</a>


# Build ID/number

For traceability every build requires a unique ID that is easily traceable to the [build host](xref:build-hosts) and the code used.
Best practice is for this to be a single incrementing [build number](xref:glossary#build-number). 
Host like TeamCity do provide such a build number but some, like GitHub do not.
There can also be multiple build number contexts such as dev boxes. 
In these cases a [build ID](xref:glossary#build-id) is constructed to be a unique psuedo build number using multiple Semmantic Versioning dot delimited identifiers.

Example build ID shemas:

* `<build number>`  - Used when the project's production build host (e.g: [TeamCity](xref:teamcity)) provides a [build number](xref:glossary#build-number).
* `<build context>.<build number>` - Used for [uncontrolled hosts](xref:uncontrolled-host). The build context is the host machine's name.
* `<build number>.<build context>` - A psuedo build number ([build ID](xref:glossary#build-id)) when the host's build number is not unique (e.g: [GitHub worfklow](xref:github-workflows)).

See the [build host](xref:build-hosts) type for details.

> [!NOTE]
> The host's generated build number can be overriden in any build by setting the MSBuild property `Git2SemVer_BuildNumber`.
>
> See [MSBuild properties](xref:versioning-msbuild-properties).