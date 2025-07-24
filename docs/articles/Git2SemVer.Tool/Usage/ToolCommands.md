---
uid: git2semver-tool-commands
---

# Git2SemVer.Tool Commands


```console
USAGE:
    git2semver [OPTIONS] <COMMAND>

EXAMPLES:
    git2semver changelog run
    git2semver versioning run
    git2semver versioning setup add
    git2semver versioning setup add -confirm false --solution 'MyOtherSolution.sln'
    git2semver versioning setup remove --solution 'MyOtherSolution.sln'

OPTIONS:
    -h, --help       Prints help information
    -v, --version    Prints version information

COMMANDS:
    changelog     Changelog commands
    versioning    Solution versioning commands (alias 'ver')
```

## Versioning

### Run

```console
DESCRIPTION:
Run version generator command

USAGE:
    git2semver versioning run [OPTIONS]

EXAMPLES:
    git2semver versioning run

OPTIONS:
                                               DEFAULT
    -h, --help                                               Prints help information
    -u, --unattended                                         Run unattended. If used, does not ask user before operation is performed and
                                                             choices defaults are used
    -b, --branch-maturity-pattern <PATTERN>                  Optional regular expression value to map branch name to release and
                                                             prerelease labels
        --conv-commits-json-write                            Enables writing found conventional commits to file
                                                             'conventionalcommits.g.json'. Used for changelog generation
        --enable-json-write                                  Enables writing generated versions to file 'Git2SemVer.VersionInfo.g.json'
        --host-type <TYPE>                                   Force the host type. Use for testing expected behaviour on other hosts. Valid
                                                             values are 'Custom', 'Uncontrolled', 'TeamCity', or 'GitHub'
    -o, --output <DIRECTORY>                                 Directory in which to place the generated version JSON file and the build log
    -r, --release-tag-format <FORMAT>          v%VERSION%    Optional regular expression format to identify a release, and get the
                                                             version, from a Git tag's friendly name. Must include `%VERSION%` placeholder
                                                             text
    -v, --verbosity <LEVEL>                    info          Sets output verbosity. Valid values are 'trace', 'debug', 'info', 'warning',
                                                             or 'error'
```

### Solution versioning - Add

Command to configure a solution for [solution versioning](xref:versioning-solution-versioning).

```console
DESCRIPTION:
Add Git2SemVer solution versioning to solution in working directory

USAGE:
    git2semver versioning setup add [OPTIONS]

EXAMPLES:
    git2semver versioning setup add
    git2semver versioning setup add -confirm false --solution 'MyOtherSolution.sln'

OPTIONS:
                        DEFAULT
    -h, --help                     Prints help information
    -u, --unattended               Run unattended. If used, does not ask user before operation is performed and choices defaults are used
    -s, --solution                 Solution name. Optional, only required when there are multiple solutions in the working directory
```

> [!NOTE]
> If only using **Git2SemVer.Tool** for solution versioning setup, then it is not required in the build environment.

### Solution versioning - Remove

```console
DESCRIPTION:
Remove Git2SemVer solution versioning from solution in working directory

USAGE:
    git2semver versioning setup remove [OPTIONS]

EXAMPLES:
    git2semver versioning setup remove --solution 'MyOtherSolution.sln'

OPTIONS:
                        DEFAULT
    -h, --help                     Prints help information
    -u, --unattended               Run unattended. If used, does not ask user before operation is performed and choices defaults are used
    -s, --solution                 Solution name. Optional, only required when there are multiple solutions in the working directory
```


## Changelog generation

Command for [changelog generation](xref:changelog-gen-landing).


```console
DESCRIPTION:
Generate/update changelog command

USAGE:
    git2semver changelog run [OPTIONS]

EXAMPLES:
    git2semver changelog run

OPTIONS:
                                        DEFAULT
    -h, --help                                                   Prints help information
    -u, --unattended                                             Run unattended. If used, does not ask user before operation is performed
                                                                 and choices defaults are used
    -a, --artifact-url <URL>                                     Optional url to a version's artifacts. May contain version placeholder
                                                                 '%VERSION%'
    -d, --data-directory <DIRECTORY>    .git2semver/changelog    Path to generator's data and configuration files directory. May be a
                                                                 relative or absolute path
        --host-type <TYPE>                                       Force the host type. Use for testing expected behaviour on other hosts.
                                                                 Valid values are 'Custom', 'Uncontrolled', 'TeamCity', or 'GitHub'
    -o, --output <FILEPATH>             CHANGELOG.md             Generated changelog file path. May be a relative or absolute path. Set to
                                                                 empty string to disable file write
    -r, --release-as <TITLE>                                     If not an empty string sets the changes version (normally version or
                                                                 'Unreleased'). Any text permitted
    -v, --verbosity <LEVEL>             info                     Sets output verbosity. Valid values are 'trace', 'debug', 'info',
                                                                 'warning', or 'error'
    -s, --show                                                   Show changelog in console
```
