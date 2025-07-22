---
uid: git2semver-tool-installing
---

# Git2SemVer.Tool

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.Tool?label=Git2SemVer.Tool)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.Tool)

##  Prerequisites

**Git2SemVer.Tool** requires:

* `dotnet` CLI to be executable from any project directory.

Known compatibility:

* `dotnet.exe` `8.0.403` or later.
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

## Uninstalling


```console
dotnet tool uninstall NoeticTools.Git2SemVer.Tool --global
```
