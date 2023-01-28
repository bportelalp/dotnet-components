using BP.Components.JwtAuth;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BP.Samples.JwtConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var tokenGen = new JwtGenerator("feeafaghgrjrynrlig9p8gbiobn");

            var token = tokenGen.Generate();
            Console.WriteLine("JWT Token: " + token);



            JwtSecurityTokenHandler tokenHandler = new();
            bool canReadToken = tokenHandler.CanReadToken(token);
            if (canReadToken)
            {
                JwtSecurityToken tknReaded = tokenHandler.ReadJwtToken(token);
                var claims = tknReaded.Claims;
                
            }


        }

    }
}