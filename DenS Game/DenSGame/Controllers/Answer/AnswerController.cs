using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using DenSGame.Controllers.User;
using DenSGame.Data;

namespace DenSGame.Controllers.Answer
{
    public class AnswerController : DenSGameController<DenSGameContext>
    {
        public JsonResult SubmitAnswer(long questionID, string answerString)
        {
            if (string.IsNullOrWhiteSpace(answerString))
            {
                return Json(true);
            }

            var question = _dataContext.Questions.Single(a_item => a_item.ID == questionID);
            var user = UserController.GetUserObject(Request);

            // Check if this is not a double post
            if(_dataContext.Answers.Any(a_item => a_item.User.ID == user.ID && a_item.Question.ID == questionID))
            {
                return Json(true);
            }

            _dataContext.Answers.Add(new Data.Models.Answer
            {
                User = _dataContext.Users.Single(a_item => a_item.ID == user.ID),
                Question = question,
                AnswerString = answerString
            });

            _dataContext.SaveChanges();

            return Json(true);
        }

        // The user has selected an lie from the list
        public JsonResult SelectAnswer(long answerID)
        {
            var answer = _dataContext.Answers
                .Include(a_item => a_item.ChosenBy)
                .Single(a_item => a_item.ID == answerID);
            var user = UserController.GetUserObject(Request);

            // Check if not yet made a selection
            if (answer.ChosenBy.SingleOrDefault(a_item => a_item.ID == user.ID) == null)
            {
                answer.ChosenBy.Add(_dataContext.Users.Single(a_item => a_item.ID == user.ID));
                _dataContext.SaveChanges();
            }

            return Json(true);
        }
        
    }
}