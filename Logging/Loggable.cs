using Microsoft.Extensions.Logging;

namespace FullYearProject.Logging;

/// <summary>
///     A class that can be inherited to provide a lazy-loaded <see cref="ILogger" /> instance.
/// </summary>
public abstract class Loggable
{
    /// <summary>
    ///     An <see cref="ILogger" /> instance for the class.
    ///     Only created if accessed.
    /// </summary>
    protected ILogger Logger => field ??= Logging.Logger.Create(GetType());
}