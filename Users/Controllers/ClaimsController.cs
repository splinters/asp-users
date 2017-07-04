using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Users.Infrastructure;

namespace Users.Controllers
{
    public class ClaimsController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ClaimsIdentity ident = HttpContext.User.Identity as ClaimsIdentity;

            if (ident == null)
            {
                return View("Error", new string[] { "Нет доступных требований" });
            }
            else
            {
                return View(ident.Claims);
            }
        }

        /*
        Пользователи смогут получить доступ к методу действия OtherAction() только если их утверждения соответствуют членству в роли DC_DNS. 
        Членство в этой роли генерируется динамически, поэтому изменение данных о должности пользователя или его местоположения будет автоматически отражаться на авторизации.
       */
        [Authorize(Roles = "DC_DNS")]
        public string OtherAction()
        {
            return "Это защищенный метод действия - пример ";
        }

        //доступ только при комбинации сочетаний утверждений
        [ClaimsAccess(Issuer = "RemoteClaims", ClaimType = ClaimTypes.Dns, Value = "DC_DNS")]
        public string OtherDnsAction()
        {
            return "Это защищенный метод действия - dns и комбинация утверждений";
        }
    }
}