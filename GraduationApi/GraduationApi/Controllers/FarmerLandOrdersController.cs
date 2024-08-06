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
    public class FarmerLandOrdersController : ControllerBase
    {
        private readonly IFarmerLandOrderServices _FarmerLandOrder;
        private readonly IBuyerFarmerServices _buyerFarmerServices;
        private readonly IFarmerServices _farmerServices;

        public FarmerLandOrdersController(IFarmerLandOrderServices FarmerLandOrder, IBuyerFarmerServices buyerFarmerServices, IFarmerServices farmerServices)
        {
            _FarmerLandOrder = FarmerLandOrder;
            _buyerFarmerServices = buyerFarmerServices;
            _farmerServices = farmerServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllFarmerLandOrders()
        {
            var LandOrders = await _FarmerLandOrder.GetAllFarmerLandOrders();
            if (LandOrders == null)
                return NotFound("there is no land orders avaliable");

            return Ok(LandOrders);
        }


        [HttpGet("{id}", Name = "GetFarmerLandOrderById")]
        public async Task<IActionResult> GetFarmerLandOrderById([FromRoute] int id)
        {
            var LandOrders = await _FarmerLandOrder.GetFarmerLandOrderById(id);
            if (LandOrders == null)
                return NotFound($"there is no land orders avaliable for this id {id}");

            return Ok(LandOrders);
        }


        [HttpPost]
        public async Task<IActionResult> AddFarmerLandOrder(FarmerLandOrderDto dto)
        {
            var LandOrder = new FarmerLandOrder
            {
                BuyerFarmerId = dto.BuyerFarmerId,
                FarmerId = dto.FarmerId,
                OrderPrice = dto.OrderPrice,
                LandSize = dto.LandSize,
                OrderStartDate = dto.OrderStartDate,
                OrderEndDate = dto.OrderEndDate,
                LandRentStatus = dto.LandRentStatus,
                LandId = dto.LandId
                
            };

          var result =  await _FarmerLandOrder.AddFarmerLandOrder(LandOrder);

            return Ok(result);
        }


        [HttpPut("{id}",Name = "UpdateFarmerLandOrder")]
        public async Task<IActionResult> UpdateFarmerLandOrder([FromRoute] int id, [FromBody] FarmerLandOrderDto dto)
        {
            var LandOrders = await _FarmerLandOrder.GetFarmerLandOrderById(id);
            if (LandOrders == null)
                return NotFound($"there is no avaliable land orders for this id {id}");

            var isValidBuyer = await _buyerFarmerServices.IsValidBuyerFarmer(dto.BuyerFarmerId);
            if (!isValidBuyer)
                return BadRequest($"there is no valid buyer for this id {dto.BuyerFarmerId}");

            var isValidFarmer = await _farmerServices.IsValidFarmer(dto.FarmerId);
            if (!isValidFarmer)
                return BadRequest($"there is no valid farmer for this id {dto.FarmerId}");

            LandOrders.FarmerId = dto.FarmerId;
            LandOrders.BuyerFarmerId = dto.BuyerFarmerId;
            LandOrders.LandId = dto.LandId;
            LandOrders.FarmerLandOrderId = id;
            LandOrders.LandRentStatus = dto.LandRentStatus;
            LandOrders.LandSize = dto.LandSize;
            LandOrders.OrderEndDate = dto.OrderEndDate;
            LandOrders.OrderPrice = dto.OrderPrice;
            LandOrders.OrderStartDate = dto.OrderStartDate;



           var result = _FarmerLandOrder.UpdateFarmerLandOrder(LandOrders);
            return Ok(result);
        }


        [HttpDelete("{id}",Name = "DeleteFarmerLandOrder")]
        public async Task<IActionResult> DeleteFarmerLandOrder([FromRoute] int id)
        {
            var LandOrders = await _FarmerLandOrder.GetFarmerLandOrderById(id);
            if (LandOrders == null)
                return NotFound($"there is no avaliable land orders for this id {id}");

           var result = _FarmerLandOrder.DeleteFarmerLandOrder(LandOrders);
            return Ok(result);
        }


        [HttpGet("{FarmerId}", Name ="GetFarmerOrdersByFarmerId")]
        public async Task<IActionResult> GetFarmerOrdersByFarmerId([FromRoute]int FarmerId)
        {
            var records = await _FarmerLandOrder.GetFarmerOrdersByFarmerId(FarmerId);
            if (records == null)
                return NotFound($"there was no records for this farmer id {FarmerId}");

            var farmerOrders = new List<FarmerLandOrderDetailsDto>();
            foreach (var record in records)
            {
                var buyer = await _buyerFarmerServices.GetBuyerFarmerById(record.BuyerFarmerId);
                var order = new FarmerLandOrderDetailsDto()
                {
                    Id = record.FarmerLandOrderId,
                    BuyerFarmerId = record.BuyerFarmerId,
                    FarmerId = record.FarmerId,
                    BuyerFarmerName = buyer.FarmerName,
                    FarmerName = record.Farmer.FarmerName,
                    OrderPrice = record.OrderPrice,
                    LandSize = record.LandSize,
                    OrderStartDate = record.OrderStartDate,
                    OrderEndDate = record.OrderEndDate,
                    LandRentStatus = record.LandRentStatus,
                    LandId = record.LandId,
                    OwnerPhone = record.Farmer.FarmerPhone
                };
                farmerOrders.Add(order);
            }
            return Ok(farmerOrders);
        }


        [HttpGet("{BuyerId}", Name = "GetFarmerOrdersByBuyerId")]
        public async Task<IActionResult> GetFarmerOrdersByBuyerId([FromRoute]int BuyerId)
        {
            var records = await _FarmerLandOrder.GetFarmerOrdersByBuyerId(BuyerId);
            if (records == null)
                return NotFound($"there was no records for this buyer id {BuyerId}");

            var companyOrders = new List<FarmerLandOrderDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _farmerServices.GetFarmerById(record.FarmerId);
                var order = new FarmerLandOrderDetailsDto()
                {
                    Id = record.FarmerLandOrderId,
                    BuyerFarmerId = record.BuyerFarmerId,
                    FarmerId = record.FarmerId,
                    BuyerFarmerName = record.BuyerFarmer.FarmerName,
                    FarmerName = farmers.FarmerName,
                    OrderPrice = record.OrderPrice,
                    LandSize = record.LandSize,
                    OrderStartDate = record.OrderStartDate,
                    OrderEndDate = record.OrderEndDate,
                    LandRentStatus = record.LandRentStatus,
                    LandId = record.LandId,
                    OwnerPhone = farmers.FarmerPhone
                };
                companyOrders.Add(order);
            }
            return Ok(companyOrders);
        }


        [HttpGet("{BuyerName}", Name ="GetFarmerOrdersByBuyerName")]
        public async Task<IActionResult> GetFarmerOrdersByBuyerName([FromRoute] string BuyerName)
        {
            var records = await _FarmerLandOrder.GetFarmerOrdersByBuyerName(BuyerName);
            if (records == null)
                return NotFound($"there was no records for this buyer name {BuyerName}");

            var companyOrders = new List<FarmerLandOrderDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _farmerServices.GetFarmerById(record.FarmerId);
                var order = new FarmerLandOrderDetailsDto()
                {
                    Id = record.FarmerLandOrderId,
                    BuyerFarmerId = record.BuyerFarmerId,
                    FarmerId = record.FarmerId,
                    BuyerFarmerName = record.BuyerFarmer.FarmerName,
                    FarmerName = farmers.FarmerName,
                    OrderPrice = record.OrderPrice,
                    LandSize = record.LandSize,
                    OrderStartDate = record.OrderStartDate,
                    OrderEndDate = record.OrderEndDate,
                    LandRentStatus  = record.LandRentStatus,
                    LandId = record.LandId,
                    OwnerPhone = farmers.FarmerPhone
                };
                companyOrders.Add(order);
            }
            return Ok(companyOrders);
        }


        [HttpGet("{FarmerName}", Name ="GetFarmerOrdersByFarmerName")]
        public async Task<IActionResult> GetFarmerOrdersByFarmerName([FromRoute]string FarmerName)
        {
            var records = await _FarmerLandOrder.GetFarmerOrdersByFarmerName(FarmerName);
            if (records == null)
                return NotFound($"there was no records for this farmer name {FarmerName}");

            var farmerOrders = new List<FarmerLandOrderDetailsDto>();
            foreach (var record in records)
            {
                var buyer = await _buyerFarmerServices.GetBuyerFarmerById(record.BuyerFarmerId);
                var order = new FarmerLandOrderDetailsDto()
                {
                    Id = record.FarmerLandOrderId,
                    BuyerFarmerId = record.BuyerFarmerId,
                    FarmerId = record.FarmerId,
                    BuyerFarmerName = buyer.FarmerName,
                    FarmerName = record.Farmer.FarmerName,
                    OrderPrice = record.OrderPrice,
                    LandSize = record.LandSize,
                    OrderStartDate = record.OrderStartDate,
                    OrderEndDate = record.OrderEndDate,
                    LandRentStatus = record.LandRentStatus,
                    LandId = record.LandId,
                    OwnerPhone = record.Farmer.FarmerPhone
                };
                farmerOrders.Add(order);
            }
            return Ok(farmerOrders);
        }

    }
}
