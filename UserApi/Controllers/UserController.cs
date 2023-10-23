using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserApi.Dtos;
using UserApi.Interfaces;
using UserApi.models;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserData _userData;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public UserController(IUserData userData, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _userData = userData ?? throw new ArgumentNullException(nameof(userData));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserModel>))]

        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userData.GetUsers());

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(users);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateUser([FromBody] UserForCreationDto userCreate)
        {
            if (userCreate == null)
            {
                return BadRequest();
            }

            var user = _userData.GetUsers().Where(c => c.UserName.Trim().ToLower() == userCreate.UserName.ToLower()).FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "Username Already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userCreate.Password = _passwordHasher.Hash(userCreate.Password);

            var userMap = _mapper.Map<UserModel>(userCreate);

            if (!_userData.AddUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while creating");
                return StatusCode(500, ModelState);
            }
            return Ok("User Successfully created");
        }

    }
}
