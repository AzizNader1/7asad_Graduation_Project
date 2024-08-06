using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RepresintorsController : ControllerBase
    {
        private readonly IRepresintorServices _represintorServices;
        private readonly ICompanyServices _companyServices;

        public RepresintorsController(IRepresintorServices represintorServices, ICompanyServices companyServices)
        {
            _represintorServices = represintorServices;
            _companyServices = companyServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllRepresintors()
        {
            var represintors = await _represintorServices.GetAllRepresintors();
            if (represintors == null)
                return NotFound("there is no represintors avaliable");

            return Ok(represintors);
        }


        [HttpGet("{id}", Name = "GetRepresintorById")]
        public async Task<IActionResult> GetRepresintorById([FromRoute] int id)
        {
            var represintor = await _represintorServices.GetRepresintorById(id);
            if (represintor == null)
                return NotFound($"there is no avaliable represintors for this id {id}");

            return Ok(represintor);
        }


        [HttpGet("{RepresintorName}", Name = "GetRepresintorByName")]
        public async Task<IActionResult> GetRepresintorByName([FromRoute] string RepresintorName)
        {
            var represintor = await _represintorServices.GetRepresintorByName(RepresintorName);
            if (represintor == null)
                return NotFound($"there is no avaliable represintors for this name :- {RepresintorName}");

            return Ok(represintor);
        }


        [HttpDelete("{id}", Name = "DeleteRepresintor")]
        public async Task<IActionResult> DeleteRepresintor([FromRoute] int id)
        {
            var represintor = await _represintorServices.GetRepresintorById(id);
            if (represintor == null)
                return NotFound($"there is no avaliable represintors for this {id}");

          var result =  _represintorServices.DeleteRepresintor(represintor);
            return Ok(result);
        }


        [HttpPut("{id}", Name = "UpdateRepresintor")]
        public async Task<IActionResult> UpdateRepresintor([FromRoute] int id, [FromBody] RepresintorDto represintorDto)
        {
            var represintor = await _represintorServices.GetRepresintorById(id);
            if (represintor == null)
                return NotFound($"there is no represintors for this id {id}");

            var isValidCompany = await _companyServices.IsValidCompany(represintorDto.CompanyId);
            if (!isValidCompany)
                return NotFound($"there is no valid companies for this id {represintorDto.CompanyId}");

            represintor.RepresintorName = represintorDto.RepresintorName;
            represintor.RepresintorPhone = represintorDto.RepresintorPhone;
            represintor.RepresintorEmail = represintorDto.RepresintorEmail;
            represintor.CompanyId = represintorDto.CompanyId;

           var result = _represintorServices.UpdateRepresintor(represintor);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddRepresintor([FromBody] RepresintorDto represintorDto)
        {
            var represintor = new Represintor()
            {
                RepresintorName = represintorDto.RepresintorName,
                RepresintorPhone = represintorDto.RepresintorPhone,
                RepresintorEmail = represintorDto.RepresintorEmail,
                CompanyId = represintorDto.CompanyId
            };
           var result = _represintorServices.AddRepresintor(represintor);
            return Ok(result);
        }


        [HttpGet("{CompanyId}", Name ="GetRepresintorsByCompanyId")]
        public async Task<IActionResult> GetRepresintorsByCompanyId([FromRoute]int CompanyId)
        {
            var records = await _represintorServices.GetRepresintorsByCompanyId(CompanyId);
            if (records == null)
                return NotFound($"there was no represintors for this company id {CompanyId}");

            var companyRepresintors = new List<RepresintorDetailsDto>();
            foreach (var record in records)
            {
                var order = new RepresintorDetailsDto()
                {

                    Id = record.RepresintorId,
                    CompanyId = record.CompanyId,
                    CompanyName = record.Company.CompanyName,
                    RepresintorName = record.RepresintorName,
                    RepresintorPhone = record.RepresintorPhone,
                    RepresintorEmail = record.RepresintorEmail,
                };
                companyRepresintors.Add(order);
            }
            return Ok(companyRepresintors);
        }


        [HttpGet("{CompanyName}", Name = "GetRepresintorsByCompanyName")]
        public async Task<IActionResult> GetRepresintorsByCompanyName([FromRoute] string CompanyName)
        {
            var records = await _represintorServices.GetRepresintorsByCompanyName(CompanyName);
            if (records == null)
                return NotFound($"there was no represintors for this company name {CompanyName}");

            var companyRepresintors = new List<RepresintorDetailsDto>();
            foreach (var record in records)
            {
                var order = new RepresintorDetailsDto()
                {

                    Id = record.RepresintorId,
                    CompanyId = record.CompanyId,
                    CompanyName = record.Company.CompanyName,
                    RepresintorName = record.RepresintorName,
                    RepresintorPhone = record.RepresintorPhone,
                    RepresintorEmail = record.RepresintorEmail,
                };
                companyRepresintors.Add(order);
            }
            return Ok(companyRepresintors);
        }

    }
}
