---
uid: git2semver-tool-getting-started
---

# Git2SemVer.Tool - Getting Started

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.Tool?label=Git2SemVer.Tool)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.Tool)

## Prerequisites

Git2SemVer requires:

* `git` CLI to be executable from any project directory.
* `dotnet` CLI to be executable from any project directory.

Known compatibility:

* `dotnet.exe` `8.0.403` or later.
* `git` `2.41.0` or later.
* Windows 11
* Ubuntu 20.04 LTS

## Installing

To install:

```console
dotnet tool install --global NoeticTools.Git2SemVer.Tool
```

To update the tool to the latest:

```console
dotnet tool update NoeticTools.Git2SemVer.Tool --global
```

Once installed the tool is available as `git2semver`. 
To test the install, run the command:

```console
git2semver --version
```

<!--
<div class="container">
  <div class="row">
    <div class="col-sm-12">
      <div id="trailer" class="section d-flex justify-content-center embed-responsive embed-responsive-4by3">
        <video class="embed-responsive-item w-100" controls>
          <source src="/../../../images/Untitled video.mp4" type="video/mp4">
          Your browser does not support the video tag.
        </video>
      </div>
    </div>
  </div>
</div>
-->

## Quick start

To see versioning information, in your solution's directory, run:

```console
dotnet versioning run
```

For more information see [versioning](xref:versioning-landing).

To generate a changelog, in your solution's directory, run:

```console
dotnet changelog
```

For more information see [changelog generation](xref:changelog-gen-landing).

To investiage other commands use the `--help` option or view all commands [here](git2semver-tool-commands).

## Uninstalling

```console
dotnet tool uninstall NoeticTools.Git2SemVer.Tool --global
```
