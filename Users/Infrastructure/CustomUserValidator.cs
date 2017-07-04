using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Users.Models;

namespace Users.Infrastructure
{
    public class CustomUserValidator : UserValidator<AppUser>
    {
        public CustomUserValidator(AppUserManager manager)
            : base(manager)
        { }

        //public override async Task<IdentityResult> ValidateAsync(AppUser item)
        public async Task<IdentityResult> ValidateAsync(AppUser item)
        {
            List<string> errors = new List<string>();

            if (String.IsNullOrEmpty(item.UserName.Trim()))
                errors.Add("Вы указали пустое имя.");

            string userNamePattern = @"^[a-zA-Z0-9а-яА-Я]+$";

            if (!Regex.IsMatch(item.UserName, userNamePattern))
                errors.Add("В имени разрешается указывать буквы английского или русского языков, и цифры");

//            IdentityResult result = await base.ValidateAsync(item);

//            if (!user.Email.ToLower().EndsWith("@mail.ru"))
            if (item.Email.ToLower().EndsWith("@mail.ru"))
                {
                    //var errors = result.Errors.ToList();
                errors.Add("Любой email-адрес от mail.ru запрещен");
//                result = new IdentityResult(errors);
            }

            if (errors.Count > 0)
                return IdentityResult.Failed(errors.ToArray());

            return IdentityResult.Success;
//            return result;
        }
    }
}