using GraduationApi.Interfaces;
using GraduationApi.Models;
using GraduationApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LandOrdersController : ControllerBase
    {
        private readonly ILandOrderServices _LandOrder;
        private readonly ICompanyServices _companyServices;
        private readonly IFarmerServices _farmerServices;

        public LandOrdersController(ILandOrderServices LandOrder, ICompanyServices companyServices, IFarmerServices farmerServices)
        {
            _LandOrder = LandOrder;
            _companyServices = companyServices;
            _farmerServices = farmerServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllLandOrders()
        {
            var LandOrders = await _LandOrder.GetAllLandOrders();
            if (LandOrders == null)
                return NotFound("there is no land orders avaliable");

            return Ok(LandOrders);
        }


        [HttpGet("{id}", Name = "GetLandOrderById")]
        public async Task<IActionResult> GetLandOrderById([FromRoute] int id)
        {
            var LandOrders = await _LandOrder.GetLandOrderById(id);
            if (LandOrders == null)
                return NotFound($"there is no land orders avaliable for this id {id}");

            return Ok(LandOrders);
        }


        [HttpPost]
        public async Task<IActionResult> AddLandOrder(LandOrderDto dto)
        {
            var LandOrder = new LandOrder
            {
                CompanyId = dto.CompanyId,
                FarmerId = dto.FarmerId,
                OrderPrice = dto.OrderPrice,
                LandSize = dto.LandSize,
                OrderStartDate = dto.OrderStartDate,
                OrderEndDate = dto.OrderEndDate,
                LandRentStatus = dto.LandRentStatus,
                LandId = dto.LandId
                
            };

          var result =  await _LandOrder.AddLandOrder(LandOrder);

            return Ok(result);
        }


        [HttpPut("{id}",Name = "UpdateLandOrder")]
        public async Task<IActionResult> UpdateLandOrder([FromRoute] int id, [FromBody] LandOrderDto dto)
        {
            var LandOrders = await _LandOrder.GetLandOrderById(id);
            if (LandOrders == null)
                return NotFound($"there is no avaliable land orders for this id {id}");

            var isValidCompany = await _companyServices.IsValidCompany(dto.CompanyId);
            if (!isValidCompany)
                return BadRequest($"there is no valid company for this id {dto.CompanyId}");

            var isValidFarmer = await _farmerServices.IsValidFarmer(dto.FarmerId);
            if (!isValidFarmer)
                return BadRequest($"there is no valid farmer for this id {dto.FarmerId}");

            LandOrders.FarmerId = dto.FarmerId;
            LandOrders.CompanyId = dto.CompanyId;
            LandOrders.LandId = dto.LandId;
            LandOrders.LandOrderId = id;
            LandOrders.LandRentStatus = dto.LandRentStatus;
            LandOrders.LandSize = dto.LandSize;
            LandOrders.OrderEndDate = dto.OrderEndDate;
            LandOrders.OrderPrice = dto.OrderPrice;
            LandOrders.OrderStartDate = dto.OrderStartDate;



           var result = _LandOrder.UpdateLandOrder(LandOrders);
            return Ok(result);
        }


        [HttpDelete("{id}",Name = "DeleteLandOrder")]
        public async Task<IActionResult> DeleteLandOrder([FromRoute] int id)
        {
            var LandOrders = await _LandOrder.GetLandOrderById(id);
            if (LandOrders == null)
                return NotFound($"there is no avaliable land orders for this id {id}");

           var result = _LandOrder.DeleteLandOrder(LandOrders);
            return Ok(result);
        }


        [HttpGet("{FarmerId}", Name ="GetOrdersByFarmerId")]
        public async Task<IActionResult> GetOrdersByFarmerId([FromRoute]int FarmerId)
        {
            var records = await _LandOrder.GetOrdersByFarmerId(FarmerId);
            if (records == null)
                return NotFound($"there was no records for this farmer id {FarmerId}");

            var farmerOrders = new List<LandOrderDetailsDto>();
            foreach (var record in records)
            {
                var companies = await _companyServices.GetCompanyById(record.CompanyId);
                var order = new LandOrderDetailsDto()
                {
                    Id = record.LandOrderId,
                    CompanyId = record.CompanyId,
                    FarmerId = record.FarmerId,
                    CompanyName = companies.CompanyName,
                    FarmerName = record.Farmer.FarmerName,
                    OrderPrice = record.OrderPrice,
                    LandSize = record.LandSize,
                    OrderStartDate = record.OrderStartDate,
                    OrderEndDate = record.OrderEndDate,
                    LandRentStatus = record.LandRentStatus,
                    LandId = record.LandId,
                    FarmerPhone = record.Farmer.FarmerPhone
                };
                farmerOrders.Add(order);
            }
            return Ok(farmerOrders);
        }


        [HttpGet("{CompanyId}", Name = "GetOrdersByCompanyId")]
        public async Task<IActionResult> GetOrdersByCompanyId([FromRoute]int CompanyId)
        {
            var records = await _LandOrder.GetOrdersByCompanyId(CompanyId);
            if (records == null)
                return NotFound($"there was no records for this company id {CompanyId}");

            var companyOrders = new List<LandOrderDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _farmerServices.GetFarmerById(record.FarmerId);
                var order = new LandOrderDetailsDto()
                {
                    Id = record.LandOrderId,
                    CompanyId = record.CompanyId,
                    FarmerId = record.FarmerId,
                    CompanyName = record.Company.CompanyName,
                    FarmerName = farmers.FarmerName,
                    OrderPrice = record.OrderPrice,
                    LandSize = record.LandSize,
                    OrderStartDate = record.OrderStartDate,
                    OrderEndDate = record.OrderEndDate,
                    LandRentStatus = record.LandRentStatus,
                    LandId = record.LandId,
                    FarmerPhone = farmers.FarmerPhone
                };
                companyOrders.Add(order);
            }
            return Ok(companyOrders);
        }


        [HttpGet("{CompanyName}", Name ="GetOrdersByCompanyName")]
        public async Task<IActionResult> GetOrdersByCompanyName([FromRoute] string CompanyName)
        {
            var records = await _LandOrder.GetOrdersByCompanyName(CompanyName);
            if (records == null)
                return NotFound($"there was no records for this company name {CompanyName}");

            var companyOrders = new List<LandOrderDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _farmerServices.GetFarmerById(record.FarmerId);
                var order = new LandOrderDetailsDto()
                {
                    Id = record.LandOrderId,
                    CompanyId = record.CompanyId,
                    FarmerId = record.FarmerId,
                    CompanyName = record.Company.CompanyName,
                    FarmerName = farmers.FarmerName,
                    OrderPrice = record.OrderPrice,
                    LandSize = record.LandSize,
                    OrderStartDate = record.OrderStartDate,
                    OrderEndDate = record.OrderEndDate,
                    LandRentStatus  = record.LandRentStatus,
                    LandId = record.LandId,
                    FarmerPhone = farmers.FarmerPhone
                };
                companyOrders.Add(order);
            }
            return Ok(companyOrders);
        }


        [HttpGet("{FarmerName}", Name ="GetOrdersByFarmerName")]
        public async Task<IActionResult> GetOrdersByFarmerName([FromRoute]string FarmerName)
        {
            var records = await _LandOrder.GetOrdersByFarmerName(FarmerName);
            if (records == null)
                return NotFound($"there was no records for this farmer name {FarmerName}");

            var farmerOrders = new List<LandOrderDetailsDto>();
            foreach (var record in records)
            {
                var companies = await _companyServices.GetCompanyById(record.CompanyId);
                var order = new LandOrderDetailsDto()
                {
                    Id = record.LandOrderId,
                    CompanyId = record.CompanyId,
                    FarmerId = record.FarmerId,
                    CompanyName = companies.CompanyName,
                    FarmerName = record.Farmer.FarmerName,
                    OrderPrice = record.OrderPrice,
                    LandSize = record.LandSize,
                    OrderStartDate = record.OrderStartDate,
                    OrderEndDate = record.OrderEndDate,
                    LandRentStatus = record.LandRentStatus,
                    LandId = record.LandId,
                    FarmerPhone = record.Farmer.FarmerPhone
                };
                farmerOrders.Add(order);
            }
            return Ok(farmerOrders);
        }

    }
}
