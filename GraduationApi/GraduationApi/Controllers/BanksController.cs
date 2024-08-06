using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly IBankServices _bankServices;

        public BanksController(IBankServices bankServices)
        {
            _bankServices = bankServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBanks()
        {
            var banks = await _bankServices.GetAllBanks();
            if (banks == null)
                return NotFound("there is no banks avaliable");

            return Ok(banks);
        }


        [HttpGet("{id}",Name = "GetBankById")]
        public async Task<IActionResult> GetBankById([FromRoute] int id)
        {
            var bank = await _bankServices.GetBankById(id);
            if (bank == null)
                return NotFound($"there is no avaliable banks for this id {id}");

            return Ok(bank);
        }


        [HttpGet("{BankName}", Name = "GetBankByName")]
        public async Task<IActionResult> GetBankByName([FromRoute] string BankName)
        {
            var bank = await _bankServices.GetBankByName(BankName);
            if (bank == null)
                return NotFound($"there is no avaliable banks for this name :- {BankName}");

            return Ok(bank);
        }


        [HttpDelete("{id}",Name ="DeleteBank")]
        public async Task<IActionResult> DeleteBank([FromRoute] int id)
        {
            var bank = await _bankServices.GetBankById(id);
            if (bank == null)
                return NotFound($"there is no avaliable banks for this {id}");

            var result = _bankServices.DeleteBank(bank);
            return Ok(result);
        }


        [HttpPut("{id}",Name = "UpdateBank")]
        public async Task<IActionResult> UpdateBank([FromRoute]int id ,[FromBody] BankDto bankDto)
        {
            var bank = await _bankServices.GetBankById(id);
            if (bank == null)
                return NotFound($"there is no banks for this id {id}");

            bank.BankName = bankDto.BankName;

            var result = _bankServices.UpdateBank(bank);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddBank([FromBody]BankDto bankDto)
        {
            var bank = new Bank
            {
                BankName = bankDto.BankName
            };

           var result = await _bankServices.AddBank(bank);
           return Ok(result);
        }
    }
}
