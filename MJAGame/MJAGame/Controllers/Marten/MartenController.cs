using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MJAGame.Controllers.User;
using MJAGame.Data;
using MJAGame.Data.Models;
using WebGrease.Css.Extensions;

namespace MJAGame.Controllers.Marten
{
    public class MartenController : MJAGameController<MJAGameContext>
    {
        // List with all question to select from
        public JsonResult GetAllQuestions()
        {
            var questions = _dataContext.Questions
                .Include(a_item => a_item.Subject)
                .OrderBy(a_item => a_item.Subject.Name)
                .ThenBy(a_item => a_item.QuestionString)
                .ToList();

            foreach (var question in questions)
            {
                question.AmountOfAnswers = _dataContext.Answers
                    .Count(a_item => a_item.Question.ID == question.ID);

                var answers = _dataContext.Answers
                    .Include(a_item => a_item.ChosenBy)
                    .Where(a_item => a_item.Question.ID == question.ID)
                    .ToList();

                long amountOfLies = 0;
                foreach (var answer in answers)
                {
                    amountOfLies += answer.ChosenBy.Count();
                }
                question.AmountOfLies = amountOfLies;
            }

            return Json(questions.Select(CreateQuestion));
        }

        // Marten selects winning answer from this list
        public JsonResult GetAllAnswers(long questionID)
        {
            var question = _dataContext.Questions
                .Include(a_item => a_item.Subject)
                .Single(a_item => a_item.ID == questionID);

            var answers = _dataContext.Answers
                .Include(a_item => a_item.User)
                .Where(a_item => a_item.Question.ID == question.ID)
                .Select(CreateAnswer)
                .ToList();

            return Json(new {question, answers});
        }

        // Marten sets the winning answer
        public JsonResult SetWinningAnswer(long answerID, long questionID)
        {
            _dataContext.Answers
                .Where(a_item => a_item.Question.ID == questionID)
                .ForEach(a_item => a_item.Correct = false);

            var answer = _dataContext.Answers.Single(a_item => a_item.ID == answerID);
            answer.Correct = true;

            _dataContext.SaveChanges();
            return Json(true);
        }

        // Post to set the current selected question
        public JsonResult SetCurrent(long id)
        {
            _dataContext.Questions
                .Where(a_item => a_item.Status == (int) Data.Models.Question.QuestionStatus.SubmitAnswer)
                .ForEach(a_item => a_item.Status = 0);

            var existingQuestion = _dataContext.Questions.Single(a_item => a_item.ID == id);
            existingQuestion.Status = (int) Data.Models.Question.QuestionStatus.SubmitAnswer;
            _dataContext.SaveChanges();

            return Json(true);
        }

        // Post to set that this question goes to fase 2, users selecting lies and Marten selecting the winning
        public JsonResult SelectLies(long id)
        {
            // Remove all submit answers
            _dataContext.Questions
                .Where(a_item => a_item.Status == (int) Data.Models.Question.QuestionStatus.SubmitAnswer)
                .ForEach(a_item => a_item.Status = 0);

            var existingQuestion = _dataContext.Questions.Single(a_item => a_item.ID == id);
            existingQuestion.Status = (int) Data.Models.Question.QuestionStatus.SelectLies;
            _dataContext.SaveChanges();

            return Json(true);
        }

        // Post to stop the current lie selecting and show scores
        public JsonResult ShowScores(long id)
        {
            // Remove all SelectLies statusses
            _dataContext.Questions
                .Where(a_item => a_item.Status == (int) Data.Models.Question.QuestionStatus.SelectLies)
                .ForEach(a_item => a_item.Status = 0);

            var existingQuestion = _dataContext.Questions.Single(a_item => a_item.ID == id);
            existingQuestion.Status = (int) Data.Models.Question.QuestionStatus.ShowScores;
            _dataContext.SaveChanges();

            var users = _dataContext.Users.ToList();

            // Calculate scores
            // Every lie choosen by Marten is 500 points
            // Every lie by another persion is 100 points
            foreach (var user in users)
            {
                long points = 0;
                var question = _dataContext.Questions
                    .Include(a_item => a_item.Subject)
                    .Single(a_item => a_item.ID == id);

                var correctAnswer = _dataContext.Answers
                    .Include(a_item => a_item.User)
                    .Include(a_item => a_item.ChosenBy)
                    .SingleOrDefault(a_item => a_item.Question.ID == id && a_item.Correct);

                // When no correct answer has been selected
                if (correctAnswer == null)
                {
                    return Json(true);
                }

                if (correctAnswer.User.ID == user.ID)
                {
                    points += 500;
                    points += (100*correctAnswer.ChosenBy.Count());
                }
                else
                {
                    var userAnswer = _dataContext.Answers
                        .Include(a_item => a_item.ChosenBy)
                        .SingleOrDefault(a_item => a_item.Question.ID == id && a_item.User.ID == user.ID);

                    if (userAnswer != null)
                    {
                        points += (100 * userAnswer.ChosenBy.Count());
                    }
                }

                _dataContext.Scores.Add(new Data.Models.Score
                {
                    User = user,
                    Question = question,
                    Points = points
                });
                _dataContext.SaveChanges();
            }

            return Json(true);
        }

        // Post to stop the round
        public JsonResult EndRound(long id)
        {
            // Remove all SelectLies statusses
            _dataContext.Questions
                .Where(a_item => a_item.Status == (int) Data.Models.Question.QuestionStatus.ShowScores)
                .ForEach(a_item => a_item.Status = 0);

            var existingQuestion = _dataContext.Questions.Single(a_item => a_item.ID == id);
            existingQuestion.Status = (int) Data.Models.Question.QuestionStatus.Ended;
            _dataContext.SaveChanges();

            return Json(true);
        }

        private static object CreateQuestion(Data.Models.Question question)
        {
            return new
            {
                question.ID,
                Subject = question.Subject.Name,
                Question = question.QuestionString,
                question.AmountOfAnswers,
                question.AmountOfLies,
                question.Status
            };
        }

        private static object CreateAnswer(Data.Models.Answer answer)
        {
            return new
            {
                answer.ID,
                Answer = answer.AnswerString,
                UserName = answer.User.Name,
                answer.Correct
            };
        }
    }
}