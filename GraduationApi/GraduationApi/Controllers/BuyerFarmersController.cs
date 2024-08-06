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
    public class BuyerFarmersController : ControllerBase
    {
        private readonly IBuyerFarmerServices _farmer;
        public BuyerFarmersController(IBuyerFarmerServices farmer)
        {
            _farmer = farmer;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllBuyerFarmers()
        {
            var farmers = await _farmer.GetAllBuyerFarmers();
            if(farmers == null)
                return NotFound("there is no farmer avaliable");

            return Ok(farmers);
        }


        [HttpGet("{id}",Name ="GetBuyerFarmerById")]
        public async Task<IActionResult> GetBuyerFarmerById([FromRoute]int id)
        {
            var farmer = await _farmer.GetBuyerFarmerById(id);
            if (farmer == null)
                return NotFound($"there is no avaliable farmers for this id {id}");

            return Ok(farmer);
        }


        [HttpGet("{FarmerName}", Name ="GetBuyerFarmerByName")]
        public async Task<IActionResult> GetBuyerFarmerByName([FromRoute] string FarmerName)
        {
            var farmer = await _farmer.GetBuyerFarmerByName(FarmerName);
            if (farmer == null)
                return NotFound($"there is no avaliable farmers for this name :- {FarmerName}");

            return Ok(farmer);
        }


        [HttpDelete("{id}",Name ="DeleteBuyerFarmer")]
        public async Task<IActionResult> DeleteBuyerFarmer([FromRoute]int id)
        {
            var farmer = await _farmer.GetBuyerFarmerById(id);
            if (farmer == null)
                return NotFound($"there is no avaliable farmers for this {id}");

          var result =  _farmer.DeleteBuyerFarmer(farmer);
            return Ok(result);
        }


        [HttpPut("{id}",Name ="UpdateBuyerFarmer")]
        public async Task<IActionResult> UpdateBuyerFarmer([FromRoute] int id, [FromBody] BuyerFarmerDto farmerDto)
        {
            var farmer = await _farmer.GetBuyerFarmerById(id);
            if (farmer == null)
                return NotFound($"there is no avaliable farmers for this {id}");

            farmer.FarmerName = farmerDto.FarmerName;
            farmer.FarmerPhone = farmerDto.FarmerPhone;
            farmer.FarmerEmail = farmerDto.FarmerEmail;
            farmer.FarmerAddress = farmerDto.FarmerAddress;
            farmer.FarmerPassword = farmerDto.FarmerPassword;

           var result = _farmer.UpdateBuyerFarmer(farmer);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddBuyerFarmer([FromBody] BuyerFarmerDto farmerDto)
        {
            var farmer = new BuyerFarmer()
            {
                FarmerName = farmerDto.FarmerName,
                FarmerEmail = farmerDto.FarmerEmail,
                FarmerPassword = farmerDto.FarmerPassword,
                FarmerAddress = farmerDto.FarmerAddress,
                FarmerPhone = farmerDto.FarmerPhone
            };
                var result = await _farmer.AddBuyerFarmer(farmer);
                return Ok(result);
        }
    }
}
