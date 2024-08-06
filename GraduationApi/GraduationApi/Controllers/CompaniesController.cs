using GraduationApi.Interfaces;
using GraduationApi.Models;
using GraduationApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyServices _companyServices;

        public CompaniesController(ICompanyServices companyServices)
        {
            _companyServices = companyServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _companyServices.GetAllCompanies();
            if (companies == null)
                return NotFound("there is no companies avaliable");
            return Ok(companies);
        }


        [HttpGet("{id}",Name ="GetCompanyById")]
        public async Task<IActionResult> GetCompanyById([FromRoute] int id)
        {
            var company = await _companyServices.GetCompanyById(id);
            if (company == null)
                return NotFound($"there is no avaliable company for this id {id}");

            return Ok(company);
        }


        [HttpGet("{CompanyName}", Name ="GetCompanyByName")]
        public async Task<IActionResult> GetCompanyByName([FromRoute] string CompanyName)
        {
            var company = await _companyServices.GetCompanyByName(CompanyName);
            if (company == null)
                return NotFound($"there is no avaliable company for this name :- {CompanyName}");

            return Ok(company);
        }


        [HttpDelete("{id}",Name = "DeleteCompany")]
        public async Task<IActionResult> DeleteCompany([FromRoute] int id)
        {
            var company = await _companyServices.GetCompanyById(id);
            if (company == null)
                return NotFound($"there is no avaliable company for this {id}");

           var result =  _companyServices.DeleteCompany(company);
           return Ok(result);
        }


        [HttpPut("{id}", Name = "UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromRoute] int id, [FromBody] CompanyDto companyDto)
        {
            var company = await _companyServices.GetCompanyById(id);
            if (company == null)
                return NotFound($"there is no company for this id {id}");
           
            company.CompanyName = companyDto.CompanyName;
            company.CompanyAddress = companyDto.CompanyAddress;
            company.CompanyEmail = companyDto.CompanyEmail;
            company.CompanyPassword = companyDto.CompanyPassword;
            company.CompanyType = companyDto.CompanyType;
            
            var result = _companyServices.UpdateCompany(company);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddCompany([FromBody] CompanyDto companyDto)
        {
            var company = new Company() 
            {
                CompanyName = companyDto.CompanyName,
                CompanyAddress = companyDto.CompanyAddress,
                CompanyEmail = companyDto.CompanyEmail,
                CompanyPassword = companyDto.CompanyPassword,
                CompanyType = companyDto.CompanyType,
            };
            
            var result = _companyServices.AddCompany(company);
            return Ok(result);
        }
    }
}
