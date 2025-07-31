namespace NoeticTools.Git2SemVer.Core;

#pragma warning disable CS8618

/// <summary>
///     Equivalent to LazyLoad. Provided for easier .NET framework / .NET multi-targeting builds.
/// </summary>
public sealed class LoadOnDemand<T>(Func<T> factory)
{
    private T _instance;
    private bool _loaded;

    public T Value
    {
        get
        {
            if (_loaded)
            {
                return _instance;
            }

            _instance = factory();
            _loaded = true;
            return _instance;
        }
    }
}