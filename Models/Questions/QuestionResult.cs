using System.Collections.Generic;

namespace FullYearProject.Models.Questions;

public class QuestionResult
{
    public string? Question { get; set; }
    public bool IsCorrect => Answers?.Exists(a => a is { IsCorrect: true, IsSelected: true }) == true;
    public List<AnswerOption>? Answers { get; set; }
}