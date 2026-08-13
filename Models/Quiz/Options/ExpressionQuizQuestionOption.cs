namespace FullYearProject.Models.Quiz.Options;

public class ExpressionQuizQuestionOption : QuizQuestionOption
{
    public ExpressionQuizQuestionOption()
    {
        Type = "Expression";
    }

    /// <summary>
    ///     The format to use for displaying the option.
    /// </summary>
    public string? Format { get; set; }
}