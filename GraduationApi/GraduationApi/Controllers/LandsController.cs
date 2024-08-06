using GraduationApi.Interfaces;
using GraduationApi.Models;
using GraduationApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LandsController : ControllerBase
    {
        private readonly ILandServices _LandServices;
        private readonly IFarmerServices _farmerServices;

        public LandsController(ILandServices LandServices,IFarmerServices farmerServices)
        {
            _LandServices = LandServices;
            _farmerServices = farmerServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllLands()
        {
            var Lands = await _LandServices.GetAllLands();
            if (Lands == null)
                return NotFound("there is no Lands avaliable");

            return Ok(Lands);
        }


        [HttpGet("{id}",Name = "GetLandById")]
        public async Task<IActionResult> GetLandById([FromRoute] int id)
        {
            var Land = await _LandServices.GetLandById(id);
            if (Land == null)
                return NotFound($"there is no avaliable Lands for this id {id}");

            return Ok(Land);
        }


        [HttpDelete("{id}",Name = "DeleteLand")]
        public async Task<IActionResult> DeleteLand([FromRoute] int id)
        {
            var Land = await _LandServices.GetLandById(id);
            if (Land == null)
                return NotFound($"there is no avaliable Lands for this {id}");

          var result =  _LandServices.DeleteLand(Land);
            return Ok(result);
        }


        [HttpPut("{id}",Name = "UpdateLand")]
        public async Task<IActionResult> UpdateLand([FromRoute] int id, [FromBody] LandDto LandDto)
        {
            var Land = await _LandServices.GetLandById(id);
            if (Land == null)
                return NotFound($"there is no Lands for this id {id}");

            var isValidFarmer = await _farmerServices.IsValidFarmer(LandDto.FarmerId);
            if (!isValidFarmer)
                return NotFound($"there is no valid farmers for this id {LandDto.FarmerId}");

            Land.LandLocation = LandDto.LandLocation;
            Land.LandSize = LandDto.LandSize;
            Land.LandType = LandDto.LandType;
            Land.FarmerId = LandDto.FarmerId;
            Land.LandDescribtion = LandDto.LandDescribtion;
            
           var result = _LandServices.UpdateLand(Land);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddLand([FromBody] LandDto LandDto)
        {

            var land = new Land()
            {
                LandLocation = LandDto.LandLocation,
                LandSize = LandDto.LandSize,
                LandType = LandDto.LandType,
                FarmerId = LandDto.FarmerId,
                LandDescribtion = LandDto.LandDescribtion
            };

           var result = _LandServices.AddLand(land);
            return Ok(result);
        }


        [HttpGet("{FarmerId}", Name ="GetLandsByFarmerId")]
        public async Task<IActionResult> GetLandsByFarmerId([FromRoute]int FarmerId)
        {
            var records = await _LandServices.GetLandsByFarmerId(FarmerId);
            if (records == null)
                return NotFound($"there was no Lands for this farmer id {FarmerId}");

            var farmerLands = new List<LandDetailsDto>();
            foreach (var record in records)
            {

                var order = new LandDetailsDto()
                {
                Id = record.LandId,
                FarmerId = record.FarmerId,
                FarmerName = record.Farmer.FarmerName,
                LandLocation = record.LandLocation,
                LandSize = record.LandSize,
                LandType = record.LandType,
                LandDescribtion = record.LandDescribtion,
            };
            farmerLands.Add(order);
            }
            return Ok(farmerLands);
        }


        [HttpGet("{FarmerName}", Name ="GetLandsByFarmerName")]
        public async Task<IActionResult> GetLandsByFarmerName([FromRoute] string FarmerName)
        {
            var records = await _LandServices.GetLandsByFarmerName(FarmerName);
            if (records == null)
                return NotFound($"there was no Lands for this farmer name {FarmerName}");

            var farmerLands = new List<LandDetailsDto>();
            foreach (var record in records)
            {
                var order = new LandDetailsDto()
                {
                    Id = record.LandId,
                    FarmerId = record.FarmerId,
                    FarmerName = record.Farmer.FarmerName,
                    LandLocation = record.LandLocation,
                    LandSize = record.LandSize,
                    LandType = record.LandType,
                    LandDescribtion = record.LandDescribtion,
                };
                farmerLands.Add(order);
            }
            return Ok(farmerLands);
        }

    }
}
