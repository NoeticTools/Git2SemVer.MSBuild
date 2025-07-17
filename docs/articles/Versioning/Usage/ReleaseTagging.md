---
uid: release-tagging
---

<style>
th {
  text-align: left;
}
</style>

# Release version tagging

When a build is released commit must be tagged with release version tag.
Release tags are identified as tags with a friendly name that matches the _release tag format_.
This format is used to extract the  released version (`<major>.<minor>.<patch>`).

> [!TIP]
> Semantic versioning conveys meaning about underlying code and what has been modified to those who consume releases.
> What is "a release" needs to target on user/consumer that will benefit from knowing if the build has breaking changes, features, or only fixes.
>
> That user may be an internal customer (such as a testing team or other teams) or external users like the devlopment community consuming open source projects.
> Often marketing are focused on a product MVP and use driven naming versioning.
>
> Product naming and versioning is often best separated from software versioning.


## Default release tag format

The default release tag format is:

```console
  v<major>.<minor>.<patch>
```

Or, in regular expression format:
```console
  ^v(?<version>\d+\.\d+\.\d+)"
```

Default release version tag friendly name example matching:

<table>
    <thead>
        <tr>
            <th><span>&#10004;</span> Will match</th>
            <th><span>&#10060;</span> With NOT match</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>
                <code>
                    v1.2.3
                </code>
                <br/>
                <code>
                    v1.2.3 with red grapes
                </code>
            <td>
                <code>
                    1.2.3
                </code>
                <br/>
                <code>
                    release v1.2.3
                </code>
                <br/>
                <code>
                    release
                </code>
                <br/>
                <code>
                    v1.2
                </code>
            </td>
        </tr>
    </tbody>
</table>
For example, tag friendly names that will match this default format include:

```console
  v1.2.3
  v1.2.3 our big beautiful pink release
```

## Configuring the release tag format

The release tag format is configurable by setting the build property `Git2SemVer_ReleaseTagFormat` 
to a regular expressing that will match tags that have the desired friendly name.

> [!IMPORTANT]  
> This regular expression must:
>
> * Contain the version placeholder text `%VERSION%`.
> * NOT start with the reserved prefixes: `^`, `tag: `, or `.gsm`.

The default release tag format equivalent to `v%VERSION%`.
The `Git2SemVer_ReleaseTagFormat` build property is set the project file or in a directory build properties file like `Directory.Build.props`.

For example:
```xml
<PropertyGroup>
    <Git2SemVer_ReleaseTagFormat>MyRelease %VERSION%</Git2SemVer_ReleaseTagFormat>
</PropertyGroup>
```

Examples:

<table>
    <thead>
        <tr>
            <th>Git2SemVer_ReleaseTagFormat </th>
            <th><span>&#10004;</span> Will match</th>
            <th><span>&#10060;</span> With NOT match</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><code>%VERSION%</code></td>
            <td>
                <code>
                    1.2.3
                </code>
                <br/>
                <code>
                    1.2.3 with red grapes
                </code>
            <td>
                <code>
                    v1.2.3
                </code>
                <br/>
                <code>
                    release 1.2.3
                </code>
                <br/>
                <code>
                    release
                </code>
                <br/>
                <code>
                    1.2
                </code>
            </td>
        </tr>
        <tr>
            <td><code>release: %VERSION%</code></td>
            <td>
                <code>
                    release: 1.2.3
                </code>
                <br/>
                <code>
                    release: 1.2.3 with red apples
                </code>
            </td>
            <td>
                <code>
                    release 1.2.3
                </code>
                <br/>
                <code>
                    my release: 1.2.3
                </code>
            </td>
        </tr>
        <tr>
            <td rowspan=3><code>.*release: %VERSION%</code></td>
            <td>
                <code>
                    release: 1.2.3
                </code>
                <br/>
                <code>
                    my release: 1.2.3
                </code>
                <br/>
                <code>
                    green apples release: 1.2.3
                </code>
            </td>
            <td>
                <code>
                    release 1.2.3
                </code>
            </td>
        </tr>
    </tbody>
</table>

