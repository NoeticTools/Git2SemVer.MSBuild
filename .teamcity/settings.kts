import jetbrains.buildServer.configs.kotlin.*
import jetbrains.buildServer.configs.kotlin.CustomChart
import jetbrains.buildServer.configs.kotlin.CustomChart.*
import jetbrains.buildServer.configs.kotlin.buildFeatures.nuGetFeedCredentials
import jetbrains.buildServer.configs.kotlin.buildFeatures.approval
import jetbrains.buildServer.configs.kotlin.buildFeatures.perfmon
import jetbrains.buildServer.configs.kotlin.buildFeatures.parallelTests
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetBuild
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetNugetPush
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetRestore
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetTest
import jetbrains.buildServer.configs.kotlin.buildSteps.script
import jetbrains.buildServer.configs.kotlin.buildTypeCustomChart
import jetbrains.buildServer.configs.kotlin.failureConditions.BuildFailureOnMetric
import jetbrains.buildServer.configs.kotlin.failureConditions.BuildFailureOnText
import jetbrains.buildServer.configs.kotlin.failureConditions.failOnMetricChange
import jetbrains.buildServer.configs.kotlin.failureConditions.failOnText
import jetbrains.buildServer.configs.kotlin.projectFeatures.githubIssues
import jetbrains.buildServer.configs.kotlin.triggers.vcs
import jetbrains.buildServer.configs.kotlin.vcs.GitVcsRoot

/*
The settings script is an entry point for defining a TeamCity
project hierarchy. The script should contain a single call to the
project() function with a Project instance or an init function as
an argument.

VcsRoots, BuildTypes, Templates, and subprojects can be
registered inside the project using the vcsRoot(), buildType(),
template(), and subProject() methods respectively.

To debug settings scripts in command-line, run the

    mvnDebug org.jetbrains.teamcity:teamcity-configs-maven-plugin:generate

command and attach your debugger to the port 8000.

To debug in IntelliJ Idea, open the 'Maven Projects' tool window (View
-> Tool Windows -> Maven Projects), find the generate task node
(Plugins -> teamcity-configs -> teamcity-configs:generate), the
'Debug' option is available in the context menu for the task.
*/

version = "2025.07"

project {
    description = "Git2SemVer Tool and MSBuild"

    vcsRoot(GithubNoetictoolsGit2semverRefsHeadsMain)

    subProject {
        id("All")
        name = "All"
        description = "Build and test MSBuild and Tool"

        val buildConfig = BuildAndTest("BuildAndTest", "")
        buildConfig.paused = false
        buildType(buildConfig)

        features {
            buildTypeCustomChart {
                id = "PROJECT_EXT_6"
                title = "Versioning time"
                seriesTitle = "Serie"
                format = CustomChart.Format.TEXT
                series = listOf(
                    Serie(title = "git2semver.runtime.seconds", key = SeriesKey("git2semver.runtime.seconds"))
                )
            }
        }
    }
}

class BuildAndTest(subId: String, solutionDir: String) : BuildType({
    name = "Build and test"
    id(subId)

    artifactRules = """
        +:artifacts/NoeticTools.*.nupkg
        +:src/MSBuild/SolutionVersioningProject/obj/Git2SemVer.MSBuild.log
        +:src/MSBuild/SolutionVersioningProject/.git2semver/Git2SemVer.VersionInfo.g.json
    """.trimIndent()

    params {
        param("BuildConfiguration", "Release")
    }

    vcs {
        root(DslContext.settingsRoot)
        checkoutMode
        cleanCheckout = true
    }

    steps {
        script {
            name = "Clear NuGet caches"
            id = "Clear_NuGet_caches"
            enabled = false
            scriptContent = "dotnet nuget locals all --clear"
        }
        dotnetRestore {
            name = "Restore"
            id = "Restore"
            sources = """
                https://api.nuget.org/v3/index.json
            """.trimIndent()
        }
        dotnetBuild {
            name = "Build"
            id = "dotnet"
            configuration = "%BuildConfiguration%"
            args = "-p:Git2SemVer_BuildNumber=%build.number% --verbosity normal"
            workingDir = solutionDir
        }
        dotnetTest {
            name = "Test"
            id = "dotnet_1"
            configuration = "%BuildConfiguration%"
            skipBuild = true
            args = "--logger console --logger teamcity"
            param("dotNetCoverage.dotCover.filters", """
                +:NoeticTools.*
                -:NoeticTools.*Tests
            """.trimIndent())
            workingDir = solutionDir
        }
    }

    triggers {
        vcs {
        }
    }

    failureConditions {
        executionTimeoutMin = 4
        failOnMetricChange {
            enabled = false
            metric = BuildFailureOnMetric.MetricType.TEST_COUNT
            threshold = 20
            units = BuildFailureOnMetric.MetricUnit.PERCENTS
            comparison = BuildFailureOnMetric.MetricComparison.LESS
            compareTo = build {
                buildRule = lastSuccessful()
            }
        }
        failOnMetricChange {
            metric = BuildFailureOnMetric.MetricType.ARTIFACT_SIZE
            threshold = 50
            units = BuildFailureOnMetric.MetricUnit.PERCENTS
            comparison = BuildFailureOnMetric.MetricComparison.LESS
            compareTo = build {
                buildRule = lastSuccessful()
            }
        }
        failOnText {
            conditionType = BuildFailureOnText.ConditionType.CONTAINS
            pattern = "The service message is invalid because it does not end with ] character"
            failureMessage = "Service message corruption detected"
            reverse = false
            stopBuildOnFailure = true
        }
    }

    features {
        perfmon {
        }
        nuGetFeedCredentials {
            feedUrl = "https://api.nuget.org/v3/index.json"
            username = "credentialsJSON:048b1358-2a0f-4d8f-917b-62869330ea79"
            password = "credentialsJSON:5577d5f6-64ef-4a22-868b-03a7d05985e6"
        }
    }

    requirements {
        exists("DotNetCLI_Path")
    }
})


object GithubNoetictoolsGit2semverRefsHeadsMain : GitVcsRoot({
    name = "https://github.com/noetictools/git2semver#refs/heads/main"
    url = "git@github.com:NoeticTools/Git2SemVer.git"
    branch = "refs/heads/main"
    branchSpec = """
        +:refs/heads/*
        +:refs/tags/*
    """.trimIndent()
    useTagsAsBranches = true
    checkoutPolicy = GitVcsRoot.AgentCheckoutPolicy.NO_MIRRORS
    authMethod = uploadedKey {
        uploadedKey = "Git2SemVerMSBuildWriteSSH"
    }
})
