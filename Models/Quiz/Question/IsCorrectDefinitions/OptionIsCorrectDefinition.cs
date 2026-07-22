using System.Collections.Generic;
using FullYearProject.Logging;

namespace FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

/// <summary>
///     Represents a definition of how to determine whether an option is correct.
/// </summary>
public abstract class OptionIsCorrectDefinition : Loggable
{
    public Dictionary<string, double> Variables { get; set; } = new();

    /// <summary>
    ///     Evaluates whether the option is correct or not.
    /// </summary>
    /// <param name="variables">The set of variables to use when determining if the option is correct.</param>
    /// <returns>A boolean value indicating whether the option is correct or not.</returns>
    public abstract bool EvaluateIsCorrect();
}