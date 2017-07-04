using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Users.Controllers
{
    public class HomeController : Controller
    {

        /*
                [Authorize]
                public ActionResult Index()
                {
                    Dictionary<string, object> data
                        = new Dictionary<string, object>();
                    data.Add("Ключ", "Значение");

                    return View(data);
                }
        */
        [Authorize]
        public ActionResult Index()
        {
            return View(GetData("Index"));
        }

        [Authorize(Roles = "Users")]
        public ActionResult OtherAction()
        {
            return View("Index", GetData("OtherAction"));
        }

        private Dictionary<string, object> GetData(string actionName)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("Action", actionName);
            dict.Add("Користувач", HttpContext.User.Identity.Name);
            dict.Add("Аутентифікований?", HttpContext.User.Identity.IsAuthenticated);
            dict.Add("Тип аутентификації", HttpContext.User.Identity.AuthenticationType);
            dict.Add("В ролі Users?", HttpContext.User.IsInRole("Users"));
            dict.Add("В роли Admin?", HttpContext.User.IsInRole("Admin"));

            return dict;
        }

    }
}