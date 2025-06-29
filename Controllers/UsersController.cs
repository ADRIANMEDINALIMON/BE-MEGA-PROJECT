using BE_MEGA_PROJECT.DTOs;
using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BE_MEGA_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        public UsersController(IUserService userService) { 
            _userService = userService;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await  _userService.GetUsers();
            return Ok(users);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDTO userDTO)
        {
            
            var user = new User {
                
                Username = userDTO.Username,
                PasswordHash = userDTO.PasswordHash,
                FullName = userDTO.FullName,
                CreatedAt = DateTime.Now,
                Active = true
            };
            var createdUser = await _userService.CreateUser(user);
            return Ok(createdUser);

        }

        [HttpGet("{username},{password}")]
        public async Task<IActionResult> Authenticate(string username, string password)
        {
            var user = await _userService.Authenticate(username, password);
            if (user == null)
                return NotFound(new { message = "usuario o contraseña incorrectos" });
            return Ok(user);
        }


    }
}
