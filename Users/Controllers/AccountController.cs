using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Users.Infrastructure;
using Users.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web;

namespace Users.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "В доступі відмовлено.." });
            }

            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel details, string returnUrl)
        {
            AppUser user = await UserManager.FindAsync(details.Name, details.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Некоректне ім'я або пароль.");
            }
            else
            {
                ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);

                //вычитать доп утверждения
                ident.AddClaims(LocationClaimsProvider.GetClaims(ident));
                //доп сконструированные утверждения
                ident.AddClaims(ClaimsRoles.CreateRolesFromClaims(ident));
                

                AuthManager.SignOut();
                AuthManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, ident);
                return Redirect(returnUrl);
            }

            return View(details);
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// GoogleLogin
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GoogleLogin(string returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleLoginCallback",
                    new { returnUrl = returnUrl })
            };
            /*Метод GoogleLogin создает экземпляр класса AuthenticationProperties и устанавливает свойство RedirectUri с адресом перенаправления, 
            который вызывает метод действия GoogleLoginCallback в этом же контроллере. 
            Следующая часть кода магическим образом вызывает ASP.NET Identity, чтобы перенаправлять пользователя на страницу аутентификации Google, а не ту, которая определена в приложении:
            */
            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Google");
            return new HttpUnauthorizedResult();
        }

        [AllowAnonymous]
        public async Task<ActionResult> GoogleLoginCallback(string returnUrl)
        {
            /* В методе действия GoogleLoginCallback мы получаем данные о пользователе от Google, используя метод GetExternalLoginInfoAsync, 
             вызываемый на реализации интерфейса IAuthenticationManager:
            Свойство	Описание
            DefaultUserName	
            Возвращает имя пользователя

            Email	
            Возвращает адрес электронной почты пользователя

            ExternalIdentity	
            Возвращает объект ClaimsIdentity, связанный с идентификатором пользователям

            Login	
            Возвращает объект UserLoginInfo, который описывает данные при внешнем входе в приложением
            */
            ExternalLoginInfo loginInfo = await AuthManager.GetExternalLoginInfoAsync();
            //метод возвращает объект AppUser, если пользователь ранее уже прошел проверку достоверности в приложении:
            AppUser user = await UserManager.FindAsync(loginInfo.Login);

            if (user == null)
            {
                user = new AppUser
                {
                    Email = loginInfo.Email,
                    UserName = loginInfo.DefaultUserName,

                    City = Cities.LONDON,
                    Country = Countries.ENG
                };

                IdentityResult result = await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return View("Error", result.Errors);
                }
                else
                {
                    //Если метод FindAsync не возвращает объект AppUser — это означает, что пользователь впервые аутентифицируется в приложении, 
                    //поэтому мы создаем новый объект AppUser с данными, полученными от Google и сохраняем нового пользователя в базе данных.
                    result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
            }
            /*
            остается для аутентификации — добавить утверждения (claims), ассоциированные с данным пользователем и предоставляемые Google, 
            а также создать аутентифицирующий файл cookie, позволяющий использовать учетные данные в приложении между сессиями:
            */
            ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                DefaultAuthenticationTypes.ApplicationCookie);

            ident.AddClaims(loginInfo.ExternalIdentity.Claims);

            AuthManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            }, ident);

            return Redirect(returnUrl ?? "/");
        }

    }
}