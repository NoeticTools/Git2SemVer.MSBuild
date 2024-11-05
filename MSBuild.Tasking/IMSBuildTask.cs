using Microsoft.Build.Framework;


namespace NoeticTools.MSBuild.Tasking;

public interface IMSBuildTask
{
    /// <summary>
    ///     See
    ///     <see
    ///         href="https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.framework.ibuildengine6?view=msbuild-17-netcore">
    ///         Build
    ///         Framework IBuildEngine9
    ///     </see>
    /// </summary>
    IBuildEngine9 BuildEngine9 { get; }
}