---
uid: git2semver-tool-commands
---

# Git2SemVer.Tool Commands


```console
USAGE:
    git2semver [OPTIONS] <COMMAND>

EXAMPLES:
    git2semver versioning solution-setup add
    git2semver ver setup add
    git2semver ver setup add -u
    git2semver ver setup add -u --solution 'MyOtherSolution.sln'
    git2semver versioning install remove --solution 'MyOtherSolution.sln'

OPTIONS:
    -h, --help       Prints help information
    -v, --version    Prints version information

COMMANDS:
    versioning    Solution versioning commands (Alias 'ver')
    changelog     Generate changelog command
```

## Versioning

#### Synopsis


### Run

```console
DESCRIPTION:
Run version generator

USAGE:
    git2semver versioning run [OPTIONS]

OPTIONS:
                                     DEFAULT
    -h, --help                                  Prints help information
    -u, --unattended                            Unattened execution. Accepts all defaults
        --conv-commits-json-write               Enables writing found conventional commits to file 'conventionalcommits.g.json'. Used for changelog
                                                generation
        --enable-json-write                     Enables writing generated versions to file 'Git2SemVer.VersionInfo.g.json'
        --host-type <TYPE>                      Force the host type. Use for testing expected behaviour on other hosts. Valid values are 'Custom',
                                                'Uncontrolled', 'TeamCity', or 'GitHub'
    -o, --output <DIRECTORY>                    Directory in which to place the generated version JSON file and the build log
    -v, --verbosity <LEVEL>          info       Sets output verbosity. Valid values are 'trace', 'debug', 'info', 'warning', or 'error'
```

### Solution versioning - Add

Command to configure a solution for [solution versioning](xref:versioning-solution-versioning).

```console
DESCRIPTION:
Add Git2SemVer solution versioning to solution in working directory

USAGE:
    git2semver versioning solution-setup add [OPTIONS]

EXAMPLES:
    git2semver versioning solution-setup add
    git2semver ver setup add
    git2semver ver setup add -u
    git2semver ver setup add -u --solution 'MyOtherSolution.sln'

OPTIONS:
                        DEFAULT
    -h, --help                     Prints help information
    -u, --unattended               Unattened execution. Accepts all defaults
    -s, --solution                 Solution name. Optional, only required when there are multiple solutions in the working directory
```

> [!NOTE]
> If only using **Git2SemVer.Tool** for solution versioning setup, then it is not required in the build environment.

### Solution versioning - Remove

```console
DESCRIPTION:
Remove Git2SemVer solution versioning from solution in working directory

USAGE:
    git2semver versioning solution-setup remove [OPTIONS]

EXAMPLES:
    git2semver versioning setup remove --solution 'MyOtherSolution.sln'

OPTIONS:
                        DEFAULT
    -h, --help                     Prints help information
    -u, --unattended               Unattened execution. Accepts all defaults
    -s, --solution                 Solution name. Optional, only required when there are multiple solutions in the working directory
```


## Changelog generation

Command for [changelog generation](xref:changelog-gen-landing).


```console
DESCRIPTION:
Generate changelog command

USAGE:
    git2semver changelog run [OPTIONS]

OPTIONS:
                                        DEFAULT
    -h, --help                                                                    Prints help information
    -u, --unattended                                                              Unattened execution. Accepts all
                                                                                  defaults
    -a, --artifact-url <URL>            https://www.nuget.org/packages/user.pr    Optional url to a version's artifacts.
                                        oject/%VERSION%                           Must contain version placeholder
                                                                                  '%VERSION%'
    -m, --meta-directory <DIRECTORY>    .git2semver/changelog                     Directory in which to place the
                                                                                  generators metadata file
        --host-type <TYPE>                                                        Force the host type. Use for testing
                                                                                  expected behaviour on other hosts.
                                                                                  Valid values are 'Custom',
                                                                                  'Uncontrolled', 'TeamCity', or
                                                                                  'GitHub'
    -o, --output <FILEPATH>             CHANGELOG.md                              Generated changelog file path. May be
                                                                                  a relative or absolute path. Set to
                                                                                  empty string to disable file write
    -v, --verbosity <LEVEL>             info                                      Sets output verbosity. Valid values
                                                                                  are 'trace', 'debug', 'info',
                                                                                  'warning', or 'error'
    -c, --console-out                                                             Enable writing generated changelog to
                                                                                  the console
```
