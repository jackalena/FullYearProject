using System;

namespace FullYearProject.Helpers;

public static class Misc
{
    public static T GetItem<T>(this Random random, T[] items)
    {
        return items[random.Next(items.Length)];
    }

    public static T1 Apply<T1, T2>(this T2 obj, Func<T2, T1> func)
    {
        return func(obj);
    }
}