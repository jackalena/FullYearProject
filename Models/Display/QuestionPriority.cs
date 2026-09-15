namespace FullYearProject.Models.Display;

/// <summary>
///     Represents the possible priorities of a quiz question.
/// </summary>
public enum QuestionPriority
{
    /// <summary>
    ///     A question that has not been answered yet.
    /// </summary>
    Unanswered,

    /// <summary>
    ///     A question where the user has pressed skip.
    /// </summary>
    Skipped,

    /// <summary>
    ///     A question that has been answered incorrectly.
    /// </summary>
    Incorrect,

    /// <summary>
    ///     A question that has been answered correctly.
    /// </summary>
    Correct
}