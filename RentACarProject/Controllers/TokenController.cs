using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentACarProject.Data;
using RentACarProject.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentACarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public  IConfiguration _config;
        public readonly DataContext _context;

        public TokenController(IConfiguration config, DataContext context) 
        {
            _config = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserInfo userInfo)
        {
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.Username) && !string.IsNullOrEmpty(userInfo.Password))
            {
                var user = await GetUser(userInfo.Username, userInfo.Password);
                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Username", user.Username),
                        new Claim("Password", user.Password)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                        (_config.GetSection("Jwt:Key").Value!));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                    var token = new JwtSecurityToken(
                        issuer: _config["Jwt:Issuer"],
                        audience: _config["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: signIn);

                    var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(jwt);
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
                
                    
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<UserInfo> GetUser(string username, string password)
        {
            return await _context.UserInfos.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

    }
}
