﻿using System.Reflection;
using NoeticTools.Git2SemVer.Core.Exceptions;


// ReSharper disable MemberCanBePrivate.Global

namespace NoeticTools.Git2SemVer.Core;

public static class AssemblyExtensions
{
    public static string GetInformationalVersion(this Assembly assembly)
    {
        return assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    }

    public static string GetResourceFileContent(this Assembly assembly, string filename)
    {
        try
        {
            using var stream = assembly.GetResourceStream(filename);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        catch (Exception exception)
        {
            throw new Exception($"Unable to get resource file {filename}.", exception);
        }
    }

    public static Stream GetResourceStream(this Assembly assembly, string filename)
    {
        var resourcePath = assembly.GetManifestResourceNames()
                                   .SingleOrDefault(str => str.EndsWith(filename));
        if (resourcePath == null)
        {
            throw new Exception($"Resource file {filename} not found.");
        }

        try
        {
            var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                throw new Git2SemVerOperationException("Unable to open stream resource.");
            }

            return stream;
        }
        catch (Exception exception)
        {
            throw new Exception($"Unable to get resource file {filename}.", exception);
        }
    }

    public static void WriteResourceFile(this Assembly assembly, string resourceFilename, string destinationPath)
    {
        var content = assembly.GetResourceFileContent(resourceFilename);
        File.WriteAllText(destinationPath, content);
    }
}