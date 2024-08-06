using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EngineerFarmersController : ControllerBase
    {
        private readonly IEngineerServices _EngineerServices;
        private readonly IFarmerServices _FarmerServices;
        private readonly IEngineerFarmerServices _EngineerFarmerServices;

        public EngineerFarmersController(IEngineerFarmerServices EngineerFarmerServices, IFarmerServices FarmerServices, IEngineerServices EngineerServices)
        {
            _EngineerFarmerServices = EngineerFarmerServices;
            _FarmerServices = FarmerServices;
            _EngineerServices = EngineerServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEngineerFarmers()
        {
            var EngineerFarmers = await _EngineerFarmerServices.GetAllEngineerFarmers();
            if (EngineerFarmers == null)
                return NotFound("there is no service deals avaliable");

            return Ok(EngineerFarmers);
        }


        [HttpGet("{id}",Name = "GetEngineerFarmerById")]
        public async Task<IActionResult> GetEngineerFarmerById([FromRoute] int id)
        {
            var EngineerFarmer = await _EngineerFarmerServices.GetEngineerFarmerById(id);
            if (EngineerFarmer == null)
                return NotFound($"there is no service deals avaliable for this id {id}");

            return Ok(EngineerFarmer);
        }


        [HttpPost]
        public async Task<IActionResult> AddEngineerFarmer(EngineerFarmerDto dto)
        {
            var EngineerFarmer = new EngineerFarmer
            {
                EngineerId = dto.EngnieerId,
                FarmerId = dto.FarmerId,
                ServicePrice = dto.ServicePrice,
                ServiveDate = dto.ServiveDate,
                Status = dto.Status
            };

           var result = await _EngineerFarmerServices.AddEngineerFarmer(EngineerFarmer);

            return Ok(result);
        }


        [HttpPut("{id}",Name = "UpdateEngineerFarmer")]
        public async Task<IActionResult> UpdateEngineerFarmer([FromRoute] int id, [FromBody] EngineerFarmerDto dto)
        {
            var EngineerFarmer = await _EngineerFarmerServices.GetEngineerFarmerById(id);
            if (EngineerFarmer == null)
                return NotFound($"there is no avaliable Engineer account for this id {id}");

            var isValidFarmer = await _FarmerServices.IsValidFarmer(dto.FarmerId);
            if (!isValidFarmer)
                return BadRequest($"there is no valid Farmer for this id {dto.FarmerId}");

            var isValidEngineer = await _EngineerServices.IsValidEngineer(dto.EngnieerId);
            if (!isValidEngineer)
                return BadRequest($"there is no valid Engineer for this id {dto.EngnieerId}");

            EngineerFarmer.ServiveDate = dto.ServiveDate;
            EngineerFarmer.ServicePrice = dto.ServicePrice;
            EngineerFarmer.Status = dto.Status;
            EngineerFarmer.EngineerId = dto.EngnieerId;
            EngineerFarmer.FarmerId = dto.FarmerId;
            

           var result = _EngineerFarmerServices.UpdateEngineerFarmer(EngineerFarmer);
            return Ok(result);
        }


        [HttpDelete("{id}",Name = "DeleteEngineerFarmer")]
        public async Task<IActionResult> DeleteEngineerFarmer([FromRoute] int id)
        {
            var EngineerFarmer = await _EngineerFarmerServices.GetEngineerFarmerById(id);
            if (EngineerFarmer == null)
                return NotFound($"there is no avaliable service deal for this id {id}");

           var result = _EngineerFarmerServices.DeleteEngineerFarmer(EngineerFarmer);
            return Ok(result);
        }


        [HttpGet("{FarmerId}", Name = "GetEngineerFarmersByFarmerId")]
        public async Task<IActionResult> GetEngineerFarmersByFarmerId([FromRoute] int FarmerId)
        {
            var records = await _EngineerFarmerServices.GetEngineerFarmerByFarmerId(FarmerId);
            if (records == null)
                return NotFound($"there was no service deals with this Farmer id {FarmerId}");

            var EngineerFarmers = new List<EngineerFarmerDetailsDto>();
            foreach (var record in records)
            {
                var engineers = await _EngineerServices.GetEngineerById(record.EngineerId);
                var order = new EngineerFarmerDetailsDto()
                {
                    Id = record.EngineerFarmerId,
                    FarmerId = record.FarmerId,
                    EngnieerId = record.EngineerId,
                    ServicePrice = record.ServicePrice,
                    EngineerName = engineers.EngineerName,
                    FarmerName = record.Farmer.FarmerName,
                   ServiveDate = record.ServiveDate,
                   Status = record.Status,
                   EngineerPhone = engineers.EngineerPhone
                };
                EngineerFarmers.Add(order);
            }
            return Ok(EngineerFarmers);
        }


        [HttpGet("{EngineerId}", Name = "GetEngineerFarmersByEngineerId")]
        public async Task<IActionResult> GetEngineerFarmersByEngineerId([FromRoute] int EngineerId)
        {
            var records = await _EngineerFarmerServices.GetEngineerFarmerByEngineerId(EngineerId);
            if (records == null)
                return NotFound($"there was no service deals with this Engineer id {EngineerId}");

            var EngineerFarmers = new List<EngineerFarmerDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _FarmerServices.GetFarmerById(record.FarmerId);
                var order = new EngineerFarmerDetailsDto()
                {
                    Id = record.EngineerFarmerId,
                    FarmerId = record.FarmerId,
                    EngnieerId = record.EngineerId,
                    ServicePrice = record.ServicePrice,
                    EngineerName = record.Engineer.EngineerName,
                    FarmerName = farmers.FarmerName,
                    ServiveDate =record.ServiveDate,
                    Status = record.Status,
                    EngineerPhone = record.Engineer.EngineerPhone
                };
                EngineerFarmers.Add(order);
            }
            return Ok(EngineerFarmers);
        }


        [HttpGet("{EngineerName}", Name = "GetEngineerFarmersByEngineerName")]
        public async Task<IActionResult> GetEngineerFarmersByEngineerName([FromRoute] string EngineerName)
        {
            var records = await _EngineerFarmerServices.GetEngineerFarmerByEngineerName(EngineerName);
            if (records == null)
                return NotFound($"there was no service deals with this Engineer name {EngineerName}");

            var EngineerFarmers = new List<EngineerFarmerDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _FarmerServices.GetFarmerById(record.FarmerId);
                var order = new EngineerFarmerDetailsDto()
                {
                    Id = record.EngineerFarmerId,
                    FarmerId = record.FarmerId,
                    EngnieerId = record.EngineerId,
                    ServicePrice = record.ServicePrice,
                    EngineerName = record.Engineer.EngineerName,
                    FarmerName = farmers.FarmerName,
                    ServiveDate=record.ServiveDate,
                    Status = record.Status,
                    EngineerPhone = record.Engineer.EngineerPhone

                };
                EngineerFarmers.Add(order);
            }
            return Ok(EngineerFarmers);
        }


        [HttpGet("{FarmerName}", Name = "GetEngineerFarmersByFarmerName")]
        public async Task<IActionResult> GetEngineerFarmersByFarmerName([FromRoute] string FarmerName)
        {
            var records = await _EngineerFarmerServices.GetEngineerFarmerByFarmerName(FarmerName);
            if (records == null)
                return NotFound($"there was no service deals with this Farmer name {FarmerName}");

            var EngineerFarmers = new List<EngineerFarmerDetailsDto>();
            foreach (var record in records)
            {
                var engineers = await _EngineerServices.GetEngineerById(record.EngineerId);
                var order = new EngineerFarmerDetailsDto()
                {
                    Id = record.EngineerFarmerId,
                    FarmerId = record.FarmerId,
                    EngnieerId = record.EngineerId,
                    ServicePrice = record.ServicePrice,
                    EngineerName = engineers.EngineerName,
                    FarmerName = record.Farmer.FarmerName,
                    ServiveDate = record.ServiveDate,
                    Status = record.Status,
                    EngineerPhone = engineers.EngineerPhone
                };
                EngineerFarmers.Add(order);
            }
            return Ok(EngineerFarmers);
        }
    }
}
