using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Users.Infrastructure;
using Users.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Web;

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

        [Authorize]
        public ActionResult UserProps()
        {
            return View(CurrentUser);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UserProps(Cities city)
        {
            AppUser user = CurrentUser;
            user.City = city;

            user.SetCountryFromCity(city);

            await UserManager.UpdateAsync(user);
            return View(user);
        }

        /*
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UserProps(Status status)
        {
            AppUser user = CurrentUser;
            user.Status = status;
            await UserManager.UpdateAsync(user);
            return View(user);
        }
        */

        private AppUser CurrentUser
        {
            get
            {
                return UserManager.FindByName(HttpContext.User.Identity.Name);
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        // Вспомогательный метод, загружающий название элемента перечисления
        // из атрибута Display
        [NonAction]
        public static string GetCityName<TEnum>(TEnum item)
            where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("Тип TEnum должен быть перечислением");
            }
            else
                return item.GetType()
                    .GetMember(item.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()
                    .Name;
        }
        [NonAction]
        public static string GetStatusName<TEnum>(TEnum item)
                    where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("Тип TEnum должен быть перечислением");
            }
            else
                return item.GetType()
                    .GetMember(item.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()
                    .Name;
        }

    }
}