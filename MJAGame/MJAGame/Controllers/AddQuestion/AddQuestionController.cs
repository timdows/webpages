using System.Linq;
using System.Web.Mvc;
using MJAGame.Controllers.User;
using MJAGame.Data;

namespace MJAGame.Controllers.AddQuestion
{
    public class AddQuestionController : MJAGameController<MJAGameContext>
    {
        public JsonResult GetEmptyResult()
        {
            var subjects = _dataContext.Subjects.ToList();
            var newQuestion = new Data.Models.Question();

            return Json(new {subjects, newQuestion });
        }

        public JsonResult SubmitQuestion(Data.Models.Question question)
        {
            var user = UserController.GetUserObject(Request);

            var newQuestion = new Data.Models.Question
            {
                QuestionString = question.QuestionString,
                Subject = _dataContext.Subjects.Single(a_item => a_item.ID == question.Subject.ID),
                Status = 0,
                SubmittedUser = _dataContext.Users.Single(a_item => a_item.ID == user.ID)
            };

            _dataContext.Questions.Add(newQuestion);
            _dataContext.SaveChanges();

            return Json(true);
        }
    }
}