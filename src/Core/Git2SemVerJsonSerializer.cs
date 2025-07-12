using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;


namespace NoeticTools.Git2SemVer.Core;

public static class Git2SemVerJsonSerializer
{
    private static readonly Mutex FileMutex = new(false, "G2SemVerJsonFileMutex");

    private static readonly JsonSerializerOptions SerialiseOptions = new()
    {
        WriteIndented = true,
        IgnoreReadOnlyFields = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        
    };

    public static T Read<T>(string filePath) where T : new()
    {
        FileMutex.WaitOne(TimeSpan.FromSeconds(10));
        try
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(json)!;
            }
            else
            {
                return new T();
            }
        }
        finally
        {
            FileMutex.ReleaseMutex();
        }
    }

    public static void Write(string filePath, object target)
    {
        var json = JsonSerializer.Serialize(target, SerialiseOptions);

        FileMutex.WaitOne(TimeSpan.FromSeconds(10));
        try
        {
            File.WriteAllText(filePath, json);
        }
        finally
        {
            FileMutex.ReleaseMutex();
        }
    }
}