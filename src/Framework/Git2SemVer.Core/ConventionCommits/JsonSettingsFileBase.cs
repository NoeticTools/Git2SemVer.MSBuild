namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

public abstract class JsonSettingsFileBase<T>
    where T : new()
{
    public static T Load(string dataDirectory, string filename)
    {
        var filePath = Path.Combine(dataDirectory, filename);
        if (File.Exists(filePath))
        {
            return Load(filePath);
        }

        var config = new T();
        (config as JsonSettingsFileBase<T>)?.Save(dataDirectory, filename);
        return config;
    }

    private static T Load(string filePath)
    {
        return Git2SemVerJsonSerializer.Read<T>(filePath);
    }

    private void Save(string dataDirectory, string filename)
    {
        if (dataDirectory.Length > 0)
        {
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }
        }

        var filePath = Path.Combine(dataDirectory, filename);
        Git2SemVerJsonSerializer.Write(filePath, this);
    }
}