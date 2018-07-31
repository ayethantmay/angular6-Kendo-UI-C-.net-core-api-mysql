using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace dotnet2_1WebAPI
{
    public class Globalfunction
    {

        public static Claim[] GetClaims(TokenData obj)
        {
            var claims = new Claim[]
            {
                new Claim("UserID",obj.UserID),
                new Claim("LoginType",obj.LoginType),
                new Claim("Userlevelid", obj.Userlevelid),
                new Claim(JwtRegisteredClaimNames.Sub, obj.Sub),
                new Claim(JwtRegisteredClaimNames.Jti, obj.Jti),
                new Claim(JwtRegisteredClaimNames.Iat, obj.Iat, ClaimValueTypes.Integer64)
            };
            return claims;
        }

        public static TokenData GetTokenData(JwtSecurityToken tokenS)
        {
            var obj = new TokenData();
            try
            {
                obj.UserID = tokenS.Claims.First(claim => claim.Type == "UserID").Value;
                obj.LoginType = tokenS.Claims.First(claim => claim.Type == "LoginType").Value;
                obj.Userlevelid = tokenS.Claims.First(claim => claim.Type == "Userlevelid").Value;
                obj.Sub = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            }
            catch (Exception ex)
            {
                WriteSystemLog(ex.Message);
            }
            return obj;
        }

        public static void WriteSystemLog(string message)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - " + message);
        }
    }
}