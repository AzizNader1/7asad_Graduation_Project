using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LogingUsersController : ControllerBase
    {
        private readonly ILogingUserServices _LogingUserServices;

        public LogingUsersController(ILogingUserServices LogingUserServices)
        {
            _LogingUserServices = LogingUserServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllLogingUsers()
        {
            var LogingUsers = await _LogingUserServices.GetAllUsers();
            if (LogingUsers == null)
                return NotFound("there is no Loging Users avaliable");

            return Ok(LogingUsers);
        }


        [HttpGet("{id}", Name = "GetLogingUserById")]
        public async Task<IActionResult> GetLogingUserById([FromRoute] int id)
        {
            var LogingUser = await _LogingUserServices.GetUserById(id);
            if (LogingUser == null)
                return NotFound($"there is no avaliable Loging Users for this id {id}");

            return Ok(LogingUser);
        }


        [HttpGet("{LogingUserEmail}", Name = "GetLogingUserByEmail")]
        public async Task<IActionResult> GetLogingUserByEmail([FromRoute] string LogingUserEmail)
        {
            var LogingUser = await _LogingUserServices.GetUserByEmail(LogingUserEmail);
            if (LogingUser == null)
                return NotFound($"there is no avaliable Loging Users for this email :- {LogingUserEmail}");

            return Ok(LogingUser);
        }


        [HttpDelete("{id}", Name = "DeleteLogingUser")]
        public async Task<IActionResult> DeleteLogingUser([FromRoute] int id)
        {
            var LogingUser = await _LogingUserServices.GetUserById(id);
            if (LogingUser == null)
                return NotFound($"there is no avaliable Loging Users for this {id}");

            var result = _LogingUserServices.DeleteUser(LogingUser);
            return Ok(result);
        }


        [HttpPut("{id}", Name = "UpdateLogingUser")]
        public async Task<IActionResult> UpdateLogingUser([FromRoute] int id, [FromBody] LogingUserDto LogingUserDto)
        {
            var LogingUser = await _LogingUserServices.GetUserById(id);
            if (LogingUser == null)
                return NotFound($"there is no Loging Users for this id {id}");

            LogingUser.UserEmail = LogingUserDto.UserEmail;
            LogingUser.UserPassword = LogingUserDto.UserPassword;
            LogingUser.UserRole = LogingUserDto.UserRole;

            var result = _LogingUserServices.UpdateUser(LogingUser);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddLogingUser([FromBody] LogingUserDto LogingUserDto)
        {
            var LogingUser = new LogingUser
            {
                UserEmail = LogingUserDto.UserEmail,
                UserPassword = LogingUserDto.UserPassword,
                UserRole = LogingUserDto.UserRole
            };

            var result = await _LogingUserServices.AddUser(LogingUser);
            return Ok(result);
        }
    }
}
