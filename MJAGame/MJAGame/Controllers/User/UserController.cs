using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MJAGame.Data;

namespace MJAGame.Controllers.User
{
    public class UserController : MJAGameController<MJAGameContext>
    {
        public const string CoockieName = "MJAGame_UserID";

        public JsonResult GetUser()
        {
            var user = new Data.Models.User();
            var coockie = Request.Cookies[UserController.CoockieName];
            if (coockie != null)
            {
                var userID = long.Parse(coockie.Value);
                user = _dataContext.Users.SingleOrDefault(a_item => a_item.ID == userID);

                if (user == null)
                {
                    coockie = new HttpCookie(UserController.CoockieName) {Expires = DateTime.Now.AddDays(-1)};
                    Response.Cookies.Add(coockie);
                    user = new Data.Models.User();
                }
            }

            return Json(user);
        }

        public static Data.Models.User GetUserObject(HttpRequestBase request)
        {
            var coockie = request.Cookies[UserController.CoockieName];
            if (coockie == null)
            {
                return null;
            }

            var userID = long.Parse(coockie.Value);
            using (var dataContext = new MJAGameContext())
            {
                return dataContext.Users.SingleOrDefault(a_item => a_item.ID == userID);
            }
        }

        public JsonResult NewUser(Data.Models.User user)
        {
            // Check if name is not used double
            if (string.IsNullOrWhiteSpace(user.Name) ||
                _dataContext.Users.Any(a_item => a_item.Name == user.Name && a_item.ID != user.ID))
            {
                throw new Exception();
            }

            if (user.IsNew)
            {
                _dataContext.Users.Add(user);
            }
            else
            {
                var existingUser = _dataContext.Users.Single(a_item => a_item.ID == user.ID);
                existingUser.Name = user.Name;
            }

            _dataContext.SaveChanges();

            var cookie = new HttpCookie(UserController.CoockieName, user.ID.ToString())
            {
                Expires = DateTime.Now.AddDays(10)
            };
            Response.AppendCookie(cookie);

            return Json(true);
        }
    }
}