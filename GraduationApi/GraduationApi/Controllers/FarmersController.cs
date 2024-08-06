using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FarmersController : ControllerBase
    {
        private readonly IFarmerServices _farmer;
        public FarmersController(IFarmerServices farmer)
        {
            _farmer = farmer;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllFarmers()
        {
            var farmers = await _farmer.GetAllFarmers();
            if(farmers == null)
                return NotFound("there is no farmer avaliable");

            return Ok(farmers);
        }


        [HttpGet("{id}",Name ="GetFarmerById")]
        public async Task<IActionResult> GetFarmerById([FromRoute]int id)
        {
            var farmer = await _farmer.GetFarmerById(id);
            if (farmer == null)
                return NotFound($"there is no avaliable farmers for this id {id}");

            return Ok(farmer);
        }


        [HttpGet("{FarmerName}", Name ="GetFarmerByName")]
        public async Task<IActionResult> GetFarmerByName([FromRoute] string FarmerName)
        {
            var farmer = await _farmer.GetFarmerByName(FarmerName);
            if (farmer == null)
                return NotFound($"there is no avaliable farmers for this name :- {FarmerName}");

            return Ok(farmer);
        }


        [HttpDelete("{id}",Name ="DeleteFarmer")]
        public async Task<IActionResult> DeleteFarmer([FromRoute]int id)
        {
            var farmer = await _farmer.GetFarmerById(id);
            if (farmer == null)
                return NotFound($"there is no avaliable farmers for this {id}");

          var result =  _farmer.DeleteFarmer(farmer);
            return Ok(result);
        }


        [HttpPut("{id}",Name ="UpdateFarmer")]
        public async Task<IActionResult> UpdateFarmer([FromRoute] int id, [FromBody] FarmerDto farmerDto)
        {
            var farmer = await _farmer.GetFarmerById(id);
            if (farmer == null)
                return NotFound($"there is no avaliable farmers for this {id}");

            farmer.FarmerName = farmerDto.FarmerName;
            farmer.FarmerPhone = farmerDto.FarmerPhone;
            farmer.FarmerEmail = farmerDto.FarmerEmail;
            farmer.FarmerAddress = farmerDto.FarmerAddress;
            farmer.FarmerPassword = farmerDto.FarmerPassword;

           var result = _farmer.UpdateFarmer(farmer);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddFarmer([FromBody] FarmerDto farmerDto)
        {
            var farmer = new Farmer()
            {
                FarmerName = farmerDto.FarmerName,
                FarmerEmail = farmerDto.FarmerEmail,
                FarmerPassword = farmerDto.FarmerPassword,
                FarmerAddress = farmerDto.FarmerAddress,
                FarmerPhone = farmerDto.FarmerPhone
            };
                var result = await _farmer.AddFarmer(farmer);
                return Ok(result);
        }
    }
}
