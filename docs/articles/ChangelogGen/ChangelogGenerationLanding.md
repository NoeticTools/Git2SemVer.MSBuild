---
uid: changelog-gen-landing
---

# Git2SemVer changelog generation

**Git2SemVer** leverages [conventional commits](https://www.conventionalcommits.org/en/v1.0.0/) used to version .NET solutions and projects to
also generate a changelog for the upcoming release. It can generate a changelog for the next or current release incrementally during 
the development changes. The changelog to be manually edited during development. 
As new changes are made they are appended to the list of changes in the changelog and **Git2SemVer** 
keeps a list of handled [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) to avoid duplication.

<div class="container-fluid mb-4 w-100">
    <div class="row row-cols-xs-2 row-cols-sm-2 row-cols-md-4 g-4">
        <div class="col">
            <div class="card" style="min-height: 210px; min-width: 130px">
                <div class="card-body" >
                    <p class="fw-semibold"><a href="/articles/ChangelogGen/GettingStarted/GettingStarted.html">Getting Started</a></p>
                    <p>Installing and quick start tutorials.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card" style="min-height: 210px; min-width: 130px" >
                <div class="card-body">
                    <p class="fw-semibold"><a href="/articles/ChangelogGen/Usage/WorkflowsIncremental.html">Incremental generation</a></p>
                    <p>Tutorials and documentation on incremental changelog generation.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card" style="min-height: 210px; min-width: 130px" >
                <div class="card-body">
                    <p class="fw-semibold"><a href="/articles/ChangelogGen/Usage/WorkflowsTraditional.html">Traditional generation</a></p>
                    <p>Tutorials and documentation on traditional changelog generation.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card" style="min-height: 210px; min-width: 130px" >
                <div class="card-body">
                    <p class="fw-semibold"><a href="/articles/ChangelogGen/Usage/Customising.html">Customising the changelog</a></p>
                    <p>Customising the changelog tutorials and documentation.</p>
                </div>
            </div>
        </div>
    </div>
</div>

> [!NOTE]
> Currently only the **[Git2SemVer.Tool](xref:git2semver-tool-landing)** can generate a changelog.
> It is planned to add this functionality to **Git2SemVer.MSBuild** later 2025 so that the changelog is automatically updated on every build.

## Incremental

*Incremental changelog generation is the continuous integration aproach for changelogs.*

The changelog is kept in the Git repository and updated automatically.
New changes will still be appended as they occur and the changelog can still be
manually edited during release development.

As the changelog is in the repository is seen in commits and pull request reviews
and edited as required.

When **Git2SemVer** sees a [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)
it appends it to a new section of the changelog. It also records that commit summary as handled
so it will not be repeated or reappear if manually deleted.

See [Increment changelog workflow](xref:changelog-workflow-incremental) for tutorials and full documentaion.

## Traditional

**Git2SemVer** can also use the traditional approach of generating a changelog in one hit.

See [Traditional changelog workflow](xref:changelog-workflow-traditional) for tutorials and full documentaion.


An example generated pre-release [incremental changelog](xref:changelog-workflow-incremental) fragment:

![](../../Images/draft_changelog_fragment.png)
