---
uid: versioning-solution-versioning
---

# Solution versioning

## Setup

Solution versioning is when projects in a solution share the same versioning.
It synchonises the versioning of all project in a solution.

To a solution to use solution versioning run the [Git2SemVer.Tool](xref:git2semver-tool-landing) [`versioning setup add` command](xref:git2semver-tool-commands).
A new versioning project will be added and projects will be setup to use versioning information from the versioning project.

In the solution's directory, run:

```winbatch
git2semver versioning setup add
```

You will be prompted with a few options and then setup is done.

> [!IMPORTANT]
> Ensure the solution is not open in Visual Studio when running this command.
> Visual Studio only loads MSBuild tasks when the solution is opened.

Rebuild the solution, all projects will be automatically versioned. See [Versioning](xref:versioning-landing).

> [!TIP]
> Git2SemVer outputs the generated informational version to the compiler's output.


## Build numbering

On an uncontrolled host (typically a dev box) solution versioning will:

* Maintain a host build number that is used by all solutions.
* On a solution rebuild or on first build after cloning a repository, 
bump the build number and result in all assemblies having the same version.
* On a project build bump the build number if that project has already been built using the current build number.
* A project nuget pack always uses the same version as the project's build.

### Scenario - Multiple clones of a repository on a dev box

Preconditions: 

* The developer has two clones of a repository. Clone 1 and Clone 2.
* The last build number on the dev box was 1000.
* Last Clone 1 build, on this dev box, was 10.0.0-DevBox.900.
* Last Clone 2 build, on this dev box, was 20.0.0-DevBox.950.

Scenario:

| Action                              | Clone 1                | Clone 2 |
|:--                                  | :---:                  | :---:     |
| Clone 1 is rebuilt.                 | **10.0.0-DevBox.1001** | 20.0.0-DevBox.950  |
| Clone 1 is rebuilt (again).         | **10.0.0-DevBox.1002** | 20.0.0-DevBox.950  |
| Clone 2 is rebuilt.                 | 10.0.0-DevBox.1002     | **20.0.0-DevBox.1003** |


### Scenario - Solution with multiple projects on dev box

Preconditions: 

* The solution has 3 projects: Project A, Project B1, Project B2.
* Project B2 is dependent Project B1.
* The solution has been built. Build number was 1000.

Scenario:

Build numbers shown for each project.

| Action                                                    | Project A | Project B | Project C |
|:--                                                        | :---:     | :---:     | :---:     |
| Code is changed in project A. On build (not rebuild).        | **1001** | 1000     | 1000     |
| Code is changed in project B2. Solution build (not rebuild). | 1001     | **1002** | **1002** |
| Code is changed in project A. Solution build (not rebuild).  | **1002** | 1002     | 1002     |
| Solution rebuilt.                                            | **1003** | **1003** | **1003** |


### Scenario - Pack on dev box

Preconditions: 

* Last build number was 1000.

Scenario:

Build numbers shown for each project.

| Action                                                    | Build number |
|:--                                                        | :---:        |
| Project build (no pack).                                  | **1001**     |
| Project pack without build.                               | 1001         |
| Project build with pack.                                  | **1002**     |






