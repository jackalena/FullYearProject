using System.Collections.Generic;
using System.Linq;

namespace FullYearProject.Models.Quiz.Question;

public class QuizQuestionCollection : List<QuizQuestion>
{
    public QuizQuestion GetCorrectQuestion()
    {
        return this.First(q => q.Options.Any(o => o.IsCorrect.EvaluateIsCorrect(null)));
    }
}