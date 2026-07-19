using FullYearProject.Models.Quiz.Question;

namespace FullYearProject.Models.Quiz;

/// <summary>
///     Represents a quiz.
/// </summary>
public class Quiz
{
    /// <summary>
    ///     The settings to use for the quiz.
    /// </summary>
    public QuizSettings Settings { get; set; } = new();

    /// <summary>
    ///     The topics in the quiz.
    /// </summary>
    public QuizTopicCollection Topics { get; set; } = [];

    /// <summary>
    ///     The questions in the quiz.
    /// </summary>
    public QuizQuestionCollection Questions { get; set; } = [];
}