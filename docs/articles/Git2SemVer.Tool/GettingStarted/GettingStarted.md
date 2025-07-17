---
uid: dotnet-tool-getting-started
---

# Git2SemVer.Tool - Getting Started

## First build

If you have not installed **Git2SemVer** and configured your test solution for solution versioning, do it now. Instructions are [here](xref:git2semver-tool-installing).

Your test solution must be under Git revision control.

Rebuild and you will see the generated version in compiler's output. It will be something like:

```console
Git2SemVer calculated version: 0.18.2-Alpha-InitialDev.MyPC.1422+Documentation-updates.3bda6f4fbedf2fe469da35b9f1a58146d4a36927
```

> [!IMPORTANT]
> If the message does not appear check that build output verbosity is at least `Normal` in Visual Studio `Tools | Options | Projects and Solutions`.
> The message will not appear if set to `Minimal` or `Quiet`.

> [!TIP]
> A versioning log `Git2SemVer.MSBuild.log` is written to the project's intermediate file folder (obj).
> This log includes information to show how the version was calculated.

### Build with custom C# script

Customise the versioning open the file `Git2SemVer.csx` in the `SolutionVersioning` (or the name given during setup) project and add:

```csharp
Log.LogInfo("Hello world - my first Git2SemVer C# script is born.");
```

Rebuild and you will the message `Hello world - my first After Burner C# script is born.` in the compiler's output.
If the message does not appear check that build output verbosity is at least `Normal` in Visual Studio `Tools | Options | Projects and Solutions`.
The message will not appear if set to `Minimal` or `Quiet`.

That was using C# script globals. `Log` is a global property. 
Another way is to use a context instance exposted by the `VersionContext` property:

```csharp
var context = VersioningContext.Instance!;

context.Logger.LogInfo("Hello world - my second Git2SemVer C# script is born.");
```

`VersioningContext.Instance` provides the global properties as a context object. It provides a better coding experience.
We will use the context object but the same can be done using the global properties.

> [!TIP]
> **Git2SemVer** has loaded common libraries and has setup implied using namespaces.
> Using statements may added to the file for more advanced editing.

As a simple example of seting assembly, file, informational, as well as package versions, replace the code in the csx file with:

```csharp
var context = VersioningContext.Instance!;
context.Logger.LogInfo("Running demo Git2SemVer customisation script.");

context.Outputs.SetAllVersionPropertiesFrom(SemVersion.ParseFrom("1.2.3-Demo.999+ASimpleDemoScriptVersion"));
```
 
 This will give you:

 <pre>
 Informational version:     1.2.3-Demo.999+ASimpleDemoScriptVersion
 Version:                   1.2.3-Demo.999
 AssemblyVersion:           1.2.3.0
 FileVersion:               1.2.3.0
 Build system version:      1.2.3-Demo.999
 Prereleaase label:         Demo
 Is in initial development: false</pre>

To demonstate reading **Git2SemVer** properties and overriding/customising everything replace the code in the csx file with:

[!code-csharp[](CsxDemos/ForceProperties4.csx)]


## Configuring build system

Your build system may need to be configured to make the build number available to **Git2SemVer**. See [Build Hosts](xref:build-hosts).