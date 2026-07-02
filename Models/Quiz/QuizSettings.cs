using System;

namespace FullYearProject.Models.Quiz;

public class QuizSettings
{
    public string Title { get; set; } = "Quiz";
    public string? Description { get; set; }
    public TimeSpan TimeLimit { get; set; } = TimeSpan.FromMinutes(5);
}