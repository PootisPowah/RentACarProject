//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using RentACarProject.Dto;
//using RentACarProject.Models;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace RentACarProject.Controllers
//{
//    /// <summary>
//    /// Authentication controller.
//    /// </summary>
//    [Route("api/[controller]")]
//    [ApiController]
//    [Produces("application/json")]
//    public class AuthenticationController : ControllerBase
//    {
//        public static User user = new User();
//        private readonly IConfiguration _config;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
//        /// </summary>
//        /// <param name="config">Authentication config</param>
//        public AuthenticationController(IConfiguration config)
//        {
//            _config = config;
//        }
//        /// <summary>
//        /// Registers a new user.
//        /// </summary>
//        /// <param name="request">The user registration request containing username and password.</param>
//        /// <returns>The created user.</returns>
//        /// <response code="200">User created successfully.</response>
//        [HttpPost("register")]
//        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
//        public ActionResult <User> Register(UserDto request)
//        {
//            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

//            user.Username = request.Username;
//            user.PasswordHash = passwordHash;
//            return Ok(user);
//        }

//        /// <summary>
//        /// Logs in a user.
//        /// </summary>
//        /// <param name="request">The user login request containing username and password.</param>
//        /// <returns>A JWT token if login is successful.</returns>
//        /// <response code="200">Returns the JWT token.</response>
//        /// <response code="400">If the user is not found or the password is incorrect.</response>
//        [HttpPost("login")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public ActionResult<User> Login(UserDto request)
//        {
//            if(user.Username != request.Username)
//            {
//                return BadRequest("User not found");
//            }

//            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
//            {
//                return BadRequest("Wrong password");
//            }
//            string token = CreateToken(user);
//            return Ok(token);
//        }
//        /// <summary>
//        /// Creates a JWT token for the user.
//        /// </summary>
//        /// <param name="user">The user for whom the token is created.</param>
//        /// <returns>A JWT token.</returns>
//        private string CreateToken(User user)
//        {
//            List<Claim> claims = new List<Claim> {
//                new Claim(ClaimTypes.Name, user.Username)
//            };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
//                _config.GetSection("AppSettings:Token").Value!));

//            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

//            var token = new JwtSecurityToken(
//                claims:claims,
//                expires:DateTime.Now.AddDays(1),
//                signingCredentials:credentials
//                );

//            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

//            return jwt;
//        }
//    }
//}
