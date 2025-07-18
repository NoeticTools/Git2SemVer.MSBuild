---
uid: changelog-workflow-incremental
---

# Changelog - Incremental Workflow

## Summary

An generated incremental changelog is kept in the Git repository and may be manually groomed at any time
during the release development cycle.

--- TODO ---

## Changelog generation

> [!NOTE]
> The [Scriban template file](xref:changelog-templating) determines the changlog layout and content.
> The documentation here assumes the default template is used.

### Release build

A incremental changelog generated on a release build:

* Shows the build version.
* Adds a link to the release artifact (see [artifact url option](xref:git2semver-tool-commands)).
* Hides all hint text that appears in pre-release changelogs.
* Lists all incremental changes after reviewed changes (one group).

A release changelog will look something like:

> <span style="font-size:x-large;font-weight: bold">Changelog</span>
> 
> All notable changes to this project will be documented in this file.
> 
> The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
> and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
> 
> <span style="font-size:large;font-weight: bold">1.2.3</span>
> 
> <span style="font-size:medium;font-weight: bold">Added</span>
> * Add changelog generation.
> * Add changelog user editable template.
> * Add versioning waypoint tagging (#55).
> * Support multiline footer topic values in conventional commits.
> 
> <span style="font-size:medium;font-weight: bold">Depreciated</span>
> 
> * Dotnet CLI tool versioning add, remove, and run commands depreciated. 
> 
>   Still functional but they will not appear in tool help.
>   Use new command under versioning command.
>   To be removed in next major release.

### Prerelease build

An incremental changelog generated on a pre-release build:

* Has an `Unreleased` header.
* Includes hint text.
* Keeps add incremental changes slightly separated from reviewed changes.

A pre-release changelog will look something like:

> <span style="font-size:x-large;font-weight: bold">Changelog</span>
> 
> All notable changes to this project will be documented in this file.
> 
> The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
> and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
> 
> <span style="font-size:large;font-weight: bold">Unreleased</span>
> 
> > This is a DRAFT changelog from a pre-release commit. It includes changes since last release.
> > HTML comments (``<!-- Start .. --> ... <!-- End .. -->``) mark sections in the markdown file used to make incremental changes.
> 
> <span style="font-size:medium;font-weight: bold">Added</span>
> * Add changelog generation.
> * Add changelog user editable template.
> 
> > Incremental changes, if any, will appear below. Move to above section when edited/reviewed.
> 
> * Add versioning waypoint tagging (#55).
> * Support multiline footer topic values in conventional commits.
> 
> <span style="font-size:medium;font-weight: bold">Depreciated</span>
> 
> * Dotnet CLI tool versioning add, remove, and run depreciated. 
> 
>   Still functional but they will not appear in tool help.
>   Use new command under versioning command.
>   To be removed in next major release.
> 
> > Incremental changes, if any, will appear below. Move to above section when edited/reviewed.

## New release detected

When **Git2SemVer** detects that there has been a new release since the last changelog generation run,
it starts a new changelog for the next release. The prior release's changelog and data are discarded.
If no changes have been made since the last release, the new changelog will be empty.

An incremental changelog is a draft changelog of changes from the prior releasee up to the head commit.

To maintain changelog accross multiple releases the options are:

* On the release's commit use the changelog command `new-release` and commit the change. 
This will create a new `Unreleased` section and move changes to just below the `Unreleased` section.

## New change detected

On a release build, new changes are appended to the existing changes list.

On a pre-release build, new change are appended to a list of incremental changes that is kept below
the manually reviewed changes list.

A pre-release changelog fragment example showing 2 reviewed changes and 2 incremental changes:

> <span style="font-size:medium;font-weight: bold">Added</span>
> * Add changelog generation.
> * Add changelog user editable template.
> 
> > Incremental changes, if any, will appear below. Move to above section when edited/reviewed.
> 
> * Add versioning waypoint tagging (#55).
> * Support multiline footer topic values in conventional commits.


## Grooming the changelog

An generated incremental changelog is kept in the Git repository and may be edited (groomed) at any time
during the release development cycle.

As changes are added the incremental changes lists in each category (like `Added`) will grow.
Being in the repository these changes have visibility in commits and pull request reviews.
This promotes early grooming.

Sections in the markdown file are marked by HTML commits like:

> <span style="font-size:medium;font-weight: bold">Added</span>
>
> **<!- Section start: Added changes -->**
> * Add changelog generation.
> * Add changelog user editable template.
> 
> **<!- Section end: Added changes -->**
>
> > Incremental changes, if any, will appear below. Move to above section when edited/reviewed.
> 
> **<!- Section start: Added changes, for manual review -->**
> * Add versioning waypoint tagging (#55).
> * Support multiline footer topic values in conventional commits.
>
> **<!- Section end: Added changes, for manual review -->**

When grooming a change move it out of the incremental section and into the changes list above.
For example the above changelog may be groomed to:

> #### Added
>
> **<!- Section start: Added changes -->**
> * Add versioning waypoint tagging (#55).
> * Add changelog generation (#145).
> * Add conventional commits multiline footer support (#99).
> 
> **<!- Section end: Added changes -->**
> 
> > Incremental changes, if any, will appear below. Move to above section when edited/reviewed.
> 
> **<!- Section start: Added changes, for manual review -->**
> 
> **<!- Section end: Added changes, for manual review -->**

> [!NOTE]
> `<---` syntax is shown here as `<-`.

## Q&A

TODO

- Duplicates
