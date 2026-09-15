using System;
using System.Collections.Generic;

namespace FullYearProject.Helpers;

/// <summary>
///     Extension methods for <see cref="IList{T}" />.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    ///     Randomises the order of items in a list.
    /// </summary>
    /// <param name="list">The list to shuffle.</param>
    /// <typeparam name="T">The type of list items.</typeparam>
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