using GraduationApi.Interfaces;
using GraduationApi.Models;
using GraduationApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{   
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FarmerProductOrdersController : ControllerBase
    {

        private readonly IFarmerProductOrderServices _FarmerProductOrder;
        private readonly IBuyerFarmerServices _buyerServices;
        private readonly IFarmerServices _farmerServices;

        public FarmerProductOrdersController(IFarmerProductOrderServices farmerProductOrder, IBuyerFarmerServices buyerServices, IFarmerServices farmerServices)
        {
            _FarmerProductOrder = farmerProductOrder;
            _buyerServices = buyerServices;
            _farmerServices = farmerServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllFarmerProductOrders()
        {
            var productOrders = await _FarmerProductOrder.GetAllFarmerProductOrders();
            if (productOrders == null)
                return NotFound("there is no product orders avaliable");

            return Ok(productOrders);
        }
        

        [HttpGet("{id}",Name = "GetFarmerProductOrderById")]
        public async Task<IActionResult> GetFarmerProductOrderById([FromRoute] int id)
        {
            var productOrders = await _FarmerProductOrder.GetFarmerProductOrderById(id);
            if (productOrders == null)
                return NotFound($"there is no product orders avaliable for this id {id}");

            return Ok(productOrders);
        }


        [HttpPost]
        public async Task<IActionResult> AddFarmerProductOrder(FarmerProductOrderDto dto)
        {
            var productOrder = new FarmerProductOrder 
            {
                BuyerFarmerId = dto.BuyerFarmerId,
                FarmerId = dto.FarmerId,
                OrderPrice = dto.OrderPrice,
                OrderWeight = dto.OrderWeight,
                ProductName = dto.ProductName,
                ProductOffersStatus = dto.ProductOffersStatus
            };

          var result =  await _FarmerProductOrder.AddFarmerProductOrder(productOrder);

            return Ok(result);
        }


        [HttpPut("{id}",Name = "UpdateFarmerProductOrder")]
        public async Task<IActionResult> UpdateFarmerProductOrder([FromRoute]int id, [FromBody] FarmerProductOrderDto dto)
        {
           var productOrders = await _FarmerProductOrder.GetFarmerProductOrderById(id);
            if (productOrders == null)
                return NotFound($"there is no avaliable product orders for this id {id}");

            var isValidBuyer = await _buyerServices.IsValidBuyerFarmer(dto.BuyerFarmerId);
            if (!isValidBuyer)
                return BadRequest($"there is no valid buyer for this id {dto.BuyerFarmerId}");

            var isValidFarmer = await _farmerServices.IsValidFarmer(dto.FarmerId);
            if (!isValidFarmer)
                return BadRequest($"there is no valid farmer for this id {dto.FarmerId}");

                productOrders.FarmerProductOrderId = id;
                productOrders.BuyerFarmerId = dto.BuyerFarmerId;
                productOrders.FarmerId = dto.FarmerId;
                productOrders.OrderPrice = dto.OrderPrice;
                productOrders.OrderWeight = dto.OrderWeight;
                productOrders.ProductName = dto.ProductName;
                productOrders.ProductOffersStatus = dto.ProductOffersStatus;


          var result = _FarmerProductOrder.UpdateFarmerProductOrder(productOrders);
            return Ok(result);
        }


        [HttpDelete("{id}",Name = "DeleteFarmerProductOrder")]
        public async Task<IActionResult> DeleteFarmerProductOrder([FromRoute] int id)
        {
            var productOrders = await _FarmerProductOrder.GetFarmerProductOrderById(id);
            if (productOrders == null)
                return NotFound($"there is no avaliable product orders for this id {id}");

           var result = _FarmerProductOrder.DeleteFarmerProductOrder(productOrders);
            return Ok(result);
        }


        [HttpGet("{id}", Name = "GetFarmerProductOrdersByFarmerId")]
        public async Task<IActionResult> GetFarmerProductOrdersByFarmerId([FromRoute] int id)
        {
            var records = await _FarmerProductOrder.GetFarmerProductOrdersByFarmerId(id);
            if (records == null)
                return NotFound($"there was no records for this farmer id {id}");

            var farmerProducts = new List<FarmerProductOrderDetailsDto>();
            foreach (var record in records)
            {
                var buyer = await _buyerServices.GetBuyerFarmerById(record.BuyerFarmerId);
                var order = new FarmerProductOrderDetailsDto()
                {
                    Id = record.FarmerProductOrderId,
                    BuyerFarmerId = record.BuyerFarmerId,
                    FarmerId = record.FarmerId,
                    BuyerFarmerName = buyer.FarmerName,
                    FarmerName = record.Farmer.FarmerName,
                    OrderPrice = record.OrderPrice,
                    OrderWeight = record.OrderWeight,
                    ProductName = record.ProductName,
                    ProductOffersStatus = record.ProductOffersStatus,
                    OwnerPhone = record.Farmer.FarmerPhone
                };
                farmerProducts.Add(order);
            }
            return Ok(farmerProducts);
        }


        [HttpGet("{BuyerId}", Name ="GetFarmerProductOrdersByBuyerId")]
        public async Task<IActionResult> GetFarmerProductOrdersByBuyerId([FromRoute] int BuyerId)
        {
            var records = await _FarmerProductOrder.GetFarmerProductOrdersByBuyerId(BuyerId);
            if (records == null)
                return NotFound($"there was no records for this buyer id {BuyerId}");

            var companyProducts = new List<FarmerProductOrderDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _farmerServices.GetFarmerById(record.FarmerId);
                var order = new FarmerProductOrderDetailsDto()
                {
                    Id = record.FarmerProductOrderId,
                    BuyerFarmerId = record.BuyerFarmerId,
                    FarmerId = record.FarmerId,
                    BuyerFarmerName = record.BuyerFarmer.FarmerName,
                    FarmerName = farmers.FarmerName,
                    OrderPrice = record.OrderPrice,
                    OrderWeight = record.OrderWeight,
                    ProductName = record.ProductName,
                    ProductOffersStatus = record.ProductOffersStatus,
                    OwnerPhone = farmers.FarmerPhone
                };
                companyProducts.Add(order);
            }
            return Ok(companyProducts);
        }


        [HttpGet("{BuyerName}", Name ="GetFarmerProductOrdersByBuyerName")]
        public async Task<IActionResult> GetFarmerProductOrdersByBuyerName([FromRoute] string BuyerName)
        {
            var records = await _FarmerProductOrder.GetFarmerProductOrdersByBuyerName(BuyerName);
            if (records == null)
                return NotFound($"there was no records for this Buyer name {BuyerName}");

            var companyProducts = new List<FarmerProductOrderDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _farmerServices.GetFarmerById(record.FarmerId);
                var order = new FarmerProductOrderDetailsDto()
                {
                    Id = record.FarmerProductOrderId,
                    BuyerFarmerId = record.BuyerFarmerId,
                    FarmerId = record.FarmerId,
                    BuyerFarmerName = record.BuyerFarmer.FarmerName,
                    FarmerName = farmers.FarmerName,
                    OrderPrice = record.OrderPrice,
                    OrderWeight = record.OrderWeight,
                    ProductName = record.ProductName,
                    ProductOffersStatus= record.ProductOffersStatus,
                    OwnerPhone = farmers.FarmerPhone
                    
                };
                companyProducts.Add(order);
            }
            return Ok(companyProducts);
        }


        [HttpGet("{name}", Name = "GetFarmerProductOrdersByFarmerName")]
        public async Task<IActionResult> GetFarmerProductOrdersByFarmerName([FromRoute] string name)
        {
            var records = await _FarmerProductOrder.GetFarmerProductOrdersByFarmerName(name);
            if (records == null)
                return NotFound($"there was no records for this farmer name {name}");

            var farmerProduct = new List<FarmerProductOrderDetailsDto>();
            foreach (var record in records)
            {
                var buyer = await _buyerServices.GetBuyerFarmerById(record.BuyerFarmerId);
                var order = new FarmerProductOrderDetailsDto()
                {
                    Id = record.FarmerProductOrderId,
                    BuyerFarmerId = record.BuyerFarmerId,
                    FarmerId = record.FarmerId,
                    BuyerFarmerName = buyer.FarmerName,
                    FarmerName = record.Farmer.FarmerName,
                    OrderPrice = record.OrderPrice,
                   OrderWeight= record.OrderWeight,
                   ProductName = record.ProductName,
                   ProductOffersStatus = record.ProductOffersStatus,
                   OwnerPhone = record.Farmer.FarmerPhone
                };
                farmerProduct.Add(order);
            }
            return Ok(farmerProduct);
        }
    }
}
