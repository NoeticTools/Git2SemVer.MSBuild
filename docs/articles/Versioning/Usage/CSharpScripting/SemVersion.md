---
uid: semversion
---

# SemVersion

Git2SemVer uses the [semver](https://www.nuget.org/packages/Semver/) Semmmantic Versioning library and makes this available to the optional C# Script.
This section provide examples for using this library.

For more information goto the [Semver project](https://github.com/WalkerCodeRanger/semver).

## Parsing

```csharp
var informationalVersion = SemVersion.Parse("1.1.0-beta.3471+e471d15", SemVersionStyles.Strict);

// gives: 1.1.0-beta.3471+e471d15
```

## Constructing

Constructing a version:

```csharp
var version = new SemVersion(1,1,0);
version = version.WithPrerelease("beta", "3471")
                 .WithMetadata(""e471d15");

// gives: 1.1.0-beta.3471+e471d15
```

## Manipulating

Get target release version (`<major>.<minor>.<patch>`) from a pre-release informational version:

```csharp
var informationalVersion = SemVersion.Parse("1.1.0-beta.3471+e471d15", SemVersionStyles.Strict);
var releaseVersion = informationalVersion.WithoutPrerelease()
                                         .WithoutMetadata();

// Gives: 1.1.0
```

Get a NuGet package vesion from a informational version:

```csharp
var informationalVersion = SemVersion.Parse("1.1.0-beta.3471+e471d15", SemVersionStyles.Strict);
var packageVersion = informationalVersion.WithoutMetadata();

// Gives: 1.1.0-beta.3471
```
