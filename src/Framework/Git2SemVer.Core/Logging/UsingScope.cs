namespace NoeticTools.Git2SemVer.Core.Logging;

public sealed class UsingScope : IDisposable
{
    private readonly Action _leaveScope;

    public UsingScope(Action leaveScope)
    {
        _leaveScope = leaveScope;
    }

    public void Dispose()
    {
        _leaveScope();
    }
}