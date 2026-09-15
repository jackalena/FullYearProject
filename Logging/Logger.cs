using System;

using Microsoft.Extensions.Logging;

namespace FullYearProject.Logging;

/// <summary>
///     Class to enable easy creation of loggers.
/// </summary>
public static class Logger
{
    private static readonly ILoggerFactory _factory =
        LoggerFactory.Create(builder => builder.AddConsole());

    /// <summary>
    ///     Creates a logger with the given name.
    /// </summary>
    /// <param name="name">The name of the logger.</param>
    /// <returns>The created <see cref="ILogger" /> instance.</returns>
    public static ILogger Create(string name)
    {
        return _factory.CreateLogger(name);
    }

    /// <summary>
    ///     Creates a logger with the given type.
    /// </summary>
    /// <param name="type">The type to use for the logger.</param>
    /// <returns>The created <see cref="ILogger" /> instance.</returns>
    public static ILogger Create(Type type)
    {
        return _factory.CreateLogger(type);
    }

    /// <summary>
    ///     Creates a logger with the given type.
    /// </summary>
    /// <typeparam name="T">The type to use for the logger.</typeparam>
    /// <returns>The created <see cref="ILogger" /> instance.</returns>
    public static ILogger Create<T>()
    {
        return _factory.CreateLogger<T>();
    }
}