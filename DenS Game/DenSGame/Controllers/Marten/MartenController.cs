using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DenSGame.Controllers.User;
using DenSGame.Data;
using DenSGame.Data.Models;
using WebGrease.Css.Extensions;
using System.Collections.Generic;

namespace DenSGame.Controllers.Marten
{
    public class MartenController : DenSGameController<DenSGameContext>
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
                question.AmountOfAnswers = GetAmountOfAnswers(question);
                question.AmountOfLies = GetAmountOfLies(question);
            }

            var runningQuestion = _dataContext.Questions
                .SingleOrDefault(a_item => a_item.Status == 1 || a_item.Status == 2 || a_item.Status == 3);
            if(runningQuestion != null)
            {
                runningQuestion.AmountOfAnswers = GetAmountOfAnswers(runningQuestion);
                runningQuestion.AmountOfLies = GetAmountOfLies(runningQuestion);
                runningQuestion.AnsweredByDennis = _dataContext.Answers
                    .Any(a_item => a_item.Question.ID == runningQuestion.ID && a_item.PickedByDennis);
                runningQuestion.AnsweredBySandra = _dataContext.Answers
                    .Any(a_item => a_item.Question.ID == runningQuestion.ID && a_item.PickedBySandra);
            }
            
            return Json(new
            {
                runningQuestion = runningQuestion,
                questions = questions.Select(CreateQuestion)
            });
        }

        private long GetAmountOfAnswers(Data.Models.Question question)
        {
            return _dataContext.Answers
                .Count(a_item => a_item.Question.ID == question.ID);    
        }

        private long GetAmountOfLies(Data.Models.Question question)
        {
            var answers = _dataContext.Answers
                    .Include(a_item => a_item.ChosenBy)
                    .Where(a_item => a_item.Question.ID == question.ID)
                    .ToList();

            long amountOfLies = 0;
            foreach (var answer in answers)
            {
                amountOfLies += answer.ChosenBy.Count();
            }
            return amountOfLies;
        }

        public JsonResult GetAllAnswers()
        {
            var question = _dataContext.Questions
                .Include(a_item => a_item.Subject)
                .SingleOrDefault(a_item => a_item.Status == 1 || a_item.Status == 2);

            var showingScores = false;
            if (_dataContext.Questions.Any(a_item => a_item.Status == (int)Data.Models.Question.QuestionStatus.ShowScores))
            {
                showingScores = true;
            }

            var answers = new List<object>();
            if (question != null)
            {
                answers = _dataContext.Answers
                .Include(a_item => a_item.User)
                .Where(a_item => a_item.Question.ID == question.ID)
                .Select(CreateAnswer)
                .ToList();
            }

            return Json(new { showingScores, question, answers});
        }

        // Marten sets the winning answer
        /*public JsonResult SetWinningAnswer(long answerID, long questionID)
        {
            _dataContext.Answers
                .Where(a_item => a_item.Question.ID == questionID)
                .ForEach(a_item => a_item.Correct = false);

            var answer = _dataContext.Answers.Single(a_item => a_item.ID == answerID);
            answer.Correct = true;

            _dataContext.SaveChanges();
            return Json(true);
        }*/

        public JsonResult DennisSetsAnswer(long answerID, long questionID)
        {
            _dataContext.Answers
                .Where(a_item => a_item.Question.ID == questionID)
                .ForEach(a_item => a_item.PickedByDennis = false);

            var answer = _dataContext.Answers.Single(a_item => a_item.ID == answerID);
            answer.PickedByDennis = true;

            _dataContext.SaveChanges();
            return Json(true);
        }

        public JsonResult SandraSetsAnswer(long answerID, long questionID)
        {
            _dataContext.Answers
                .Where(a_item => a_item.Question.ID == questionID)
                .ForEach(a_item => a_item.PickedBySandra = false);

            var answer = _dataContext.Answers.Single(a_item => a_item.ID == answerID);
            answer.PickedBySandra = true;

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
        public void ShowScores(long id)
        {
            // Remove all SelectLies statusses
            /*_dataContext.Questions
                .Where(a_item => a_item.Status == (int) Data.Models.Question.QuestionStatus.SelectLies)
                .ForEach(a_item => a_item.Status = 0);

            var existingQuestion = _dataContext.Questions.Single(a_item => a_item.ID == id);
            existingQuestion.Status = (int) Data.Models.Question.QuestionStatus.ShowScores;
            _dataContext.SaveChanges();*/

            var question = _dataContext.Questions
                    .Include(a_item => a_item.Subject)
                    .Single(a_item => a_item.ID == id);

            var users = _dataContext.Users.ToList();

            // Calculate scores
            // Every lie choosen by Dennis or Sandra is 250 points
            // Every lie by another persion is 100 points
            // If you guessed where Dennis or Sandra picked, 50 points
            foreach (var user in users)
            {
                long points = 0;

                var PickedByDennis = _dataContext.Answers
                    .Include(a_item => a_item.User)
                    .Include(a_item => a_item.ChosenBy)
                    .SingleOrDefault(a_item => a_item.Question.ID == id && a_item.PickedByDennis);

                var PickedBySandra = _dataContext.Answers
                    .Include(a_item => a_item.User)
                    .Include(a_item => a_item.ChosenBy)
                    .SingleOrDefault(a_item => a_item.Question.ID == id && a_item.PickedBySandra);

                // When no correct answer has been selected
                if (PickedByDennis != null && PickedByDennis.User.ID == user.ID)
                {
                    points += 250;
                }

                if (PickedBySandra != null && PickedBySandra.User.ID == user.ID)
                {
                    points += 250;
                }

                var userAnswer = _dataContext.Answers
                    .Include(a_item => a_item.ChosenBy)
                    .SingleOrDefault(a_item => a_item.Question.ID == id && a_item.User.ID == user.ID);

                if (userAnswer != null)
                {
                    points += (100 * userAnswer.ChosenBy.Count());
                }

                // 50 points
                if ((PickedByDennis != null && 
                        PickedByDennis.User.ID != user.ID && 
                        PickedByDennis.ChosenBy.Any(a_user => a_user.ID == user.ID)) ||
                    (PickedBySandra != null &&
                        PickedBySandra.User.ID != user.ID &&
                        PickedBySandra.ChosenBy.Any(a_user => a_user.ID == user.ID)))
                {
                    points += 50;
                }

                _dataContext.Scores.Add(new Data.Models.Score
                {
                    User = user,
                    Question = question,
                    Points = points
                });
                _dataContext.SaveChanges();
            }

            //return Json(true);
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

        public JsonResult ChangeQuestionStatus(long questionID, int status)
        {
            var selectedQuestion = _dataContext.Questions.Single(a_item => a_item.ID == questionID);
            selectedQuestion.Status = status;
            _dataContext.SaveChanges();

            if(status == (int)Data.Models.Question.QuestionStatus.ShowScores)
            {
                ShowScores(questionID);
            }

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
                question.Status,
                question.AnsweredByDennis,
                question.AnsweredBySandra
            };
        }

        private static object CreateAnswer(Data.Models.Answer answer)
        {
            return new
            {
                answer.ID,
                Answer = answer.AnswerString,
                UserName = answer.User.Name,
                answer.PickedByDennis,
                answer.PickedBySandra
            };
        }
    }
}