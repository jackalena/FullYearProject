using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FullYearProject.Helpers;
using FullYearProject.Logging;
using Microsoft.Extensions.Logging;

namespace FullYearProject.Models.Questions;

public class StringVariableFormatter
{
    private readonly ILogger _logger = Logger.Create<StringVariableFormatter>();

    private readonly StringBuilder _sb = new();

    public StringVariableFormatter(Dictionary<string, decimal> variables)
    {
        Variables = variables;
    }

    public Dictionary<string, decimal> Variables { get; }

    public string Format(string text)
    {
        _sb.Clear();
        _sb.Append(text);

        while (true)
        {
            var startIdx = _sb.IndexOf("@{");
            if (startIdx == -1 || _sb.TryGetChar(startIdx - 1) == '\\') break;

            var endIdx = _sb.IndexOf("}", startIdx);
            if (endIdx == -1)
            {
                _logger.LogError("Unclosed variable brace");
                break;
            }

            var len = endIdx - startIdx + 1;
            string? newVal = null;
            var formatSepIdx = _sb.IndexOf(":", startIdx + 2, endIdx - 1);

            if (formatSepIdx == -1)
            {
                var varName = _sb.ToString(startIdx + 2, len - 3);

                if (Variables.TryGetValue(varName, out var num)) newVal = num.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                var varName = _sb.ToString(startIdx + 2, formatSepIdx - startIdx - 2);
                var format = _sb.ToString(formatSepIdx + 1, endIdx - formatSepIdx - 2);

                if (Variables.TryGetValue(varName, out var num))
                    newVal = num.ToString($"{{0:{format}}}", CultureInfo.CurrentCulture);
            }

            if (newVal == null)
                _logger.LogError("Invalid variable name: {VariableName}", _sb.ToString(startIdx + 2, len - 3));

            _sb.ReplaceRange(startIdx, len, newVal ?? string.Empty);
        }

        return _sb.ToString();
    }
}