using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EngineerAccountsController : ControllerBase
    {
        private readonly IEngineerServices _EngineerServices;
        private readonly IBankServices _bankServices;
        private readonly IEngineerAccountServices _EngineerAccountServices;

        public EngineerAccountsController(IEngineerAccountServices EngineerAccountServices, IBankServices bankServices, IEngineerServices EngineerServices)
        {
            _EngineerAccountServices = EngineerAccountServices;
            _bankServices = bankServices;
            _EngineerServices = EngineerServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEngineerAccounts()
        {
            var EngineerAccounts = await _EngineerAccountServices.GetAllEngineerAccounts();
            if (EngineerAccounts == null)
                return NotFound("there is no Engineer accounts avaliable");

            return Ok(EngineerAccounts);
        }


        [HttpGet("{id}",Name = "GetEngineerAccountById")]
        public async Task<IActionResult> GetEngineerAccountById([FromRoute] int id)
        {
            var EngineerAccount = await _EngineerAccountServices.GetEngineerAccountById(id);
            if (EngineerAccount == null)
                return NotFound($"there is no Engineer account avaliable for this id {id}");

            return Ok(EngineerAccount);
        }


        [HttpPost]
        public async Task<IActionResult> AddEngineerAccount(EngineerAccountDto dto)
        {
            var EngineerAccount = new EngineerAccount
            {
                EngineerId = dto.EngineerId,
                BankId = dto.BankId,
                AccountNumber = dto.AccountNumber,
                AccountBalance = dto.AccountBalance,
                ExpireDate = dto.ExpireDate,
                AccountType = dto.AccountType,
                CvvNumber = dto.CvvNumber,
            };

            var result = await _EngineerAccountServices.AddEngineerAccount(EngineerAccount);

            return Ok(result);
        }


        [HttpPut("{id}",Name = "UpdateEngineerAccount")]
        public async Task<IActionResult> UpdateEngineerAccount([FromRoute] int id, [FromBody] EngineerAccountDto dto)
        {
            var EngineerAccount = await _EngineerAccountServices.GetEngineerAccountById(id);
            if (EngineerAccount == null)
                return NotFound($"there is no avaliable Engineer account for this id {id}");

            var isValidBank = await _bankServices.IsValidBank(dto.BankId);
            if (!isValidBank)
                return BadRequest($"there is no valid bank for this id {dto.BankId}");

            var isValidEngineer = await _EngineerServices.IsValidEngineer(dto.EngineerId);
            if (!isValidEngineer)
                return BadRequest($"there is no valid Engineer for this id {dto.EngineerId}");

            var result = _EngineerAccountServices.UpdateEngineerAccount(EngineerAccount);
            return Ok(result);
        }


        [HttpDelete("{id}",Name = "DeleteEngineerAccount")]
        public async Task<IActionResult> DeleteEngineerAccount([FromRoute] int id)
        {
            var EngineerAccount = await _EngineerAccountServices.GetEngineerAccountById(id);
            if (EngineerAccount == null)
                return NotFound($"there is no avaliable Engineer accounts for this id {id}");

            var result = _EngineerAccountServices.DeleteEngineerAccount(EngineerAccount);
            return Ok(result);
        }


        [HttpGet("{BankId}", Name = "GetEngineerAccountsByBankId")]
        public async Task<IActionResult> GetEngineerAccountsByBankId([FromRoute] int BankId)
        {
            var records = await _EngineerAccountServices.GetEngineerAccountsByBankId(BankId);
            if (records == null)
                return NotFound($"there was no accounts from this bank id {BankId}");

            var EngineerAccounts = new List<EngineerAccountDetailsDto>();
            foreach (var record in records)
            {
                var engineers = await _EngineerServices.GetEngineerById(record.EngineerId);
                var order = new EngineerAccountDetailsDto()
                {
                    Id = record.EngineerAccountId,
                    BankId = record.BankId,
                    EngineerId = record.EngineerId,
                    AccountType = record.AccountType,
                    AccountNumber = record.AccountNumber,
                    EngineerName = engineers.EngineerName,
                    BankName = record.Bank.BankName,
                    ExpireDate = record.ExpireDate,
                    AccountBalance = record.AccountBalance,
                    CvvNumber = record.CvvNumber
                };
                EngineerAccounts.Add(order);
            }
            return Ok(EngineerAccounts);
        }


        [HttpGet("{EngineerId}", Name = "GetEngineerAccountsByEngineerId")]
        public async Task<IActionResult> GetEngineerAccountsByEngineerId([FromRoute] int EngineerId)
        {
            var records = await _EngineerAccountServices.GetEngineerAccountsByEngineerId(EngineerId);
            if (records == null)
                return NotFound($"there was no accounts for this Engineer id {EngineerId}");

            var EngineerAccounts = new List<EngineerAccountDetailsDto>();
            foreach (var record in records)
            {
                var banks = await _bankServices.GetBankById(record.BankId);
                var order = new EngineerAccountDetailsDto()
                {
                    Id = record.EngineerAccountId,
                    BankId = record.BankId,
                    EngineerId = record.EngineerId,
                    AccountType = record.AccountType,
                    AccountNumber = record.AccountNumber,
                    EngineerName = record.Engineer.EngineerName,
                    BankName = banks.BankName,
                    ExpireDate = record.ExpireDate,
                    AccountBalance = record.AccountBalance,
                    CvvNumber = record.CvvNumber
                };
                EngineerAccounts.Add(order);
            }
            return Ok(EngineerAccounts);
        }


        [HttpGet("{EngineerName}", Name = "GetEngineerAccountsByEngineerName")]
        public async Task<IActionResult> GetEngineerAccountsByEngineerName([FromRoute] string EngineerName)
        {
            var records = await _EngineerAccountServices.GetEngineerAccountsByEngineerName(EngineerName);
            if (records == null)
                return NotFound($"there was no accounts for this Engineer name {EngineerName}");

            var EngineerAccounts = new List<EngineerAccountDetailsDto>();
            foreach (var record in records)
            {
                var banks = await _bankServices.GetBankById(record.BankId);
                var order = new EngineerAccountDetailsDto()
                {
                    Id = record.EngineerAccountId,
                    BankId = record.BankId,
                    EngineerId = record.EngineerId,
                    AccountType = record.AccountType,
                    AccountNumber = record.AccountNumber,
                    EngineerName = record.Engineer.EngineerName,
                    BankName = banks.BankName,
                    ExpireDate = record.ExpireDate,
                    AccountBalance = record.AccountBalance,
                    CvvNumber = record.CvvNumber
                };
                EngineerAccounts.Add(order);
            }
            return Ok(EngineerAccounts);
        }


        [HttpGet("{BankName}", Name = "GetEngineerAccountsByBankName")]
        public async Task<IActionResult> GetEngineerAccountsByBankName([FromRoute] string BankName)
        {
            var records = await _EngineerAccountServices.GetEngineerAccountsByBankName(BankName);
            if (records == null)
                return NotFound($"there was no accounts froms this bank name {BankName}");

            var EngineerAccounts = new List<EngineerAccountDetailsDto>();
            foreach (var record in records)
            {
                var engineers = await _EngineerServices.GetEngineerById(record.EngineerId);
                var order = new EngineerAccountDetailsDto()
                {
                    Id = record.EngineerAccountId,
                    BankId = record.BankId,
                    EngineerId = record.EngineerId,
                    AccountType = record.AccountType,
                    AccountNumber = record.AccountNumber,
                    EngineerName = engineers.EngineerName,
                    BankName = record.Bank.BankName,
                    ExpireDate = record.ExpireDate,
                    AccountBalance = record.AccountBalance,
                    CvvNumber = record.CvvNumber
                };
                EngineerAccounts.Add(order);
            }
            return Ok(EngineerAccounts);
        }
    }
}
