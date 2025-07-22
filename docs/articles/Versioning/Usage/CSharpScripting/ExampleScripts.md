---
uid: example-scripts
---

# Example C# Scripts

## Demo - Happy New Year

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


## Demo - Change version to optional (custom) commit message value.

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


## Demo - Change version to value from optional (custom) file.

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


