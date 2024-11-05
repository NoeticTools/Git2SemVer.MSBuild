//using Microsoft.Build.Framework;
//using NoeticTools.Common.Exceptions;
//using ILogger = NoeticTools.Common.Logging.ILogger;


//namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;

//internal class VersionGeneratorInputs : IVersionGeneratorInputs
//{
//    public VersionGeneratorInputs(string mode,
//                                  string version,
//                                  string versionSuffix,
//                                  string buildNumber,
//                                  string buildContext,
//                                  string buildIdFormat,
//                                  bool updateHostBuildLabel,
//                                  string hostType,
//                                  bool? runScript,
//                                  string buildScriptPath,
//                                  string scriptArgs,
//                                  string branchMaturityPattern,
//                                  string gitDirectory,
//                                  string intermediateOutputPath,
//                                  string solutionSharedDirectory,
//                                  string solutionSharedVersioningPropsFile,
//                                  IBuildEngine9 buildEngine9)
//    {
//        BuildEngine9 = buildEngine9;
//        BuildContext = buildContext;
//        BuildIdFormat = buildIdFormat;
//        BuildNumber = buildNumber;
//        UpdateHostBuildLabel = updateHostBuildLabel;
//        HostType = hostType;
//        BuildScriptPath = buildScriptPath;
//        ScriptArgs = scriptArgs;
//        WorkingDirectory = gitDirectory;
//        Version = version;
//        VersionSuffix = versionSuffix;
//        IntermediateOutputDirectory = intermediateOutputPath;
//        SolutionSharedDirectory = solutionSharedDirectory;
//        SolutionSharedVersioningPropsFile = solutionSharedVersioningPropsFile;
//        RunScript = runScript ?? true;
//        BranchMaturityPattern = branchMaturityPattern;
//        try
//        {
//            VersioningMode = (VersioningMode)Enum.Parse(typeof(VersioningMode), mode);
//        }
//        catch (Exception exception)
//        {
//            throw new Git2SemVerConfigurationException($"Invalid Git2SemVer_Mode value '{mode}'.", exception);
//        }
//    }

//    public string BranchMaturityPattern { get; }

//    public string BuildContext { get; }

//    public IBuildEngine9 BuildEngine9 { get; }

//    public string BuildIdFormat { get; }

//    public string BuildNumber { get; }

//    public string BuildScriptPath { get; }

//    public string HostType { get; }

//    public string IntermediateOutputDirectory { get; }

//    public VersioningMode VersioningMode { get; set; }

//    public bool? RunScript { get; }

//    public string ScriptArgs { get; }

//    public string SolutionSharedDirectory { get; }

//    public string SolutionSharedVersioningPropsFile { get; }

//    public bool UpdateHostBuildLabel { get; }

//    public string Version { get; }

//    public string VersionSuffix { get; }

//    public string WorkingDirectory { get; }

//    public bool Validate(ILogger logger)
//    {
//        if (string.IsNullOrWhiteSpace(BuildScriptPath))
//        {
//            logger.LogError($"The script file path (property {nameof(BuildScriptPath)}) is required.");
//            return false;
//        }

//        if (RunScript is true && !File.Exists(BuildScriptPath))
//        {
//            logger.LogError($"The required build script file '{BuildScriptPath}' was not found.");
//            return false;
//        }

//        return true;
//    }
//}