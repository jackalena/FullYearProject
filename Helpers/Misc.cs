using System;

namespace FullYearProject.Helpers;

/// <summary>
///     Misc. extension methods.
/// </summary>
public static class Misc
{
    /// <summary>
    ///     Gets a random item from an array.
    /// </summary>
    /// <param name="random">The <see cref="Random" /> instance to use.</param>
    /// <param name="items">The array of items to get an item from.</param>
    /// <typeparam name="T">The type of the items in the array.</typeparam>
    /// <returns>A random item from the array.</returns>
    public static T GetItem<T>(this Random random, T[] items)
    {
        return items[random.Next(items.Length)];
    }
}