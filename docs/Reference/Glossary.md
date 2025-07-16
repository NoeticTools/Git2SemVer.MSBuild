---
uid: glossary
---

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.MSBuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MsBuild)
<a href="https://github.com/NoeticTools/Git2SemVer">
  ![Static Badge](https://img.shields.io/badge/GitHub%20project-944248?logo=github)
</a>


# Glossary


## Build ID

A build ID is one or more identifiers used when a build number is not available or requires context ([uncontrolled builds](#uncontrolled-build)).
A build ID is intended to provide a possibility of traceability.

So that build IDs are not confused with build numbers it is useful for version prerelease and/or metadata to identify uncontrolled builds.

## Build height

Build height refers to a count of commits since a reference commit.

## Build number

The build number uniquely identifies a build to provide traceability to build logs/records on a build system.
Traditionally a integer that increases with each build.
It is useful if the build number always increases chronologically.

A build number is critical to achieve traceability and therefore also to acheive reproducability.

[Uncontrolled builds](#uncontrolled-build) cannot provide a build number to this definition and instead provide a non-traceable "build ID".

**Note:** A semmantic version usually requires a build number as identifiers including the normal version (1.2.3),
commit ID (SHA), and commits count are not unique.

## Controlled build

A build made on a [controlled host](#controlled-host)

## Controlled host

A controlled host is a build host that:

* Has a controlled environment so that builds on the host are reproducable.
* Keeps build logs.
* Keeps build records of build environment.

A build system is normally a controlled host.

## Environment parity

![](../Images/consistency_64x64.png)

Environment parity is a best pracitice.
It is the practise of keeping all dev/production environments as similar as possible.

## Initial development

A Semantic Versioning 2.0 version with a major version number of 0 is an [initial development](https://semver.org/#spec-item-4) build.
This refers to a period of software development from project start to the first stable release.

## Metadata identifier

See Semantic Versioning 2.0 description of [build metadata](https://semver.org/#spec-item-10).

## Prerelease identifier

See Semantic Versioning 2.0 description of a [prerelease version](https://semver.org/#spec-item-9).

## Prerelease label

A prerelease label, like "beta", is a category describing a prerelease's maturity.
For prereleases to be ordered by maturing (after normal semver) it is the first prerelease identify to give [Semantic Versioning precidence](https://semver.org/#spec-item-11) by maturity.

For the most mature prerelease to be shown first when sorted lexically in ASCII sort order.
For example a beta build is more mature than an alpha build and `"beta" > "alpha"`.

## Project versioning

Project versionining is where a project is versioned independent of any other solution project.
Project versionining is a best for single project solutions.

See also: [Solution versioning](#solution-versioning)

## Release

What is a "release" depends on context. 
Semantic Versioning calls for certain version bumping after a release,
so understanding what a release is is important.

From the point of view of software artifact versioning, a release is when when a team releases tested artifacts to a customer.
In a CI environment a release build is typically a release candidate that may become the release. 
In a CD environment every (or almost every) release build is a release.

To define what a release is the **software team** needs to identify who is the customer of its artifact releases. 
Commonly it is another team within an organisation like:

* A product integration test team.
* Product management team that may include the artifacts in a product release.

A product release however may include a product version that is different to the software artifacts versioning.
A product releases are triggered by:

* Software achieving a MVP.
* When other components, that may not be software, are also available.
* Separate V&V cycles.
* Unforseen circumstances such as an urgent feature for new customer or an urgent bug fix.
* Commercial needs.

## Semver

![](../Images/SemVer122x64(dark).png)

An abbreviation for [Semantic Versioning](https://semver.org).


## Solution versioning

Solution versionining is where all projects in a solution share the same versioning.
That is all assemblies are versioned the same.
Solution versioning is best for solutions that generate one product.

See also: [Project versioning](#project-versioning)

## Uncontrolled build

A build made on an [uncontrolled host](#uncontrolled-host)

## Uncontrolled host

![](../Images/developer.png)

An uncontrolled host is a build host that is not a [controlled host](#controlled-host).
That is, at the time of a build, it is not known for certain what build tools, source code, and OS version was used.
Build logs and records are not kept.

Builds from an uncontrolled host may not be reproducable and the build environment may have unknown differences to the build production environment.

Typically developer boxes are uncontrolled hosts.

> [!NOTE]
> Git2SemVer creates a [build number](#build-number) on an uncontrolled host that is used in the context of that host.
> Hence it includes pre-release/metadata identifier with the host's name alongside the build number.
