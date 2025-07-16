---
uid: csharp-script
---

# C# script

On each build, Git2SemVer will load and executes a C# script (CS-Script) file.
This optional script can be used to change any Git2SemVer output.

> [!TIP]
> If your new to Git2SemVer's C# scripting and just want a quick answer to "why?".
> Have a look at the [example scripts](xref:example-scripts) first. 

## Script location

MSBuild property [Git2SemVer_ScriptPath](xref:versioning-msbuild-properties) provides the path to this C# script file.
It defaults to the file `Git2SemVer.csx` in the project folder.

The CS-Script file may be empty but if the file is not found the build fails.

Examples setting script path:

# [dotnet command line](#tab/dotnet)

```console
  dotnet build -p:Git2SemVer_ScriptPath=Build/MyVersioning.csx
```

# [MSBuild csproj file](#tab/msbuild)

```xml
<Git2SemVer_ScriptPath>$(MSBuildProjectDirectory)/Build/MyVersioning.csx</Git2SemVer_ScriptPath>
```

---

## API

The script is given reference to the versioning API `NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting.IVersioningContext`.
The API's properties are available as globals. 

An instance of the API is also available from the static `NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting.VersioningContext.Instance`.
This can be used for class context coding. 

Both global and class context approaches are explained in following sections.

`NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting.IVersioningContext` properties:

| Property     | Description   |
|:---          |:---           |
| Host         | A build host instance modeling the detected, or set, current host type. Provides a `BuildNumber` property.   |
| Inputs       | The task's MSBuild input property values. Includes a property for optional script arguments.          |
| Logger       | An MSBuild logger. Logging an error will cause the build to fail.                                     |
| Outputs      | Outputs that the script may optionally write to. These will be available to other MSBuild tasks.      |

`NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting.VersioningContext.Instance` is a static property that gives access to the VersioningContext instance. 
Required for class context code.


## Editing with syntax highlighting and intellisense

The Git2SemVer project source include a [EditingTestAssembly]() project that demonstrates one way to setup
a C# script editing project for syntax highlighting and itellisence (highly recommended for class context code), 
intellisense does not recognise global property types.

For a better editing experence work obtain the runner from static property instead.
It is the same instance but will be easier to edit.

> ![NOTE]
> It is planned to release a nuget package with the API binaries for editing projects.

## Using the global context (recommened)

The C# script has a global context and a class context.
The global context is given `NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting.IVersioningContext`
properties as globals.

For example the `Logger` property can be accessed as a global:

```csharp
Logger.LogInfo("Hello world.")
```

More examples can be found on the [example scripts](#example-scripts) section.

## Using the class context

The C# script has a global context and a class context. To use classes the runner instance needs to be passed to the class.

> [!TIP]
> Consider using the runner global instance instead.
> If your code is simple, using the runner in the global context may be simpler option.

Example:

```csharp
var context = VersioningContext.Instance!;

new MyVersioningClass(context).Run();

public class MyVersioningClass
{
    private IVersionContext _context;

    public MyVersioningClass(IVersionContext context)
    {
        _context = context;
    }

    public void Run()
    {
        _context.Logger.LogInfo("Hello world from my versioning class.")
    }
}
```

> [!NOTE]
> Global context properties and variables may not be available from within a class context.


## Example scripts

### Demo - Happy New Year

_A nonsense example to demonstrate capability.__

On new year's day beta builds (only), add a friendly _HappyNewYear_ message to the informational version.

```csharp
var now = DateTime.Now;
if (now.Month != 1 || now.Day != 1 ||
    !Outputs.PrereleaseLabel.Equals("beta"))
{
    return;
}

Logger.LogInfo("This is an beta build. HAPPY NEW YEAR!")

var identifier = new PrereleaseIdentifier("HappyNewYear");
var priorVersion = Ouputs.InformationalVersion;
var newVersion = priorVersion.InformationalVersion.WithPrerelease(priorVersion.PrereleaseIdentifiers.Append(identifier));
Outputs.InformationalVersion = version;
```

This will give an informational version like `1.2.3-alpha.5678.HappyNewYear+d1988132f8cd4abf2ff13658e7e484692e7f6822`.


### Demo - Change version to optional (custom) commit message value.

If the key `FORCE-VERSION: <version>` appears in the commit body, force all generated versions to the given `<major>.<minor>.<patch` version.

```csharp
var regex = new Regex(@"FORCE-VERSION: (?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)")
var match = regex.Match(Outputs.Git.HeadCommit.MessageBody);
if (!match.Success)
{
    return;
}

var major = int.Parse(match.Groups["major"].Value);
var minor = int.Parse(match.Groups["minor"].Value);
var patch = int.Parse(match.Groups["patch"].Value);

Logger.LogInfo($"Setting version {major}.{minor}.{patch}.")

var priorVersion = Ouputs.InformationalVersion;
var newVersion = new SemVersion(major, minor, patch,
                                priorVersion.PrereleaseIdentifiers,
                                priorVersion.MetadataIdentifiers);

Outputs.SetAllVersionPropertiesFrom(newVersion);
```

This script will update the `Outputs` properties:

* InformationalVersion
* Version
* AssemblyVersion
* FileVersion
* PackageVersion
* BuildSystemVersion
* PrereleaseLabel
* IsInInitialDevelopment

Prerelease identifiers, metadata identifiers, and build number are not changed.


### Demo - Change version to value from optional (custom) file.

If the key `FORCE-VERSION: <major>.<minor>.<patch` appears in the commit body, force all generated versions to the given version.

```csharp
const string filename = "version.txt";
if (!File.Exists(filename))
{
    return;
}

var content = File.ReadAllText(filename);
var elements = content.Split('.');
if (elements.Length != 3)
{
  Logger.LogError($"Invalid version in file {filename}. Expected single line <major>.<minor>.<patch>.")
}

var major = int.Parse(elements[0]);
var minor = int.Parse(elements[1]);
var patch = int.Parse(elements[2]);

Logger.LogInfo($"Setting version {version}.")

var priorVersion = Ouputs.InformationalVersion;
var newVersion = new SemVersion(major, minor, patch,
                                priorVersion.PrereleaseIdentifiers,
                                priorVersion.MetadataIdentifiers);

Outputs.SetAllVersionPropertiesFrom(newVersion);
```

This script will update the `Outputs` properties:

* InformationalVersion
* Version
* AssemblyVersion
* FileVersion
* PackageVersion
* BuildSystemVersion
* PrereleaseLabel
* IsInInitialDevelopment

Prerelease identifiers, metadata identifiers, and build number are not changed.


