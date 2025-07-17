---
uid: changelog-settings
---

#  Settings

**Git2SemVer** settings are found in file `git2semver.changelog.settings.json` in the `.git2semver\changelog` folder.

If the file does not exist **Git2SemVer** creates a default settings file as shown below:.

```json
{
  "Rev": "1",
  "IssueLinkFormat": "{0}",
  "AllowVariationsToSemVerStandard": false,
  "ConvCommits": {
    "FooterIssueTokens": [
      "issues",
      "issue",
      "ref",
      "refs"
    ]
  },
  "Categories": [
    {
      "ChangeTypePattern": "feat",
      "Name": "Added",
      "Order": 1
    },
    {
      "ChangeTypePattern": "change",
      "Name": "Changed",
      "Order": 2
    },
    {
      "ChangeTypePattern": "deprecate",
      "Name": "Depreciated",
      "Order": 3
    },
    {
      "ChangeTypePattern": "remove",
      "Name": "Removed",
      "Order": 4
    },
    {
      "ChangeTypePattern": "fix",
      "Name": "Fixed",
      "Order": 5
    },
    {
      "ChangeTypePattern": "security",
      "Name": "Security",
      "Order": 6
    },
    {
      "ChangeTypePattern": "^(?!dev|Dev|refactor).*$",
      "Name": "Other",
      "Order": 7
    }
  ]
}
```

## REV

The settings file `REV` property is for **Git2SemVer** internal use only. It represents tile schema revision number to allow for future migration.

## IssueLinkFormat

The `IssueLinkFormat` property provides a .NET string format to format issues retrieved from [ConvCommits](#convcommits) footer token to a desired markdown string.
The default value is `{0}` which does nothing.

Examples:

| IssueLinkFormat    | Issue    | Markdown               |
| :---               | :---     | :--                    |
| `{0}`              | 42, #43  | 42, #43                |
| `#{0}`             | 42       | #42                    |
| `https://organisation-name/project-name/issues/{0}` | #42  | \[#42](https://organisation-name/project-name/issues/#42)  |


## AllowVariationsToSemVerStandard

If `true`, variations to the Semantic Versioning standard are permitted. The default is `false`.

Currently this property is only used by the changelog generator when it detects a new prior release since the last changelog run and using incremental changelog generation.
If `AllowVariationsToSemVerStandard` is `false` **Git2SemVer** will generate a new changelog, discarding any manual edits.
Otherwise it will continue to add new changes to the incremental changelog.

## ConvCommits

The settings file `ConvCommits.FooterIssueTokens` property provides a list of [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) footer tokens (like `issues`) that may have comma delimited list of issues related to the change.

## Categories

The settings file `Categories` property has a prioritised array of [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) change type to changelog category mappings.
The map with the lowest `Order` value and matches change type is used.

Mapping entry properties are:

| Property           | Description                                                      |
| :---               | :---                                                             |
| ChangeTypePattern  | Regular express to match change type(s). This is case sensitive. |
| Name               | The changelog category name as it will appear in the changelog.  |
| Order              | The map's priority. Lowest value has highest priority.           |
 
If change's type does not match any mapper it is not included in the changelog.