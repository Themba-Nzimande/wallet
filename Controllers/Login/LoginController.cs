namespace WebApplication1.Controllers.Login
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using WebApplication1.Controllers.Login.DTOs;
    using WebApplication1.helpers;
    using WebApplication1.Repos.Accounts;
    using WebApplication1.Repos.Users;

    /// <summary>
    /// API controller for login related enpoints such as account create and login.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [RequireHttps]
    public class LoginController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        private readonly IUserRepo _userRepo;

        private readonly IAccountsRepo _accountsRepo;

        private EncryptionHelper encryptionHelper;


        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        /// <param name="configuration">appsettings config.</param>
        /// <param name="userRepo">User DB repo singleton.</param>
        /// <param name="accountsRepo">Accounts DB repo singleton.</param>
        public LoginController(IConfiguration configuration, IUserRepo userRepo, IAccountsRepo accountsRepo)
        {
            this._configuration = configuration;
            this._userRepo = userRepo;
            this._accountsRepo = accountsRepo;
            this.encryptionHelper = new EncryptionHelper(this._configuration);
        }


        /// <summary>
        /// Login API endpoint.
        /// </summary>
        /// <param name="email">user email.</param>
        /// <param name="password">user password.</param>
        /// <returns>jwt token string.</returns>
        [HttpPost("Login")]
        public IActionResult Login(string email, string password)
        {
            var userExistsCheck = this._userRepo.GetUsers(email).FirstOrDefault();
            if (userExistsCheck != null && this.encryptionHelper.DecryptString(userExistsCheck.User_password) == password)
            {
                var issuer = this._configuration.GetValue<string>("Jwt:Issuer");
                var audience = this._configuration.GetValue<string>("Jwt:Audience");
                var key = Encoding.ASCII.GetBytes(this._configuration.GetValue<string>("Jwt:Key"));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim("UserID", userExistsCheck.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature),
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);

                return this.Ok(stringToken);
            }
            else
            {
                return this.Unauthorized("Invalid login");
            }
        }

        /// <summary>
        /// API  endpoint for new account creation. Token is returned for succesful creation to skip login.
        /// </summary>
        /// <param name="newUserDTO">new user object.</param>
        /// <returns>jwt token string.</returns>
        [HttpPost("CreateAccount")]
        public IActionResult CreateAccount(NewUserDTO newUserDTO)
        {
            var newuser = new Models.User()
            {
                Email = newUserDTO.Email,
                User_password = this.encryptionHelper.EncryptString(newUserDTO.Password),
            };

            var newAccount = new Models.Account()
            {
                Current_balance = 0,
                Account_number = 0,
            };
            var createNewUser = this._userRepo.CreateUser(newuser);

            var createNewUserAccount = this._accountsRepo.CreateNewAccount(newAccount);

            var createNewUserAccountMpping = this._accountsRepo.CreateNewUserAccountMapping(new Models.UserAccountMapping()
            {
                Account_Id = newAccount.Id,
                User_Id = newuser.Id,
            });

            var issuer = this._configuration.GetValue<string>("Jwt:Issuer");
            var audience = this._configuration.GetValue<string>("Jwt:Audience");
            var key = Encoding.ASCII.GetBytes(this._configuration.GetValue<string>("Jwt:Key"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim("UserID", newuser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return this.Ok(stringToken);
        }
    }
}
