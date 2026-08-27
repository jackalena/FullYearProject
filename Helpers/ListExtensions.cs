using System;
using System.Collections.Generic;

namespace FullYearProject.Helpers;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;

            var k = Random.Shared.Next(n + 1);

            // Swap elements k and n.
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}