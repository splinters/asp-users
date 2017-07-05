using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Users.Models;

namespace Users.Infrastructure
{

 
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store)
            : base(store)
        { }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options,
            IOwinContext context)
        {
            AppIdentityDbContext db = context.Get<AppIdentityDbContext>();
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(db));

            /*
             * 
             * Свойства, определенные в классе PasswordValidator
                Название	Описание
                RequiredLength	
                Задает минимально допустимую длину пароля

                RequireNonLetterOrDigit	
                Если установлено значение true, пароль должен содержать хотя бы один символ, который не является ни буквой ни цифрой

                RequireDigit	
                Если установлено значение true, пароль должен содержать цифры

                RequireLowercase	
                Если установлено значение true, пароль должен содержать строчные символы

                RequireUppercase	
                Если установлено значение true, пароль должен содержать прописные символы
            */

            //            manager.PasswordValidator = new PasswordValidator  // стандартный и кастомный пасворд валидатор

            // при валидации через внешние источники - отключить пользовательскую проверку пароля
            /*
            manager.PasswordValidator = new CustomPasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = false
            };
            */

            /*Свойства, определенные в классе UserValidator
            Название	Описание
            AllowOnlyAlphanumericUserNames	
            Если задано true, имена пользователей могут содержать только буквы и цифры

            RequireUniqueEmail	
            Требует наличие уникального адреса электронно почты
            */

            //manager.UserValidator = new UserValidator<AppUser>(manager)
            /*
            manager.UserValidator = new CustomUserValidator(manager)  // с кастомным валидатором юзеров
             {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = false
            };
            */
            manager.UserValidator = new CustomUserValidator();

            return manager;
        }
    }
}