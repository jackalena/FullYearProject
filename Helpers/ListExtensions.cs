using System;
using System.Collections.Generic;

namespace FullYearProject.Helpers;

public static class ListExtensions
{
    public static T RandomElement<T>(this IList<T> list, Random? random = null)
    {
        random ??= Random.Shared;

        return list[random.Next(list.Count)];
    }
}