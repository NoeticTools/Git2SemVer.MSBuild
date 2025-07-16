---
uid: script-logging
---

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.MSBuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MsBuild)
<a href="https://github.com/NoeticTools/Git2SemVer">
  ![Static Badge](https://img.shields.io/badge/GitHub%20project-944248?logo=github)
</a>

# C# Script Logging

Log messages are are seen in the compiler output.

The C# script uses a `NoeticTools.Git2SemVer.Core.Logging.ILogger` logger instance
available on the ScriptRunner's context `NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting.VersioningContext.Logger` property.
This logger is available in both the script's global and class contexts.


## With runner instance (recommended)

```csharp
var runner = ScriptRunner.Instance!;
runner.Logger.LogImportantMessage("Hellow world);
```

## Global context

```csharp
Logger.LogImportantMessage("Hellow world);
```

---
