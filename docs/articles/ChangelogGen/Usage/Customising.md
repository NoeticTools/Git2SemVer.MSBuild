---
uid: changelog-customising
---

#  Customising

## Scriban template

**Git2SemVer** generates the changelog using [Scriban](https://github.com/scriban/scriban) scripting language template file `.git2semver/changelog/markdown.template.scriban`.
If no template is provided **Git2SemVer** will install and its default template file.

The template file can be edited to customise the generated changelog markdown.

For more details see [Templating](xref:changelog-templating).

## Release link

The url set by the **Git2SemVer.Tool** changelog command option `--artifact-url` is passed to the changelog template.
The default template uses this link to link a release version to site or artifact.
The default template does not use it for Unreleased (or pre-release) changelogs.

## Change categories

The changelog lists changes under categories like `Added` or `Fixed`. The categories used and the mapping of [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) change type to each category is configurable in the [settings file's Category property](xref:changelog-settings).

By default **Git2SemVer** lists changes in categories:

* `Added`
* `Changes`
* `Changed`
* `Depreciated`
* `Removed`
* `Fixed`
* `Security`
* `Other`

## Related issue numbers

Which footer tokens are used to provide related issue numbers is configurable in the [settings file's Categories property](xref:changelog-settings).
