---
uid: github-workflows
---

<div style="background-color:#944248;padding:0px;margin-bottom:0.5em">
  <img src="https://noetictools.github.io/Git2SemVer.MSBuild/Images/Git2SemVer_banner_840x70.png"/>
</div>

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.MSBuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MsBuild)
<a href="https://github.com/NoeticTools/Git2SemVer">
  ![Static Badge](https://img.shields.io/badge/GitHub%20project-944248?logo=github)
</a>

## GitHub workflows

It is common to use GitHub Workflows to achieve a build host.

### Detection

Git2SemVer does not automatically detect when it is running within a GitHub workflow. To use Git2SemVer's GitHub host, set the MSBuild property `Git2SemVer_Env_HostType` to `GitHub` on the build command line like:

```yaml
    - name: Build Project
      run: dotnet build MyApplication.sln -p:Git2SemVer_Env_HostType=GitHub
```

### Build number

Github workflows do not provide a [build number](xref:glossary#build-number).

> [!IMPORTANT]  
> `github.run_number` does not uniquely identify every build. It increments on each new run but does not change if a "run" is rebuilt.
>
> Using run_number alone as a defacto build number will result in some builds being untraceable.

There are two workarounds:

1. Git2SemVer can construct composite build number ([Build ID](xref:glossary#build-id)) is constructed from the run number and the run attempt count.
2. Use a custom GitHub action like [build-tag-number](https://github.com/onyxmueller/build-tag-number). Using a Custom action is not discussed here.

To construct a composite build number, in the github workflow yml pass the run number and the run attempt count
to Git2SemVer like this:

```yaml
    - name: Build Project
      env:
          RUN_NUMBER: ${{ github.run_number }}
          RUN_ATTEMPT: ${{ github.run_attempt }}
      run: |
          dotnet build MyApplication.sln -p:Git2SemVer_BuildNumber=${{ env.run_number }} \
                                         -p:Git2SemVer_BuildContext=${{ env.run_attempt }} \
                                         -p:Git2SemVer_Env_HostType=GitHub
```

Git2SemVer's GitHub host object then provides a unique [Build ID](xref:glossary#build-id) for every build with the format:

```
<github.run_number>.<github.run_attempt>
    or
<build number>.<build context>
```

An example scenario:

> On a new build after a commit this build ID is: `12345.1`. 
>
> Then that "run" is rebuilt and versioned with the build ID: `12345.2`.
>
> Then, there is another commit, and a new run is versioned with the build ID: `12346.1`.

> [!NOTE]
> GitHub build versions are identifyable as the only versions where the first 2 prerelease or metadata (if a releaase) identifiers are numeric.

Example versions with the build ID identifiers highlighted:

* 1.2.3-`12345.1`
* 1.2.3-`12345.1`+3a962b33
* 1.2.3+`12345.1`.3a962b33

The build context identifier (run attempt) comes second to ensure correct [Semmantic Versioning pecedence](https://semver.org/#spec-item-11) of rebuilds.

### Properties

The GitHub build host object's properties:

| Host property | Description  |
|:-- |:-- |
| Build number  | Default is 'UNKNOWN'. Set via MSBuild [Git2SemVer_BuildNumber](xref:versioning-msbuild-properties##inputs) property to `github.run_number` in the workflow. |
| Build context | Default is 'UNKNOWN'. Set via MSBuild [Git2SemVer_BuildContext](xref:versioning-msbuild-properties##inputs) property to `github.run_attempt` in the workflow. |
| Build ID      | `<build context>.<build number>` |
| Name          | 'GitHub'    |

### Services

| Service | Description  |
|:-- |:-- |
| BumpBuildNumber       | Not supported (does nothing) |
| ReportBuildStatistic  | Not supported (does nothing) |
| SetBuildLabel         | Not supported (does nothing) |
