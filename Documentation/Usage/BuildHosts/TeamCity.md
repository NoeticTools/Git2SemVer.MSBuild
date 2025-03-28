﻿---
uid: teamcity
---

<div style="background-color:#944248;padding:0px;margin-bottom:0.5em">
  <img src="https://noetictools.github.io/Git2SemVer.MSBuild/Images/Git2SemVer_banner_840x70.png"/>
</div>

# TeamCity

[TeamCity](https://www.jetbrains.com/teamcity/) is a [controlled host](xref:glossary#controlled-host). A TeamCity server and its agents are seen as one host.

## Detection

Git2SemVer automatically detects when it is running on a TeamCity agent and uses the TeamCity build number.

## Build number

| Host property | Description  |
|:--            |:--           |
| Build number  | Local Git2SemVer build counter stored in `%AppData%/Git2SemVer`. |
| Build context | '0'          |
| Build ID      | `<build number>`  |

Example versions: 
* `1.2.3-12345`
* `1.2.3-12345+3a962b33`
* `1.2.3+12345.3a962b33`

Also, TeamCity supports setting the build label and for best experience set the MSBuild property `Git2SemVer_UpdateHostBuildLabel` to true. 
This can be done on the build command line like this:

```
  dotnet build -p:Git2SemVer_UpdateHostBuildLabel=true
```

Or, in the `csproj` file like:

```
  <PropertyGroup>
        :
    <Git2SemVer_UpdateHostBuildLabel>true</Git2SemVer_UpdateHostBuildLabel>
        :
  </PropertyGroup>
```

## Properties

The build host object's properties:

| Host property | Description  |
|:-- |:-- |
| Build number  | TeamCity's build number. |
| Build context | '0' |
| Build ID      | `<build number>` |
| IsControlled          | true          |
| Name                  | 'TeamCity'    |

## Services

| Service | Description  |
|:-- |:-- |
| BumpBuildNumber       | Not supported. |
| ReportBuildStatistic  | Supported. See [TeamCity - Reporting Build Statistics](https://www.jetbrains.com/help/teamcity/service-messages.html#Reporting+Build+Statistics). |
| SetBuildLabel         | Supported. See [TeamCity - Reporting Build Number](https://www.jetbrains.com/help/teamcity/service-messages.html#Reporting+Build+Number). |

Git2SemVer's default version generator will:

* Call `SetBuildLabel` with the generated build system version if [Git2SemVer_UpdateHostBuildLabel](xref:msbuild-properties##inputs) is set to true.
* Call `ReportBuildStatistic` with Git2SemVer's MSTask execution time.