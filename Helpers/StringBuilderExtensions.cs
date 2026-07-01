using System;
using System.Linq;
using System.Text;

namespace FullYearProject.Helpers;

public static class StringBuilderExtensions
{
    public static int IndexOf(this StringBuilder sb, string str, int start = 0, int end = int.MaxValue)
    {
        if (str.Length == 0) return start;

        var searchEnd = Math.Min(sb.Length, end) - str.Length;
        for (var i = start; i <= searchEnd; i++)
        {
            var noMatch = str.Where((t, j) => sb[i + j] != t).Any();

            if (!noMatch) return i;
        }

        return -1;
    }

    public static StringBuilder ReplaceRange(this StringBuilder sb, int startIndex, int length, string value)
    {
        return sb.Remove(startIndex, length).Insert(startIndex, value);
    }

    public static char? TryGetChar(this StringBuilder sb, int index)
    {
        if (index < 0 || index >= sb.Length) return null;
        return sb[index];
    }
}