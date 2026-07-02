namespace FullYearProject.Models.Quiz.Question;

public class QuizQuestion
{
    public int Topic { get; set; } = -1;
    public string Text { get; set; } = "Question";
    public QuizQuestionOptionCollection Options { get; set; } = [];
    public string? Explanation { get; set; }
}