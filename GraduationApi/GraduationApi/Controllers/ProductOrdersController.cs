using GraduationApi.Interfaces;
using GraduationApi.Models;
using GraduationApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{   
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductOrdersController : ControllerBase
    {

        private readonly IProductOrderServices _productOrder;
        private readonly ICompanyServices _companyServices;
        private readonly IFarmerServices _farmerServices;

        public ProductOrdersController(IProductOrderServices productOrder, ICompanyServices companyServices, IFarmerServices farmerServices)
        {
            _productOrder = productOrder;
            _companyServices = companyServices;
            _farmerServices = farmerServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProductOrders()
        {
            var productOrders = await _productOrder.GetAllProductOrders();
            if (productOrders == null)
                return NotFound("there is no product orders avaliable");

            return Ok(productOrders);
        }
        

        [HttpGet("{id}",Name = "GetProductOrderById")]
        public async Task<IActionResult> GetProductOrderById([FromRoute] int id)
        {
            var productOrders = await _productOrder.GetProductOrderById(id);
            if (productOrders == null)
                return NotFound($"there is no product orders avaliable for this id {id}");

            return Ok(productOrders);
        }


        [HttpPost]
        public async Task<IActionResult> AddProductOrder(ProductOrderDto dto)
        {
            var productOrder = new ProductOrder 
            {
                CompanyId = dto.CompanyId,
                FarmerId = dto.FarmerId,
                OrderPrice = dto.OrderPrice,
                OrderWeight = dto.OrderWeight,
                ProductName = dto.ProductName,
                ProductOffersStatus = dto.ProductOffersStatus
            };

          var result =  await _productOrder.AddProductOrder(productOrder);

            return Ok(result);
        }


        [HttpPut("{id}",Name = "UpdateProductOrder")]
        public async Task<IActionResult> UpdateProductOrder([FromRoute]int id, [FromBody] ProductOrderDto dto)
        {
           var productOrders = await _productOrder.GetProductOrderById(id);
            if (productOrders == null)
                return NotFound($"there is no avaliable product orders for this id {id}");

            var isValidCompany = await _companyServices.IsValidCompany(dto.CompanyId);
            if (!isValidCompany)
                return BadRequest($"there is no valid company for this id {dto.CompanyId}");

            var isValidFarmer = await _farmerServices.IsValidFarmer(dto.FarmerId);
            if (!isValidFarmer)
                return BadRequest($"there is no valid farmer for this id {dto.FarmerId}");

            productOrders.ProductOrderId = id;
                productOrders.CompanyId = dto.CompanyId;
                productOrders.FarmerId = dto.FarmerId;
                productOrders.OrderPrice = dto.OrderPrice;
                productOrders.OrderWeight = dto.OrderWeight;
                productOrders.ProductName = dto.ProductName;
                productOrders.ProductOffersStatus = dto.ProductOffersStatus;


          var result =  _productOrder.UpdateProductOrder(productOrders);
            return Ok(result);
        }


        [HttpDelete("{id}",Name = "DeleteProductOrder")]
        public async Task<IActionResult> DeleteProductOrder([FromRoute] int id)
        {
            var productOrders = await _productOrder.GetProductOrderById(id);
            if (productOrders == null)
                return NotFound($"there is no avaliable product orders for this id {id}");

           var result = _productOrder.DeleteProductOrder(productOrders);
            return Ok(result);
        }


        [HttpGet("{id}", Name = "GetProductsByFarmerId")]
        public async Task<IActionResult> GetProductsByFarmerId([FromRoute] int id)
        {
            var records = await _productOrder.GetProductsByFarmerId(id);
            if (records == null)
                return NotFound($"there was no records for this farmer id {id}");

            var farmerProducts = new List<ProductOrderDetailsDto>();
            foreach (var record in records)
            {
                var companies = await _companyServices.GetCompanyById(record.CompanyId);
                var order = new ProductOrderDetailsDto()
                {
                    Id = record.ProductOrderId,
                    CompanyId = record.CompanyId,
                    FarmerId = record.FarmerId,
                    CompanyName = companies.CompanyName,
                    FarmerName = record.Farmer.FarmerName,
                    OrderPrice = record.OrderPrice,
                    OrderWeight = record.OrderWeight,
                    ProductName = record.ProductName,
                    ProductOffersStatus = record.ProductOffersStatus,
                    FarmerPhone = record.Farmer.FarmerPhone
                };
                farmerProducts.Add(order);
            }
            return Ok(farmerProducts);
        }


        [HttpGet("{CompanyId}", Name ="GetProductsByCompanyId")]
        public async Task<IActionResult> GetProductsByCompanyId([FromRoute] int CompanyId)
        {
            var records = await _productOrder.GetProductsByCompanyId(CompanyId);
            if (records == null)
                return NotFound($"there was no records for this company id {CompanyId}");

            var companyProducts = new List<ProductOrderDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _farmerServices.GetFarmerById(record.FarmerId);
                var order = new ProductOrderDetailsDto()
                {
                    Id = record.ProductOrderId,
                    CompanyId = record.CompanyId,
                    FarmerId = record.FarmerId,
                    CompanyName = record.Company.CompanyName,
                    FarmerName = farmers.FarmerName,
                    OrderPrice = record.OrderPrice,
                    OrderWeight = record.OrderWeight,
                    ProductName = record.ProductName,
                    ProductOffersStatus = record.ProductOffersStatus,
                    FarmerPhone = farmers.FarmerPhone
                };
                companyProducts.Add(order);
            }
            return Ok(companyProducts);
        }


        [HttpGet("{CompanyName}", Name ="GetProductsByCompanyName")]
        public async Task<IActionResult> GetProductsByCompanyName([FromRoute] string CompanyName)
        {
            var records = await _productOrder.GetProductsByCompanyName(CompanyName);
            if (records == null)
                return NotFound($"there was no records for this company name {CompanyName}");

            var companyProducts = new List<ProductOrderDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _farmerServices.GetFarmerById(record.FarmerId);
                var order = new ProductOrderDetailsDto()
                {
                    Id = record.ProductOrderId,
                    CompanyId = record.CompanyId,
                    FarmerId = record.FarmerId,
                    CompanyName = record.Company.CompanyName,
                    FarmerName = farmers.FarmerName,
                    OrderPrice = record.OrderPrice,
                    OrderWeight = record.OrderWeight,
                    ProductName = record.ProductName,
                    ProductOffersStatus= record.ProductOffersStatus,
                    FarmerPhone = farmers.FarmerPhone
                    
                };
                companyProducts.Add(order);
            }
            return Ok(companyProducts);
        }


        [HttpGet("{name}", Name = "GetProductsByFarmerName")]
        public async Task<IActionResult> GetProductsByFarmerName([FromRoute] string name)
        {
            var records = await _productOrder.GetProductsByFarmerName(name);
            if (records == null)
                return NotFound($"there was no records for this farmer name {name}");

            var farmerProduct = new List<ProductOrderDetailsDto>();
            foreach (var record in records)
            {
                var companies = await _companyServices.GetCompanyById(record.CompanyId);
                var order = new ProductOrderDetailsDto()
                {
                    Id = record.ProductOrderId,
                    CompanyId = record.CompanyId,
                    FarmerId = record.FarmerId,
                    CompanyName = companies.CompanyName,
                    FarmerName = record.Farmer.FarmerName,
                    OrderPrice = record.OrderPrice,
                   OrderWeight= record.OrderWeight,
                   ProductName = record.ProductName,
                   ProductOffersStatus = record.ProductOffersStatus,
                   FarmerPhone = record.Farmer.FarmerPhone
                };
                farmerProduct.Add(order);
            }
            return Ok(farmerProduct);
        }
    }
}
