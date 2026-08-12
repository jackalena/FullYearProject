using System.Collections.Generic;

using FullYearProject.Helpers;
using FullYearProject.Models.Quiz.Expressions;
using FullYearProject.Models.Quiz.Question;

namespace FullYearProject.Models.Display;

public class GeneratedQuizQuestion : QuizQuestion
{
    public QuizQuestion SourceQuestion { get; }

    public GeneratedQuizQuestion(QuizQuestion sourceQuestion, QuizQuestionOptionCollection options, string text, string? explanation)
    {
        SourceQuestion = sourceQuestion;
        Options = options;

        Text = text;
        Explanation = explanation;

        Topic = SourceQuestion.Topic;
    }

    public GeneratedQuizQuestion(QuizQuestion sourceQuestion) : this(sourceQuestion, sourceQuestion.Options, sourceQuestion.Text, sourceQuestion.Explanation) { }

    public static GeneratedQuizQuestion GenerateQuestion(QuizQuestion sourceQuestion)
    {
        if (sourceQuestion.Parameters == null)
        {
            return new(sourceQuestion);
        }

        Dictionary<string, double> parameters = new();

        foreach (var parameter in sourceQuestion.Parameters)
        {
            parameters[parameter.Name] = 0;

            foreach (var constraint in parameter.Constraints)
            {
                constraint.Variables = parameters;
                constraint.Apply(parameters[parameter.Name]);
            }
        }

        var formatter = new StringVariableFormatter(parameters);

        var questionText = formatter.Format(sourceQuestion.Text);
        var questionExplanation = sourceQuestion.Explanation?.Apply(formatter.Format);

        var questionOptions = new QuizQuestionOption[sourceQuestion.Options.Count];
    }
}