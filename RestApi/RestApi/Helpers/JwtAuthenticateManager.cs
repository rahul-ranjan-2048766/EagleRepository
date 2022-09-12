using RestApi.Models;
using RestApi.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace RestApi.Helpers
{
    public class JwtAuthenticateManager : IJwtAuthenticateManager
    {
        private readonly IUserService service;
        private readonly IConfiguration configuration;
        public JwtAuthenticateManager(IUserService userService, IConfiguration configuration)
        {
            service = userService;
            this.configuration = configuration;
        }

        public Response? Authenticate(UserCred userCred)
        {
            var user = service.GetUsers().FirstOrDefault(x => x.LoginId.Equals(userCred.LoginId));
            if (user == null) return null;

            if (!user.Password.Equals(userCred.Password)) return null;

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                { 
                    new Claim(ClaimTypes.Name, userCred.LoginId),
                    new Claim(ClaimTypes.Role, "USER")
                }),
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["SecretKey"])), SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return new Response 
            { 
                LoginId = user.LoginId,
                Token = jwtSecurityTokenHandler.WriteToken(token),
                ValidFrom = token.ValidFrom,
                ValidTo = token.ValidTo
            };
        }
    }
}
