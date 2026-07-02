using FullYearProject.Models.Quiz.Question;

namespace FullYearProject.Models.Quiz;

public class Quiz
{
    public QuizSettings Settings { get; set; } = new();
    public QuizTopicCollection Topics { get; set; } = [];
    public QuizQuestionCollection Questions { get; set; } = [];
}