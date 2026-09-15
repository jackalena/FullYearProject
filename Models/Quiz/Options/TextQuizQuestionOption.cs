namespace FullYearProject.Models.Quiz.Options;

/// <summary>
///     Represents a quiz question option that displays plain text.
/// </summary>
public class TextQuizQuestionOption : QuizQuestionOption
{
    /// <summary>
    ///     Initialises a new instance of the <see cref="TextQuizQuestionOption" /> class.
    /// </summary>
    public TextQuizQuestionOption()
    {
        Type = "Text";
    }
}