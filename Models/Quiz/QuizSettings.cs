using System;

namespace FullYearProject.Models.Quiz;

/// <summary>
///     Represents the settings for a quiz.
/// </summary>
public class QuizSettings
{
    /// <summary>
    ///     The title of the quiz.
    /// </summary>
    public string Title { get; set; } = "Quiz";

    /// <summary>
    ///     A description of the quiz.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     The time limit for the quiz.
    /// </summary>
    public TimeSpan TimeLimit { get; set; } = TimeSpan.FromMinutes(5);
}