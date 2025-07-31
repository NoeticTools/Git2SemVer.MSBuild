namespace NoeticTools.Git2SemVer.Core;

public static class DirectoryInfoExtensions
{
    public static FileInfo WithFile(this DirectoryInfo directory, string name)
    {
        return new FileInfo(Path.Combine(directory.FullName, name));
    }

    public static DirectoryInfo WithSubDirectory(this DirectoryInfo directory, string name)
    {
        return new DirectoryInfo(Path.Combine(directory.FullName, name));
    }
}