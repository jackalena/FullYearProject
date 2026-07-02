using System;
using System.Linq;
using System.Text;

namespace FullYearProject.Helpers;

public static class StringBuilderExtensions
{
    /// <param name="sb">The <see cref="StringBuilder" /> instance to use.</param>
    extension(StringBuilder sb)
    {
        /// <summary>
        ///     Returns the index of the first occurrence of a substring in a string.
        /// </summary>
        /// <param name="str">The string to search for.</param>
        /// <param name="start">
        ///     The index of the first character to include in the search. Optional, defaults to the
        ///     start of the string.
        /// </param>
        /// <param name="end">
        ///     The last character to include in the search. Optional, defaults to the last character of
        ///     the string.
        /// </param>
        /// <returns>The index of the first occurence of <paramref name="str" />, or -1 if the substring is not found.</returns>
        public int IndexOf(string str, int start = 0, int end = int.MaxValue)
        {
            if (str.Length == 0)
            {
                return -1;
            }

            // Find the index of the last character in the string to search from
            var searchEnd = Math.Min(sb.Length, end) - str.Length;
            for (var i = start; i <= searchEnd; i++)
            {
                // Check if the substring matches at the current index
                var noMatch = str.Where((t, j) => sb[i + j] != t).Any();

                // If the substring matches, return the index
                if (!noMatch)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        ///     Replaces a range of characters in the string.
        /// </summary>
        /// <param name="startIndex">The index to start replacing characters from.</param>
        /// <param name="length">The number of characters to remove.</param>
        /// <param name="value">The new string to replace with.</param>
        /// <returns>The string with the range of characters replaced.</returns>
        public StringBuilder ReplaceRange(int startIndex, int length, string value)
        {
            return sb.Remove(startIndex, length).Insert(startIndex, value);
        }

        /// <summary>
        ///     Gets the char at the specified index, or null if the index is out of bounds.
        /// </summary>
        /// <param name="index">The index of the character to get.</param>
        /// <returns>The character and the specified index, or null</returns>
        public char? TryGetChar(int index)
        {
            if (index < 0 || index >= sb.Length)
            {
                return null;
            }

            return sb[index];
        }
    }
}