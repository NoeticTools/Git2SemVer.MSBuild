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
                    <p class="fw-semibold"><a href="/articles/ChangelogGen/Usage/Workflows.html">Workflows</a></p>
                    <p>changelog generation workflows tutorials and documentation.</p>
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


An example generated pre-release changelog fragment:

![](../../Images/draft_changelog_fragment.png)
