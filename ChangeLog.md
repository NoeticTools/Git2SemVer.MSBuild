# Git2SemVer.MSBuild Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## 1.2.0 - _November 26, 2024_

### Changed

* Changed the build system label, as used by TeamCity, to include build number on release builds (#33).
* Moved Git2SemVer.Tool (dotnet tool) code to its own repository.

### Known Issues

* Versioning fails build when TeamCity build configuration's VCS Root uses the option "Use tags as branches" is set ([#32](https://github.com/NoeticTools/Git2SemVer/issues/32)). A compile time exception is thrown.

 
## 1.1.0 - _November 16, 2024_

### Added

* Added [Cross targeted projects](https://learn.microsoft.com/en-us/nuget/create-packages/multiple-target-frameworks-project-file) support ([#26](https://github.com/NoeticTools/Git2SemVer/issues/26)).
* Added versioning log file ([#26](https://github.com/NoeticTools/Git2SemVer/issues/2)).
* Added [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0-beta.2/) support for versioning.

### Fixed

* On an uncontrolled host with solution open in Visual Studio, build number updates in background every few seconds ([#28](https://github.com/NoeticTools/Git2SemVer/issues/28)).
* On an uncontrolled host prior build number is reused on first build after new clone ([#27](https://github.com/NoeticTools/Git2SemVer/issues/27)).
* ([#24](https://github.com/NoeticTools/Git2SemVer/issues/24)).


### Known Issues

* Solution configured using `Git2SemVer.Tool` dotnet tool 1.0.0 does not allow for `Git2SemVer.MSBuild` package. Requires manual correction ([#25](https://github.com/NoeticTools/Git2SemVer/issues/25)).


## 1.0.0 - _November 7, 2024_

First release.

### Added

* Visual Studio & dotnet CLI solution Semmantic versioning
* Build hosts:
  * TeamCity - detects build number and updates build label
  * GitHub - supports composite build numbering
  * Uncontrolled (dev box) - local build numbering
* C# scripting
* Git workflow agnostic

### Known Issues

* Does not work with [cross targeted projects](https://learn.microsoft.com/en-us/nuget/create-packages/multiple-target-frameworks-project-file).
See issue [#26](https://github.com/NoeticTools/Git2SemVer/issues/26).
