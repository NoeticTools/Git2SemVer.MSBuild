---
uid: changelog-gen-landing
---


# Git2SemVer changelog generation

**Git2SemVer** leverages [conventional commits](https://www.conventionalcommits.org/en/v1.0.0/) used to version .NET solutions and projects to
also generate a changelog for the upcoming release.

> [!NOTE]
> Currently only the **[Git2SemVer.Tool](xref:git2semver-tool-landing)** can generate a changelog.
> It is planned to add this functionality to **Git2SemVer.MSBuild** later 2025 so that the changelog is automatically updated on every build.

<div class="container-fluid mb-4 w-100">
    <div class="row row-cols-xs-2 row-cols-s-3 g-4">
        <div class="col">
            <div class="card" style="min-height: 140px; min-width: 170px">
                <div class="card-body" >
                    <p class="fw-semibold"><a href="/articles/ChangelogGen/GettingStarted/GettingStarted.html">Getting Started</a></p>
                    <p>Installing and quick start tutorials.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card" style="min-height: 140px; min-width: 170px" >
                <div class="card-body">
                    <p class="fw-semibold"><a href="/articles/ChangelogGen/Usage/Customising.html">Customising the changelog</a></p>
                    <p>Customising the changelog tutorials and documentation.</p>
                </div>
            </div>
        </div>
    </div>
</div>

An example generated draft (pre-release) changelog fragment:

![](../../Images/draft_changelog_fragment.png)
