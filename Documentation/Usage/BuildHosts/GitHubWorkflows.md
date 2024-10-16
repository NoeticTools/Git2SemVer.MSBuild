---
uid: github-workflows
---
![](../Images/Git2SemVer_banner_840x70.png)
## GitHub workflows

TeamCity is a controlled host.

### Detection

Git2SemVer does not automatically detect when it is running within a GitHub workflow. To use Git2SemVer's GitHub host, set the MSBuild property `Git2SemVer_Env_HostType` to `GitHub` on the build command line like:

```yaml
    - name: Build Project
      run: dotnet build MyApplication.sln -p:Git2SemVer_Env_HostType=GitHub
```

### Build number

Github workflows do not provide a [build number](xref:glossary#build-number) but Git2SemVer can construct composite build number ([Build ID](xref:glossary#build-id)) can be constructed from the run number and the run attempt count.

> [!IMPORTANT]  
> `github.run_number` does not uniquely identify every build. It increments on each new run but does not change if a "run" is rebuilt.
>
> Using run_number alone as a defacto build number will result in some builds being untraceable.

In a github workflow yml pass these two numbers to Git2SemVer like this:

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

Git2SemVer then constructs a unique [Build ID](xref:glossary#build-id) for every build with the format:

On a GitHub host:

| Host property | Description  |
|:-- |:-- |
| Build number | The github run_number. |
| Build context | The github run_attempt. Starts at 1.
| Build ID | `<build number>.<build context>`  (`<github.run_number>.<github.run_attempt>`)  |

Example versions:
* `1.2.3-12345.1`
* `1.2.3-12345.1+3a962b33`
* `1.2.3+12345.1.3a962b33`

An example scenario:

> On a new build after a commit this build ID is: `12345.1`. 
>
> Then that "run" is rebuilt and versioned with the build ID: `12345.2`.
>
> Then, there is another commit, and a new run is versioned with the build ID: `12346.1`.

The build context identifier comes last to ensure correct [Semmantic Versioning pecedence](https://semver.org/#spec-item-11) of rebuilds.

### Properties & services

The build host object's properties:

| Host property | Description  |
|:-- |:-- |
| Build number  | Default is 'UNKNOWN'. Set via MSBuild [Git2SemVer_BuildNumber](xref:msbuild-properties#input) property to `github.run_number` in the workflow. |
| Build context | Default is 'UNKNOWN'. Set via MSBuild [Git2SemVer_BuildContext](xref:msbuild-properties#input) property to `github.run_attempt` in the workflow. |
| Build ID      | `<build context>.<build number>` |
| IsControlled          | false          |
| Name                  | 'Custom'    |

Services:

| Service | Description  |
|:-- |:-- |
| BumpBuildNumber       | Not supported (does nothing) |
| ReportBuildStatistic  | Not supported (does nothing) |
| SetBuildLabel         | Not supported (does nothing)  |
