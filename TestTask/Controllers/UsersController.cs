using Microsoft.AspNetCore.Mvc;
using TestTask.Services.Meetings;
using TestTask.Services.Users;
using TestTask.Models;
using TestTask.DTOs.Meetings;
using TestTask.DTOs.Users;


namespace TestTask.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public ActionResult<UserResponse> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = _userService.CreateUser(request.Name);

            var response = new UserResponse
            {
                Id = user.Id,
                Name = user.Name
            };

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, response);
        }


        [HttpGet("{id}")]
        public ActionResult<UserResponse> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name
            };
        }
    }
}
