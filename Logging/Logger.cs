using System;
using Microsoft.Extensions.Logging;

namespace FullYearProject.Logging;

public static class Logger
{
    private static readonly ILoggerFactory _factory =
        LoggerFactory.Create(builder => builder.AddConsole());

    public static ILogger Create(string name)
    {
        return _factory.CreateLogger(name);
    }

    public static ILogger Create(Type type)
    {
        return _factory.CreateLogger(type);
    }

    public static ILogger Create<T>()
    {
        return _factory.CreateLogger<T>();
    }
}