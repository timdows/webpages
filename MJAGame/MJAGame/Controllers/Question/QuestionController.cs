using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using MJAGame.Controllers.User;
using MJAGame.Data;

namespace MJAGame.Controllers.Question
{
    public class QuestionController : MJAGameController<MJAGameContext>
    {
        public JsonResult GetCurrent()
        {
            var questions = _dataContext.Questions
                .Include(a_item => a_item.Subject)
                .Where(a_item => a_item.Status == (int)Data.Models.Question.QuestionStatus.SubmitAnswer ||
                                           a_item.Status == (int)Data.Models.Question.QuestionStatus.SelectLies ||
                                           a_item.Status == (int)Data.Models.Question.QuestionStatus.ShowScores)
                .ToList();

            if (!questions.Any())
            {
                return Json(new {NoQuestionSelected = true});
            }

            // If there are more that two questions active, reset them
            if (questions.Count() > 1)
            {
                foreach (var qst in questions)
                {
                    qst.Status = 4;
                }
                _dataContext.SaveChanges();

                return Json(new { NoQuestionSelected = true });
            }

            var question = questions[0];

            if (question.Status == (int)Data.Models.Question.QuestionStatus.ShowScores)
            {
                return Json(new {ShowingScores = true, QuestionID = question.ID});
            }

            var formattedQuestion = CreateQuestion(question);

            var user = UserController.GetUserObject(Request);

            var answer = _dataContext.Answers
                .SingleOrDefault(a_item => a_item.User.ID == user.ID &&
                                           a_item.Question.ID == question.ID);

            if (answer == null)
            {
                answer = new Data.Models.Answer
                {
                    User = _dataContext.Users.Single(a_item => a_item.ID == user.ID),
                    Question = question
                };
            }

            var amountOfAnswers = _dataContext.Answers.Count(a_item => a_item.Question.ID == question.ID);

            // Fill the list with lies so the users can select one
            var selectLiesList = new List<object>();
            if (question.Status == (int) Data.Models.Question.QuestionStatus.SelectLies)
            {
                selectLiesList = _dataContext.Answers
                    .Where(a_item => a_item.Question.ID == question.ID && a_item.User.ID != user.ID)
                    .Select(CreateAnswer)
                    .ToList();
            }

            // Check if the user has selected a lie
            var answersOfQuestion = _dataContext.Answers
                .Include(a_item => a_item.ChosenBy)
                .Where(a_item => a_item.Question.ID == question.ID)
                .ToList();
            var userSelectedLie =
                answersOfQuestion.SingleOrDefault(
                    a_item => a_item.ChosenBy.SingleOrDefault(a_user => a_user.ID == user.ID) != null);

            return Json(new
            {
                Question = formattedQuestion,
                Answer = answer,
                AmountOfAnswers = amountOfAnswers,
                SelectLiesList = selectLiesList,
                UserSelectedLie = userSelectedLie
            } );
        }

        public JsonResult GetUserScores(long questionID)
        {
            //var user = UserController.GetUserObject(Request);
            var question = _dataContext.Questions
                .Include(a_item => a_item.Subject)
                .Include(a_item => a_item.SubmittedUser)
                .Single(a_item => a_item.ID == questionID);
            var correctAnswer = _dataContext.Answers
                .Include(a_item => a_item.User)
                .Include(a_item => a_item.ChosenBy)
                .SingleOrDefault(a_item => a_item.Question.ID == questionID && a_item.Correct);

            // No correct answer has been set by Marten
            if (correctAnswer == null)
            {
                return Json(true);
            }

            var answersOfQuestion = _dataContext.Answers
                .Include(a_item => a_item.User)
                .Include(a_item => a_item.ChosenBy)
                .Where(a_item => a_item.Question.ID == question.ID && !a_item.Correct)
                .ToList();

            return Json(new
            {
                question,
                correctAnswer,
                answersOfQuestion
            });
        }

        private static object CreateQuestion(Data.Models.Question question)
        {
            return new
            {
                question.ID,
                Subject = question.Subject.Name,
                Question = question.QuestionString,
                question.AmountOfAnswers,
                question.Status
            };
        }

        private static object CreateAnswer(Data.Models.Answer answer)
        {
            return new
            {
                answer.ID,
                Answer = answer.AnswerString
            };
        }
    }
}