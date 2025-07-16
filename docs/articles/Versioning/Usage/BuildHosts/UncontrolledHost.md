---
uid: uncontrolled-host
---

## Uncontrolled host

An uncontrolled host is a build machine that is not under strict control (OS, tools, compilers, etc) or does not provide a build number for every build.
Developer boxes are usually uncontrolled hosts.

### Detection

If Git2SemVer does not detect a controlled host (such as a build system) is will treat the host as uncontrolled and create a build counter on that machine.

### Build number

On an uncontrolled host:

| Host property | Description  |
|:-- |:-- |
| Build number  | Locally managed build counter stored in the machines AppData directory. |
| Build context | Machine name as the build number's context is the machine. |
| Build ID      | `<build context>.<build number>`  (`<machine name>.<build number>`)  |

Example versions: 
* `1.2.3-JohnsPC.12345`
* `1.2.3-JohnsPC.12345+3a962b33`
* `1.2.3+JohnsPC.12345.3a962b33`

The machine name (build context) identifier comes before the build number to give the machine name higher [Semmantic Versioning pecedence](https://semver.org/#spec-item-11)
precedence for comparing versions from multiple hosts as the build numbers for different hosts are not comparable.

> [!NOTE]
> [Semmantic Versioning pecedence](https://semver.org/#spec-item-11) will not work when comparing versions from different hosts.
> So NuGet pre-release builds must be published from one host only (typically the build system) so that the highest precedence build is shown.

### Properties

The build host object's properties are:

| Host property | Description  |
|:-- |:-- |
| Build number  | Set to local generated build count. |
| Build context | Set to the host's machine name. |
| Build ID      | `<build context>.<build number>` |
| IsControlled          | false          |
| Name                  | 'Uncontrolled' |

## Services

| services | Description  |
|:-- |:-- |
| BumpBuildNumber       | Not supported (does nothing)  |
| ReportBuildStatistic  | Not supported (does nothing)  |
| SetBuildLabel         | Not supported (does nothing)  |
