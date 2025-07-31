﻿using System.Diagnostics.CodeAnalysis;
using NoeticTools.Git2SemVer.Core.Exceptions;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging.Exceptions;

[ExcludeFromCodeCoverage]
public class Git2SemVerScribanFileParsingException : Git2SemverExceptionBase
{
    public Git2SemVerScribanFileParsingException(string message) : base(message)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public Git2SemVerScribanFileParsingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}