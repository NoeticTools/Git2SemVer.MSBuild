using System.Diagnostics;


namespace NoeticTools.Git2SemVer.Testing.Core;

public static class TestHelper
{
    public static bool WaitUntil(Func<bool> predicate)
    {
        var stopwatch = Stopwatch.StartNew();
        while (!predicate())
        {
            if (stopwatch.Elapsed > TimeSpan.FromSeconds(30))
            {
                return false;
            }

            Thread.Sleep(5);
        }

        return true;
    }
}