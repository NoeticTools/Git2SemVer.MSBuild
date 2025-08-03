---
uid: changelog-templating
---

# Changelog - Templating

**Git2SemVer** generates the changelog using [Scriban](https://github.com/scriban/scriban) scripting language template file `.git2semver/changelog.markdown.template.scriban`.
If no template is provided **Git2SemVer** will install and its default template file.

## Essentials

The template must generate a markdown file with section markers. 
Section markers are HTML comments that identify sections **Git2SemVer** can update. The section format is:

```html
<!-- Section start: section_name -->
    :
<!-- Section start: section_name -->
```

See section markers in the default template file `markdown.template.scriban`.

## Customising

The template file can be edited. A new changelog (or after the existing changelog is deleted) will use the edited template.
Any changes to the template's `next release` section will be seen when a new release is added to the changelog.
Any changes to the template's `version` section will impact the `next release` sections title/version on next changelog generation.

## Model

Values are passed to the [Scriban](https://github.com/scriban/scriban) template in a model. See class 
[ChangelogScribanModel](https://https://github.com/NoeticTools/Git2SemVer/blob/main/src/Framework/ChangeLogging/ChangelogScribanModel.cs).
