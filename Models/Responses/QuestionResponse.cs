using System.Collections.Generic;

namespace FullYearProject.Models.Responses;

/// <summary>
///     Represents a response to a quiz question.
/// </summary>
public class QuestionResponse
{
    /// <summary>
    ///     The question that was answered.
    /// </summary>
    public string? Question { get; set; }

    /// <summary>
    ///     Whether the given answer is correct.
    /// </summary>
    public bool IsAnswerCorrect => Answers?.Exists(a => a.IsSelected && a.IsCorrect.EvaluateIsCorrect(null)) == true;

    /// <summary>
    ///     The possible answers to the question and their responses.
    /// </summary>
    public List<QuizQuestionOptionResponse>? Answers { get; set; }
}