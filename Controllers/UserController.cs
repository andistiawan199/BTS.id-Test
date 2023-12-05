using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web_API.dto;
using Web_API.Models;

namespace Web_API.Controllers
{
   
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DBContext _dbContext;
        public UserController(IConfiguration configuration, DBContext dBContext) { 
            _configuration = configuration;
            _dbContext = dBContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Save([FromBody] CreateUser createUser) 
        {
            //try
            //{
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(createUser.password, SaltRevision.Revision2A); ;
                UserModel model = new()
                {
                    Username = createUser.username,
                    Email = createUser.email,
                    Password = passwordHash
                };
                await _dbContext.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                ResponseDto<CreateUser> response = new()
                {
                    statusCode = 201,
                    data = createUser,
                    message = "User created"
                };
                return new ObjectResult(response) { StatusCode = 201 };
            //}
            //catch(Exception ex)
            //{
            //    ResponseDto<CreateUser> response = new()
            //    {
            //        statusCode = 500,
            //        data = null,
            //        message = ex.Message.ToString()
            //    };
            //    return new ObjectResult(response) { StatusCode = 500 };
            //}
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIn([FromBody] SignIn signIn)
        {
            try
            {
                ResponseDto<TokenResult> response = new ResponseDto<TokenResult>();
                response.statusCode = 401;
                response.message = "Incorrect username or password";
                response.data = null;
                UserModel user = _dbContext.User.First(user => user.Username.Equals(signIn.username));
                if (user == null)
                {
                    return new ObjectResult(response) { StatusCode = 401 };
                }

                if (!BCrypt.Net.BCrypt.Verify(signIn.password, user.Password))
                {
                    return new ObjectResult(response) { StatusCode = 401 };
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                    };

                var Sectoken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                      _configuration["Jwt:Issuer"],
                      claims,
                      expires: DateTime.Now.AddMinutes(120),
                      signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
                TokenResult tokenResult = new()
                {
                    token = token
                };

                response.statusCode = 200;
                response.message = "Hello, welcome back";
                response.data = tokenResult;
                return new ObjectResult(response) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                ResponseDto<TokenResult> response = new()
                {
                    statusCode = 500,
                    data = null,
                    message = ex.Message.ToString()
                };
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }
    }
}
