using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EngineersController : ControllerBase
    {
        private readonly IEngineerServices _engineerServices;

        public EngineersController(IEngineerServices engineerServices)
        {
            _engineerServices = engineerServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEngineers()
        {
            var engineers = await _engineerServices.GetAllEngineers();
            if (engineers == null)
                return NotFound("there is no engineers avaliable");

            return Ok(engineers);
        }


        [HttpGet("{id}", Name = "GetEngineerById")]
        public async Task<IActionResult> GetEngineerById([FromRoute] int id)
        {
            var Engineer = await _engineerServices.GetEngineerById(id);
            if (Engineer == null)
                return NotFound($"there is no avaliable Engineer for this id {id}");

            return Ok(Engineer);
        }


        [HttpGet("{EngineerName}", Name ="GetEngineerByName")]
        public async Task<IActionResult> GetEngineerByName([FromRoute] string EngineerName)
        {
            var Engineer = await _engineerServices.GetEngineerByName(EngineerName);
            if (Engineer == null)
                return NotFound($"there is no avaliable Engineer for this name :- {EngineerName}");

            return Ok(Engineer);
        }


        [HttpDelete("{id}",Name ="DeleteEngineer")]
        public async Task<IActionResult> DeleteEngineer([FromRoute] int id)
        {
            var Engineer = await _engineerServices.GetEngineerById(id);
            if (Engineer == null)
                return NotFound($"there is no avaliable Engineer for this {id}");

           var result = _engineerServices.DeleteEngineer(Engineer);
            return Ok(result);
        }


        [HttpPut("{id}",Name ="UpdateEngineer")]
        public async Task<IActionResult> UpdateEngineer([FromRoute] int id, [FromBody] EngineerDto EngineerDto)
        {
            var Engineer = await _engineerServices.GetEngineerById(id);
            if (Engineer == null)
                return NotFound($"there is no Engineer for this id {id}");

            Engineer.EngineerName = EngineerDto.EngineerName;
            Engineer.EngineerPhone = EngineerDto.EngineerPhone;
            Engineer.EngineerEmail = EngineerDto.EngineerEmail;
            Engineer.EngineerPassword = EngineerDto.EngineerPassword;
            Engineer.EngineerAddress = EngineerDto.EngineerAddress;
           
           var result = _engineerServices.UpdateEngineer(Engineer);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddEngineer([FromBody] EngineerDto EngineerDto)
        {
            var Engineer = new Engineer()
            {
                EngineerName = EngineerDto.EngineerName,
                EngineerPhone = EngineerDto.EngineerPhone,
                EngineerEmail = EngineerDto.EngineerEmail,
                EngineerPassword = EngineerDto.EngineerPassword,
                EngineerAddress = EngineerDto.EngineerAddress
            };
           var result = _engineerServices.AddEngineer(Engineer);
            return Ok(result);
        }
    }
}

