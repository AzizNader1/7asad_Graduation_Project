using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EngineerCompaniesController : ControllerBase
    {
        private readonly IEngineerServices _EngineerServices;
        private readonly ICompanyServices _CompanyServices;
        private readonly IEngineerCompanyServices _EngineerCompanyServices;

        public EngineerCompaniesController(IEngineerCompanyServices EngineerCompanyServices, ICompanyServices CompanyServices, IEngineerServices EngineerServices)
        {
            _EngineerCompanyServices = EngineerCompanyServices;
            _CompanyServices = CompanyServices;
            _EngineerServices = EngineerServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEngineerCompanies()
        {
            var EngineerCompanys = await _EngineerCompanyServices.GetAllEngineerCompanies();
            if (EngineerCompanys == null)
                return NotFound("there is no service deals avaliable");

            return Ok(EngineerCompanys);
        }


        [HttpGet("{id}",Name = "GetEngineerCompanyById")]
        public async Task<IActionResult> GetEngineerCompanyById([FromRoute] int id)
        {
            var EngineerCompany = await _EngineerCompanyServices.GetEngineerCompanyById(id);
            if (EngineerCompany == null)
                return NotFound($"there is no service deals avaliable for this id {id}");

            return Ok(EngineerCompany);
        }


        [HttpPost]
        public async Task<IActionResult> AddEngineerCompany(EngineerCompanyDto dto)
        {
            var EngineerCompany = new EngineerCompany
            {
                EngineerId = dto.EngineerId,
                CompanyId = dto.CompanyId,
                ServicePrice = dto.ServicePrice,
                ServiveDate = dto.ServiveDate,
                Status = dto.Status,
            };

            var result = await _EngineerCompanyServices.AddEngineerCompany(EngineerCompany);

            return Ok(result);
        }


        [HttpPut("{id}",Name = "UpdateEngineerCompany")]
        public async Task<IActionResult> UpdateEngineerCompany([FromRoute] int id, [FromBody] EngineerCompanyDto dto)
        {
            var EngineerCompany = await _EngineerCompanyServices.GetEngineerCompanyById(id);
            if (EngineerCompany == null)
                return NotFound($"there is no avaliable Engineer account for this id {id}");

            var isValidCompany = await _CompanyServices.IsValidCompany(dto.CompanyId);
            if (!isValidCompany)
                return BadRequest($"there is no valid Company for this id {dto.CompanyId}");

            var isValidEngineer = await _EngineerServices.IsValidEngineer(dto.EngineerId);
            if (!isValidEngineer)
                return BadRequest($"there is no valid Engineer for this id {dto.EngineerId}");

            EngineerCompany.ServiveDate = dto.ServiveDate;
            EngineerCompany.ServicePrice = dto.ServicePrice;
            EngineerCompany.CompanyId = dto.CompanyId;
            EngineerCompany.EngineerId = dto.EngineerId;
            EngineerCompany.Status = dto.Status;

            var result = _EngineerCompanyServices.UpdateEngineerCompany(EngineerCompany);
            return Ok(result);
        }



        [HttpDelete("{id}",Name = "DeleteEngineerCompany")]
        public async Task<IActionResult> DeleteEngineerCompany([FromRoute] int id)
        {
            var EngineerCompany = await _EngineerCompanyServices.GetEngineerCompanyById(id);
            if (EngineerCompany == null)
                return NotFound($"there is no avaliable service deal for this id {id}");

           var result = _EngineerCompanyServices.DeleteEngineerCompany(EngineerCompany);
            return Ok(result);
        }


        [HttpGet("{CompanyId}", Name = "GetEngineerCompanysByCompanyId")]
        public async Task<IActionResult> GetEngineerCompanysByCompanyId([FromRoute] int CompanyId)
        {
            var records = await _EngineerCompanyServices.GetEngineerCompanyByCompanyId(CompanyId);
            if (records == null)
                return NotFound($"there was no service deals with this Company id {CompanyId}");

            var EngineerCompanys = new List<EngineerCompanyDetailsDto>();
            foreach (var record in records)
            {
                var engineers = await _EngineerServices.GetEngineerById(record.EngineerId);
                var order = new EngineerCompanyDetailsDto()
                {
                    Id = record.EngineerCompanyId,
                    CompanyId = record.CompanyId,
                    EngineerId = record.EngineerId,
                    ServicePrice = record.ServicePrice,
                    EngineerName = engineers.EngineerName,
                    CompanyName = record.Company.CompanyName,
                    ServiveDate = record.ServiveDate,
                    Status = record.Status,
                    EngineerPhone = engineers.EngineerPhone
                };
                EngineerCompanys.Add(order);
            }
            return Ok(EngineerCompanys);
        }


        [HttpGet("{EngineerId}", Name = "GetEngineerCompanysByEngineerId")]
        public async Task<IActionResult> GetEngineerCompanysByEngineerId([FromRoute] int EngineerId)
        {
            var records = await _EngineerCompanyServices.GetEngineerCompanyByEngineerId(EngineerId);
            if (records == null)
                return NotFound($"there was no service deals with this Engineer id {EngineerId}");

            var EngineerCompanys = new List<EngineerCompanyDetailsDto>();
            foreach (var record in records)
            {
                var companies = await _CompanyServices.GetCompanyById(record.CompanyId);
                var order = new EngineerCompanyDetailsDto()
                {
                    Id = record.EngineerCompanyId,
                    CompanyId = record.CompanyId,
                    EngineerId = record.EngineerId,
                    ServicePrice = record.ServicePrice,
                    EngineerName = record.Engineer.EngineerName,
                    CompanyName = companies.CompanyName,
                    ServiveDate = record.ServiveDate,
                    Status = record.Status,
                    EngineerPhone = record.Engineer.EngineerPhone
                };
                EngineerCompanys.Add(order);
            }
            return Ok(EngineerCompanys);
        }


        [HttpGet("{EngineerName}", Name = "GetEngineerCompanysByEngineerName")]
        public async Task<IActionResult> GetEngineerCompanysByEngineerName([FromRoute] string EngineerName)
        {
            var records = await _EngineerCompanyServices.GetEngineerCompanyByEngineerName(EngineerName);
            if (records == null)
                return NotFound($"there was no service deals with this Engineer name {EngineerName}");

            var EngineerCompanys = new List<EngineerCompanyDetailsDto>();
            foreach (var record in records)
            {
                var companies = await _CompanyServices.GetCompanyById(record.CompanyId);
                var order = new EngineerCompanyDetailsDto()
                {
                    Id = record.EngineerCompanyId,
                    CompanyId = record.CompanyId,
                    EngineerId = record.EngineerId,
                    ServicePrice = record.ServicePrice,
                    EngineerName = record.Engineer.EngineerName,
                    CompanyName = companies.CompanyName,
                    ServiveDate = record.ServiveDate,
                    Status = record.Status,
                    EngineerPhone = record.Engineer.EngineerPhone
                };
                EngineerCompanys.Add(order);
            }
            return Ok(EngineerCompanys);
        }


        [HttpGet("{CompanyName}", Name = "GetEngineerCompanysByCompanyName")]
        public async Task<IActionResult> GetEngineerCompanysByCompanyName([FromRoute] string CompanyName)
        {
            var records = await _EngineerCompanyServices.GetEngineerCompanyByCompanyName(CompanyName);
            if (records == null)
                return NotFound($"there was no service deals with this Company name {CompanyName}");

            var EngineerCompanys = new List<EngineerCompanyDetailsDto>();
            foreach (var record in records)
            {
                var engineers = await _EngineerServices.GetEngineerById(record.EngineerId);
                var order = new EngineerCompanyDetailsDto()
                {
                    Id = record.EngineerCompanyId,
                    CompanyId = record.CompanyId,
                    EngineerId = record.EngineerId,
                    ServicePrice = record.ServicePrice,
                    EngineerName = engineers.EngineerName,
                    CompanyName = record.Company.CompanyName,
                    ServiveDate = record.ServiveDate,
                    Status =record.Status,
                    EngineerPhone = engineers.EngineerPhone
                };
                EngineerCompanys.Add(order);
            }
            return Ok(EngineerCompanys);
        }
    }
}
