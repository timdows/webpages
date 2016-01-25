using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using MJAGame.Controllers.User;
using MJAGame.Data;
using WebGrease.Css.Extensions;

namespace MJAGame.Controllers.Answer
{
    public class AnswerController : MJAGameController<MJAGameContext>
    {
        public JsonResult SubmitAnswer(long questionID, string answerString)
        {
            if (string.IsNullOrWhiteSpace(answerString))
            {
                return Json(true);
            }

            var question = _dataContext.Questions.Single(a_item => a_item.ID == questionID);
            var user = UserController.GetUserObject(Request);

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