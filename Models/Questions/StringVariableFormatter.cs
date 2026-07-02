using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FullYearProject.Helpers;
using FullYearProject.Logging;
using Microsoft.Extensions.Logging;

namespace FullYearProject.Models.Questions;

/// <summary>
///     Formats a string using a set of variables and their values.
/// </summary>
public class StringVariableFormatter
{
    private readonly ILogger _logger = Logger.Create<StringVariableFormatter>();

    // StringBuilder to use for string replacement
    private readonly StringBuilder _sb = new();

    /// <summary>
    ///     Creates a new StringVariableFormatter using the variables provided.
    /// </summary>
    /// <param name="variables">The variables and their values to use in replacements</param>
    public StringVariableFormatter(Dictionary<string, double> variables)
    {
        Variables = variables;
    }

    /// <summary>
    ///     The variables to use in replacements.
    /// </summary>
    public Dictionary<string, double> Variables { get; }

    /// <summary>
    ///     Formats a string using the values in <see cref="Variables" />, optionally applying a format string.
    /// </summary>
    /// <example>
    ///     If the provided string is "The value is @{value:F1}", and the value is 123.45, the formatted text will be
    ///     "The value is 123.4".
    /// </example>
    /// <param name="text">The text to format.</param>
    /// <returns>The formatted text with variables replaced with their values.</returns>
    public string Format(string text)
    {
        // Remove any existing text and add the new text
        _sb.Clear();
        _sb.Append(text);

        // Loop through the string until no more variables are found
        while (true)
        {
            // Find the start and end of the next variable
            var startIdx = _sb.IndexOf("@{");
            if (startIdx == -1 || _sb.TryGetChar(startIdx - 1) == '\\')
            {
                break;
            }

            var endIdx = _sb.IndexOf("}", startIdx);
            if (endIdx == -1)
            {
                _logger.LogError("Unclosed variable brace");
                break;
            }

            var len = endIdx - startIdx + 1;
            string? newVal = null;

            // Find the index of the ':' character to determine if a format string is provided
            var formatSepIdx = _sb.IndexOf(":", startIdx + 2, endIdx - 1);

            // If no format string is provided, use the default format string, otherwise use the format string provided
            if (formatSepIdx == -1)
            {
                // Find the name of the variable without the braces
                var varName = _sb.ToString(startIdx + 2, len - 3);

                // Get the value of the variable from the dictionary if it exists and convert it to a string to replace
                // in the provided string
                if (Variables.TryGetValue(varName, out var num))
                {
                    newVal = num.ToString(CultureInfo.CurrentCulture);
                }
            }
            else
            {
                // Find the name of the variable and the format string
                var varName = _sb.ToString(startIdx + 2, formatSepIdx - startIdx - 2);
                var format = _sb.ToString(formatSepIdx + 1, endIdx - formatSepIdx - 2);

                // Get the value of the variable from the dictionary if it exists and convert it to a string using
                // the provided format string
                if (Variables.TryGetValue(varName, out var num))
                {
                    newVal = num.ToString($"{{0:{format}}}", CultureInfo.CurrentCulture);
                }
            }

            // Replace the variable with its value or log an error if the variable doesn't exist
            if (newVal == null)
            {
                _logger.LogError("Invalid variable name: {VariableName}", _sb.ToString(startIdx + 2, len - 3));
            }
            else
            {
                _sb.ReplaceRange(startIdx, len, newVal);
            }
        }

        return _sb.ToString();
    }
}