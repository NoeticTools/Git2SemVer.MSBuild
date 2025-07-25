---
uid: msbuild-properties
---

<style>
.propertyCol {
  font-size:1.0em;
}

.headerCell {
  font-size:1.0em;
  font-weight:bold;
}

.optionalCol {
  margin-top:3px;
  font-size:1.0em;
}

.descriptionCol {
  margin-top:0px;
}

p {
  margin-bottom:5px;
}

table, tr, td {
  border:none !important;
}

a 
{
  text-decoration: none; 
}
</style>

# MSBuild properties

## Inputs

Input properties are passed to the Git2SemVer MSBuild task.
A property may be defined within a `csproj` file or on a command line like:

```
  dotnet build -p:Git2SemVer_UpdateHostBuildLabel=true
```

Or, in the `csproj` file like:

```
  <PropertyGroup>
    <Git2SemVer_UpdateHostBuildLabel>true</Git2SemVer_UpdateHostBuildLabel>
  </PropertyGroup>
```

<br/>

The MSBuild input poperties are:

<div style="margin:5px;">
 <table>
 <tr>
    <td>
      <div class="headerCell">
        Property
      </div>
    </td>
    <td>
      <div class="headerCell">
        Description
      </div>
    </td>
  </tr>

  <tr>
    <td>
        <div class="propertyCol">
            <a name="build-context"/>
            Git2SemVer_BuildContext
        </div>
    </td>
    <td>
        <div class="descriptionCol">
        <p>
            If set, overrides the host's BuildContext property.
        </p><p>
            The build context gives context to the build number.
            On a build system that provides a build number, like TeamCity, it is not required or set to "0".
            On an uncontrolled host (dev box) it is the host's machine name as the build number only makes sense in the context of that host.
        </p>
        </div>
    </td>
  </tr>

  </tr>
    <td>
        <div class="propertyCol">
            <a name="build-id-format"/>
            Git2SemVer_BuildIDFormat
        </div>
    </td>
    <td>
        <div class="descriptionCol">
            <p>
                If set overrides the build host's BuildIdFormat property to overrides how the host formats build ID
            </p><p>
                A a dot delimited string of identifiers. Identifiers <code>BUILD_NUMBER</code> and <code>BUILD_CONTEXT</code> are replaced with the values of the host's build number and build contxt.
            </p><p>
                Example values: <code>BUILD_NUMBER.BUILD_CONTEXT</code>, <code>BUILD_NUMBER.BUILD_CONTEXT</code>, <code>BUILD_NUMBER</code>, or <code>MyBuildId</code>.
            </p><p>
                Default is build host dependent.
            </p>
        </div>
    </td>
  </tr>

  <tr>
    <td>
        <div class="propertyCol">
            <a name="build-number"/>
            Git2SemVer_BuildNumber
        </div>
    </td>
    <td>
        <div class="descriptionCol">
        <p>
            Optional value that overrides the host's BuildNumber property.
        </p><p>
        Use this to pass in build number from a build system. Not required if using TeamCity.
        </p>
        </div>
    </td>
  </tr>

  <tr>
    <td>
        <div class="propertyCol">
            <a name="branch-maturity-pattern"/>
            Git2SemVer_BranchMaturityPattern
        </div>
    </td>
    <td>
        <div class="descriptionCol">
        <p>
            Optional regular expression value to map branch name to release and prerelease labels.
        </p><p>
            Default is <code>^((?<release>main|release)|(?<rc>(main|release)[\\/_]rc)|(?<beta>feature)|(?<alpha>.+))[\\/_]?</code>
        </p><p>
            The <code>release</code> group is required.
            All others a prereleases and the group name is the prerelease label.
        </p>
        </div>
    </td>
  </tr>

  <tr>
    <td>
        <div class="propertyCol">
            Git2SemVer_Disable
        </div>
    </td>
    <td>
        <div class="descriptionCol">
            Set to true to disable Git2SemVer. Default is false.
        </div>
    </td>
  </tr>

  <tr>
    <td>
        <a name="host-type"/>
        <div class="propertyCol">
            Git2SemVer_HostType
        </div>
    </td>
    <td>
        <div class="descriptionCol">
            If set, overrides automatic host detection. Value values are "Uncontrolled", "TeamCity", or "GitHub".
        </div>
    </td>
  </tr>

  <tr>
    <td>
        <div class="propertyCol">
            Git2SemVer_ScriptArg
        </div>
    </td>
    <td>
        <div class="descriptionCol">
        A string made that Git2SemVer passes on for the C# script to use.
        Not used by Git2SemVer.
        </div>
    </td>
  </tr>
  <tr>
    <td>
        <div class="propertyCol">
            Git2SemVer_ScriptPath
        </div>
    </td>
    <td>
        <div class="descriptionCol">
            <p>
                Path, relative to the project's folder, to the C# script file.
                Provided to allow the script file use a custom name and/or be moved to another folder.
            </p><p>
                Default is Git2SemVer.csx in the project's folder.
            </p>
        </div>
    </td>
  </tr>
  <tr>
    <td>
        <div class="propertyCol">
            Git2SemVer_UpdateHostBuildLabel
        </div>
    </td>
    <td>
        <div class="descriptionCol">
            <p>
                Only used if building on a host, such as a TeamCity agent, that supports setting the build's label/version.
                Then if set to true, and build label will be set to the generated build system version.
            </p>
            <p>
                Default is false. To update the build system's label set this on the command line or in the csproj file.
            </p>
        </div>
    </td>
  </tr>
  <tr>
    <td>
        <div class="propertyCol">
            Git2SemVer_ReleaseTagFormat
        </div>
    </td>
    <td>
        <div class="descriptionCol">
            <p>
                Optional regular expression format to identify a release tag, and get the version, from a Git tag's friendly name.
                Must include the version placeholder text "%VERSION%".
            </p>
            <p>
                Default is "v%VERSION%"` which will match "v1.2.3".
            </p>
            <p>
                See: <a href="https://noetictools.github.io/Git2SemVer/articles/Versioning/Usage/ReleaseTagging.html">Release Tagging</a>
            </p>
            <p>
                <i>Available in Git2SemVer.MSBuild 3.1.0 or later.</i>
            </p>
        </div>
    </td>
  </tr>

</table> 
</div>


## Outputs

Git2SemVer's MSBuild task outputs version information to MSBuild properties.
Some of these properties are standard MSBuild properties used by the compiler.
The custom properties are provided for third party MSBuild task use.

### Standard versioning properties

MSBuild standard versioning properties set are:

 * AssemblyVersion
 * FileVersion
 * InformationalVersion
 * PackageVersion
 * Version
 * VersionPrefix
 * VersionSuffix

More information can be found <a href="https://gist.github.com/jonlabelle/34993ee032c26420a0943b1c9d106cdc">here</a>.

### Custom properties

Custom properties set for use by other scripts are:

<div style="margin:5px;">
 <table>
  <tr>
    <td>
      <div class="headerCell">
        Property
      </div>
    </td>
    <td>
      <div class="headerCell">
        Description
      </div>
    </td>
  </tr>

  <tr>
    <td>
        <div class="propertyCol">
            Git2SemVer_Output1
        </div>
    </td>
    <td>
        <div class="descriptionCol">
          <p>
            An output value that may be set by C# script (context.Outputs.Output1).
          </p><p>
            Default is an empty string.
          </p><p>
            Not used by Git2SemVer.
          </p>
        </div>
    </td>
  </tr>
  <tr>
    <td>
        <div class="propertyCol">
            Git2SemVer_Output2
        </div>
    </td>
    <td>
        <div class="descriptionCol">
          <p>
            An output value that may be set by C# script (context.Outputs.Output1).
          <p/><p>
            Default is an empty string.
          </p><p>
            Not used by Git2SemVer.
          </p>
        </div>
     </td>
    </tr>
  </table> 
</div>

