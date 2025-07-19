---
uid: changelog-templating
---

# Changelog - Templating

**Git2SemVer** generates the changelog using [Scriban](https://github.com/scriban/scriban) scripting language template file `.git2semver/changelog/markdown.template.scriban`.
If no template is provided **Git2SemVer** will install and its default template file.

The template file can be edited to customise the generated new changelog markdown. 
Changes to the template will probably not change incremental changelogs.

## Essentials

If using incremental changelog generations, the template must generate a markdown file with section markers. 
Section markers are HTML comments that identify sections **Git2SemVer** can update. The section format is:

```html
<!-- Section start: section_name -->
    :
<!-- Section start: section_name -->
```

See section markers in the default template file `markdown.template.scriban`.

## Model

Values are passed to the [Scriban](https://github.com/scriban/scriban) template in a model. See class 
[ChangelogScribanModel](https://https://github.com/NoeticTools/Git2SemVer.MSBuild/blob/main/src/Framework/ChangeLogging/ChangelogScribanModel.cs).
