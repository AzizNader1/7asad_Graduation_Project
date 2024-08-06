using GraduationApi.Interfaces;
using GraduationApi.Models;
using GraduationApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FarmerAccountsController : ControllerBase
    {
        private readonly IFarmerServices _farmerServices;
        private readonly IBankServices _bankServices;
        private readonly IFarmerAccountServices _farmerAccountServices;

        public FarmerAccountsController(IFarmerAccountServices farmerAccountServices, IBankServices bankServices, IFarmerServices farmerServices)
        {
            _farmerAccountServices = farmerAccountServices;
            _bankServices = bankServices;
            _farmerServices = farmerServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllFarmerAccounts()
        {
            var farmerAccounts = await _farmerAccountServices.GetAllFarmerAccounts();
            if (farmerAccounts == null)
                return NotFound("there is no farmer accounts avaliable");

            return Ok(farmerAccounts);
        }


        [HttpGet("{id}",Name = "GetFarmerAccountById")]
        public async Task<IActionResult> GetFarmerAccountById([FromRoute] int id)
        {
            var farmerAccount = await _farmerAccountServices.GetFarmerAccountById(id);
            if (farmerAccount == null)
                return NotFound($"there is no farmer account avaliable for this id {id}");

            return Ok(farmerAccount);
        }


        [HttpPost]
        public async Task<IActionResult> AddFarmerAccount(FarmerAccountDto dto)
        {
            var farmerAccount = new FarmerAccount
            {
                FarmerId = dto.FarmerId,
                BankId = dto.BankId,
                AccountNumber = dto.AccountNumber,
                AccountBalance = dto.AccountBalance,
                ExpireDate = dto.ExpireDate,
                AccountType = dto.AccountType,
                CvvNumber = dto.CvvNumber,
            };

           var result = await _farmerAccountServices.AddFarmerAccount(farmerAccount);

            return Ok(result);
        }


        [HttpPut("{id}",Name = "UpdateFarmerAccount")]
        public async Task<IActionResult> UpdateFarmerAccount([FromRoute] int id, [FromBody] FarmerAccountDto dto)
        {
            var farmerAccount = await _farmerAccountServices.GetFarmerAccountById(id);
            if (farmerAccount == null)
                return NotFound($"there is no avaliable farmer account for this id {id}");

            var isValidBank = await _bankServices.IsValidBank(dto.BankId);
            if (!isValidBank)
                return BadRequest($"there is no valid bank for this id {dto.BankId}");

            var isValidFarmer = await _farmerServices.IsValidFarmer(dto.FarmerId);
            if (!isValidFarmer)
                return BadRequest($"there is no valid farmer for this id {dto.FarmerId}");

            var result = _farmerAccountServices.UpdateFarmerAccount(farmerAccount);
            return Ok(result);
        }


        [HttpDelete("{id}",Name = "DeleteFarmerAccount")]
        public async Task<IActionResult> DeleteFarmerAccount([FromRoute] int id)
        {
            var farmerAccount = await _farmerAccountServices.GetFarmerAccountById(id);
            if (farmerAccount == null)
                return NotFound($"there is no avaliable farmer accounts for this id {id}");

           var result = _farmerAccountServices.DeleteFarmerAccount(farmerAccount);
            return Ok(result);
        }


        [HttpGet("{BankId}", Name = "GetFarmerAccountsByBankId")]
        public async Task<IActionResult> GetFarmerAccountsByBankId([FromRoute] int BankId)
        {
            var records = await _farmerAccountServices.GetFarmerAccountsByBankId(BankId);
            if (records == null)
                return NotFound($"there was no accounts from this bank id {BankId}");

            var farmerAccounts = new List<FarmerAccountDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _farmerServices.GetFarmerById(record.FarmerId);
                var order = new FarmerAccountDetailsDto()
                {
                    Id = record.FarmerAccountId,
                    BankId = record.BankId,
                    FarmerId = record.FarmerId,
                    AccountType = record.AccountType,
                    FarmerName = farmers.FarmerName,
                    BankName = record.Bank.BankName,
                    ExpireDate = record.ExpireDate,
                    AccountNumber = record.AccountNumber,
                    AccountBalance = record.AccountBalance,
                    CvvNumber = record.CvvNumber,
                };
                farmerAccounts.Add(order);
            }
            return Ok(farmerAccounts);
        }


        [HttpGet("{FarmerId}", Name = "GetFarmerAccountsByFarmerId")]
        public async Task<IActionResult> GetFarmerAccountsByFarmerId([FromRoute] int FarmerId)
        {
            var records = await _farmerAccountServices.GetFarmerAccountsByFarmerId(FarmerId);
            if (records == null)
                return NotFound($"there was no accounts for this farmer id {FarmerId}");

            var farmerAccounts = new List<FarmerAccountDetailsDto>();
            foreach (var record in records)
            {
                var banks = await _bankServices.GetBankById(record.BankId);
                var order = new FarmerAccountDetailsDto()
                {
                    Id = record.FarmerAccountId,
                    BankId = record.BankId,
                    FarmerId = record.FarmerId,
                    AccountType = record.AccountType,
                    FarmerName = record.Farmer.FarmerName,
                    BankName = banks.BankName,
                    ExpireDate = record.ExpireDate,
                    AccountNumber =record.AccountNumber,
                    AccountBalance = record.AccountBalance,
                    CvvNumber = record.CvvNumber,
                };
                farmerAccounts.Add(order);
            }
            return Ok(farmerAccounts);
        }


        [HttpGet("{FarmerName}", Name = "GetFarmerAccountsByFarmerName")]
        public async Task<IActionResult> GetFarmerAccountsByFarmerName([FromRoute] string FarmerName)
        {
            var records = await _farmerAccountServices.GetFarmerAccountsByFarmerName(FarmerName);
            if (records == null)
                return NotFound($"there was no accounts for this farmer name {FarmerName}");

            var farmerAccounts = new List<FarmerAccountDetailsDto>();
            foreach (var record in records)
            {
                var banks = await _bankServices.GetBankById(record.BankId);
                var order = new FarmerAccountDetailsDto()
                {
                    Id = record.FarmerAccountId,
                    BankId = record.BankId,
                    FarmerId = record.FarmerId,
                    AccountType = record.AccountType,
                    FarmerName = record.Farmer.FarmerName,
                    BankName = banks.BankName,
                    ExpireDate = record.ExpireDate,
                    AccountNumber = record.AccountNumber,
                    AccountBalance = record.AccountBalance,
                    CvvNumber = record.CvvNumber,
                };
                farmerAccounts.Add(order);
            }
            return Ok(farmerAccounts);
        }


        [HttpGet("{BankName}", Name = "GetFarmerAccountsByBankName")]
        public async Task<IActionResult> GetFarmerAccountsByBankName([FromRoute] string BankName)
        {
            var records = await _farmerAccountServices.GetFarmerAccountsByBankName(BankName);
            if (records == null)
                return NotFound($"there was no accounts froms this bank name {BankName}");

            var farmerAccounts = new List<FarmerAccountDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _farmerServices.GetFarmerById(record.FarmerId);
                var order = new FarmerAccountDetailsDto()
                {
                    Id = record.FarmerAccountId,
                    BankId = record.BankId,
                    FarmerId = record.FarmerId,
                    AccountType = record.AccountType,
                    FarmerName = farmers.FarmerName,
                    BankName = record.Bank.BankName,
                    ExpireDate = record.ExpireDate,
                    AccountNumber = record.AccountNumber,
                    AccountBalance = record.AccountBalance,
                    CvvNumber = record.CvvNumber,
                };
                farmerAccounts.Add(order);
            }
            return Ok(farmerAccounts);
        }
    }
}
