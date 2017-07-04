using System.Collections.Generic;
using System.Security.Claims;

namespace Users.Infrastructure
{
    public class LocationClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ClaimsIdentity user)
        {
            List<Claim> claims = new List<Claim>();

            //доп данные для утверждений могут быть получены из любых внешних источников
            /*
            Получение утверждений из нескольких источников означает, что приложение не должно дублировать данные, сохраняемые в другом месте и позволяет 
            легко интегрироваться со сторонними системами.Свойство Claim.Issure всегда подскажет вам, в какой системе было создано утверждение, благодаря 
            чему можно судить о точности получаемых данных. Например, данные о местоположении сотрудников, полученные из центральной базы данных HR, вероятно, 
            будут более точными и надежными по сравнению с данными, полученными от внешнего поставщика списка рассылки.

            Используя утверждения, можно более гибко настроить авторизацию в вашем приложении, нежели чем с ролями. Проблема с ролями заключается в том, что они статичны, 
            и после того, как пользователю была назначена роль, он остается членом этой роли пока явно не будет удален из нее. 
            Утверждения могут быть использованы для авторизации пользователей непосредственно на основе той информации, что известна о них. 
            Самым простым способом реализации такой системы авторизации является генерация требований Role на основе пользовательских данных. 
            */
            if (user.Name.ToLower() == "vin")
            {
                claims.Add(CreateClaim(ClaimTypes.PostalCode, "DC 20500"));
                claims.Add(CreateClaim(ClaimTypes.StateOrProvince, "DC"));
                claims.Add(CreateClaim(ClaimTypes.Dns, "dns!"));
            }
            else
            {
                claims.Add(CreateClaim(ClaimTypes.PostalCode, "NY 10036"));
                claims.Add(CreateClaim(ClaimTypes.StateOrProvince, "NY"));
            }
            return claims;
        }

        private static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String, "RemoteClaims");
        }
    }
}