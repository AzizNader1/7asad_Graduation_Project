using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompanyAccountsController : ControllerBase
    {
        private readonly ICompanyServices _CompanyServices;
        private readonly IBankServices _bankServices;
        private readonly ICompanyAccountServices _companyAccountServices;

        public CompanyAccountsController(ICompanyAccountServices CompanyAccountServices, IBankServices bankServices, ICompanyServices CompanyServices)
        {
            _companyAccountServices = CompanyAccountServices;
            _bankServices = bankServices;
            _CompanyServices = CompanyServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCompanyAccounts()
        {
            var CompanyAccounts = await _companyAccountServices.GetAllCompanyAccounts();
            if (CompanyAccounts == null)
                return NotFound("there is no Company accounts avaliable");

            return Ok(CompanyAccounts);
        }


        [HttpGet("{id}", Name = "GetCompanyAccountById")]
        public async Task<IActionResult> GetCompanyAccountById([FromRoute] int id)
        {
            var CompanyAccount = await _companyAccountServices.GetCompanyAccountById(id);
            if (CompanyAccount == null)
                return NotFound($"there is no Company account avaliable for this id {id}");

            return Ok(CompanyAccount);
        }


        [HttpPost]
        public async Task<IActionResult> AddCompanyAccount(CompanyAccountDto dto)
        {
            var CompanyAccount = new CompanyAccount
            {
                CompanyId = dto.CompanyId,
                BankId = dto.BankId,
                AccountNumber = dto.AccountNumber,
                AccountBalance = dto.AccountBalance,
                CvvNumber = dto.CvvNumber,
                ExpireDate = dto.ExpireDate,
                AccountType = dto.AccountType
            };

           var result = await _companyAccountServices.AddCompanyAccount(CompanyAccount);

            return Ok(result);
        }


        [HttpPut("{id}", Name = "UpdateCompanyAccount")]
        public async Task<IActionResult> UpdateCompanyAccount([FromRoute] int id, [FromBody] CompanyAccountDto dto)
        {
            var CompanyAccount = await _companyAccountServices.GetCompanyAccountById(id);
            if (CompanyAccount == null)
                return NotFound($"there is no avaliable Company account for this id {id}");

            var isValidBank = await _bankServices.IsValidBank(dto.BankId);
            if (!isValidBank)
                return BadRequest($"there is no valid bank for this id {dto.BankId}");

            var isValidCompany = await _CompanyServices.IsValidCompany(dto.CompanyId);
            if (!isValidCompany)
                return BadRequest($"there is no valid Company for this id {dto.CompanyId}");

            var result = _companyAccountServices.UpdateCompanyAccount(CompanyAccount);
            return Ok(result);
        }


        [HttpDelete("{id}", Name = "DeleteCompanyAccount")]
        public async Task<IActionResult> DeleteCompanyAccount([FromRoute] int id)
        {
            var CompanyAccount = await _companyAccountServices.GetCompanyAccountById(id);
            if (CompanyAccount == null)
                return NotFound($"there is no avaliable Company accounts for this id {id}");

            var result = _companyAccountServices.DeleteCompanyAccount(CompanyAccount);
            return Ok(result);
        }


        [HttpGet("{BankId}", Name = "GetCompanyAccountsByBankId")]
        public async Task<IActionResult> GetCompanyAccountsByBankId([FromRoute] int BankId)
        {
            var records = await _companyAccountServices.GetCompanyAccountsByBankId(BankId);
            if (records == null)
                return NotFound($"there was no accounts from this bank id {BankId}");

            var CompanyAccounts = new List<CompanyAccountDetailsDto>();
            foreach (var record in records)
            {
                var companies = await _CompanyServices.GetCompanyById(record.CompanyId);
                var order = new CompanyAccountDetailsDto()
                {
                    Id = record.CompanyAccountId,
                    BankId = record.BankId,
                    CompanyId = record.CompanyId,
                    AccountNumber = record.AccountNumber,
                    AccountType = record.AccountType,
                    CompanyName = companies.CompanyName,
                    BankName = record.Bank.BankName,
                    ExpireDate = record.ExpireDate,
                    AccountBalance = record.AccountBalance,
                    CvvNumber = record.CvvNumber
                };
                CompanyAccounts.Add(order);
            }
            return Ok(CompanyAccounts);
        }


        [HttpGet("{CompanyId}",Name ="GetCompanyAccountsByCompanyId")]
        public async Task<IActionResult> GetCompanyAccountsByCompanyId([FromRoute] int CompanyId)
        {
            var records = await _companyAccountServices.GetCompanyAccountsByCompanyId(CompanyId);
            if (records == null)
                return NotFound($"there was no accounts for this Company id {CompanyId}");

            var CompanyAccounts = new List<CompanyAccountDetailsDto>();
            foreach (var record in records)
            {
                var banks = await _bankServices.GetBankById(record.BankId);
                var order = new CompanyAccountDetailsDto()
                {
                    Id = record.CompanyAccountId,
                    BankId = record.BankId,
                    CompanyId = record.CompanyId,
                    AccountType = record.AccountType,
                    AccountNumber = record.AccountNumber,
                    CompanyName = record.Company.CompanyName,
                    BankName = banks.BankName,
                    ExpireDate = record.ExpireDate,
                    AccountBalance = record.AccountBalance,
                    CvvNumber = record.CvvNumber
                };
                CompanyAccounts.Add(order);
            }
            return Ok(CompanyAccounts);
        }


        [HttpGet("{CompanyName}",Name ="GetCompanyAccountsByCompanyName")]
        public async Task<IActionResult> GetCompanyAccountsByCompanyName([FromRoute] string CompanyName)
        {
            var records = await _companyAccountServices.GetCompanyAccountsByCompanyName(CompanyName);
            if (records == null)
                return NotFound($"there was no accounts for this Company name {CompanyName}");

            var CompanyAccounts = new List<CompanyAccountDetailsDto>();
            foreach (var record in records)
            {
                var banks = await _bankServices.GetBankById(record.BankId);
                var order = new CompanyAccountDetailsDto()
                {
                    Id = record.CompanyAccountId,
                    BankId = record.BankId,
                    CompanyId = record.CompanyId,
                    AccountType = record.AccountType,
                    AccountNumber = record.AccountNumber,
                    CompanyName = record.Company.CompanyName,
                    BankName = banks.BankName,
                    ExpireDate = record.ExpireDate,
                    AccountBalance = record.AccountBalance,
                    CvvNumber = record.CvvNumber
                };
                CompanyAccounts.Add(order);
            }
            return Ok(CompanyAccounts);
        }
        

        [HttpGet("{BankName}", Name ="GetCompanyAccountsByBankName")]
        public async Task<IActionResult> GetCompanyAccountsByBankName([FromRoute] string BankName)
        {
            var records = await _companyAccountServices.GetCompanyAccountsByBankName(BankName);
            if (records == null)
                return NotFound($"there was no accounts froms this bank name {BankName}");

            var CompanyAccounts = new List<CompanyAccountDetailsDto>();
            foreach (var record in records)
            {
                var companies = await _CompanyServices.GetCompanyById(record.CompanyId);
                var order = new CompanyAccountDetailsDto()
                {
                    Id = record.CompanyAccountId,
                    BankId = record.BankId,
                    CompanyId = record.CompanyId,
                    AccountType = record.AccountType,
                    AccountNumber = record.AccountNumber,
                    CompanyName = companies.CompanyName,
                    BankName = record.Bank.BankName,
                    ExpireDate = record.ExpireDate,
                    AccountBalance = record.AccountBalance,
                    CvvNumber=record.CvvNumber
                };
                CompanyAccounts.Add(order);
            }
            return Ok(CompanyAccounts);
        }
    }
}
