using System;
using System.Text.Json.Serialization;

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
    public string? Description { get; init; }

    /// <summary>
    ///     The time limit for the quiz.
    /// </summary>
    public TimeSpan TimeLimit { get; init; } = TimeSpan.FromMinutes(5);

    /// <summary>
    ///     The path to the file containing the quiz data.
    /// </summary>
    [JsonIgnore]
    public string? FileName { get; set; }
}