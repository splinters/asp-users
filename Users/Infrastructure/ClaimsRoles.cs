using System.Collections.Generic;
using System.Security.Claims;

namespace Users.Infrastructure
{
    public class ClaimsRoles
    {
        public static IEnumerable<Claim> CreateRolesFromClaims(ClaimsIdentity user)
        {
            List<Claim> claims = new List<Claim>();
            /*
            Метод CreateRolesFromClaims() использует лямбда-выражение для определения того, имеет ли пользователь утверждение о местоположении 
            StateOrProvince поступающее из источника RemoteClaims со значением «DC» (проживает ли пользователь в штате Вашингтон) и проверяет 
            утверждение Role со значением «Employees» (является ли сотрудником компании). Если пользователь соответствует этим утверждениям, 
            то ему задается роль-утверждение DCStaff. 
            */

            if (user.HasClaim(x => x.Type == ClaimTypes.StateOrProvince
                    && x.Issuer == "RemoteClaims" && x.Value == "DC")
                    && user.HasClaim(x => x.Type == ClaimTypes.Role
                    && x.Value == "Employees"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "DCStaff"));
            }


            if (user.HasClaim(x => x.Type == ClaimTypes.Dns
               && x.Issuer == "RemoteClaims" && x.Value == "dns!"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "DC_DNS"));
            }


            return claims;
        }
    }
}