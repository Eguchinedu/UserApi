using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserApi.Interfaces;
using UserApi.models;
using UserApi.Repository;

namespace UserApi.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserData _userData;
        private readonly IPasswordHasher _passwordHasher;



        public class AuthenticationRequestBody
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
        }
       
        public AuthenticationController(IConfiguration configuration, IUserData userData, IPasswordHasher passwordHasher)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userData = userData ?? throw new ArgumentNullException(nameof(userData));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }


        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            if (authenticationRequestBody.UserName == null || authenticationRequestBody.Password == null)
            {
                return BadRequest("Username and password cannot be null.");
            }

            var user = ValidateUserCredentials(authenticationRequestBody.UserName, authenticationRequestBody.Password);
            

            if (user == null)
            {
                ModelState.AddModelError("", "username or password is incorrect");
                return Unauthorized(ModelState);
            }
            

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString() ));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials
                );

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
            
        }

        private UserModel? ValidateUserCredentials(string? userName, string? password)
        {
            var user = _userData.GetUsers()
       .FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

            if (user == null || !_passwordHasher.Verify(user.Password, password))
            {
                return null;
            }
            

            return user;

        }
    }
}

