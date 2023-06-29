namespace WebApplication1.Controllers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using WebApplication1.Repos.Users;

    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly IUserRepo _userRepo;

        public AuthController(IConfiguration configuration, IUserRepo userRepo)
        {
            _configuration = configuration;
            _userRepo = userRepo;
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            //if (user.UserName == "joydip" && user.Password == "joydip123")
            //{
                var issuer = _configuration.GetValue<string>("Jwt:Issuer");
                var audience = _configuration.GetValue<string>("Jwt:Audience");
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>(("Jwt:Key")));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim("UserID", "test name"),
                new Claim("Role", "test role"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);
                return this.Ok(stringToken);
           // }
            //return Results.Unauthorized();
        }






    }
}
