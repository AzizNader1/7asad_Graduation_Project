using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _ProductServices;
        private readonly IFarmerServices _farmerServices;

        public ProductsController(IProductServices ProductServices, IFarmerServices farmerServices)
        {
            _ProductServices = ProductServices;
            _farmerServices = farmerServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var Products = await _ProductServices.GetAllProducts();
            if (Products == null)
                return NotFound("there is no Products avaliable");

            return Ok(Products);
        }


        [HttpGet("{id}",Name ="GetProductById")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var Product = await _ProductServices.GetProductById(id);
            if (Product == null)
                return NotFound($"there is no avaliable Products for this id {id}");

            return Ok(Product);
        }


        [HttpDelete("{id}",Name = "DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var Product = await _ProductServices.GetProductById(id);
            if (Product == null)
                return NotFound($"there is no avaliable Products for this {id}");

          var result =  _ProductServices.DeleteProduct(Product);
            return Ok(result);
        }


        [HttpPut("{id}",Name = "UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductDto ProductDto)
        {
            var Product = await _ProductServices.GetProductById(id);
            if (Product == null)
                return NotFound($"there is no Products for this id {id}");

            var isValidFarmer = await _farmerServices.IsValidFarmer(ProductDto.FarmerId);
            if (!isValidFarmer)
                return NotFound($"there is no valid farmers for this id {ProductDto.FarmerId}");

            Product.ProductName = ProductDto.ProductName;
            Product.ProductPrice = ProductDto.ProductPrice;
            Product.ProductQuality = ProductDto.ProductQuality;
            Product.ProductWeight = ProductDto.ProductWeight;
            Product.FarmerId = ProductDto.FarmerId;
            Product.ProductDescribtion = ProductDto.ProductDescribtion;

           var result = _ProductServices.UpdateProduct(Product);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto ProductDto)
        {

            var product = new Product()
            {
                ProductName = ProductDto.ProductName,
                ProductWeight = ProductDto.ProductWeight,
                ProductQuality = ProductDto.ProductQuality,
                ProductPrice = ProductDto.ProductPrice,
                FarmerId = ProductDto.FarmerId,
                ProductDescribtion = ProductDto.ProductDescribtion
            };

           var result = _ProductServices.AddProduct(product);
            return Ok(result);
        }


        [HttpGet("{id}", Name = "GetProductByFarmerId")]
        public async Task<IActionResult> GetProductsByFarmerId([FromRoute] int id)
        {
            var records = await _ProductServices.GetProductsByFarmerId(id);
            if (records == null)
                return NotFound($"there was no Products for this farmer id {id}");

            var farmerProducts = new List<ProductDetailsDto>();
            foreach (var record in records)
            {
                var order = new ProductDetailsDto()
                {

                    Id = record.ProductId,
                    FarmerId = record.FarmerId,
                    FarmerName = record.Farmer.FarmerName,
                    ProductName = record.ProductName,
                    ProductWeight = record.ProductWeight,
                    ProductQuality = record.ProductQuality,
                    ProductPrice = record.ProductPrice,
                    ProductDescribtion = record.ProductDescribtion,
                };
                farmerProducts.Add(order);
            }
            return Ok(farmerProducts);
        }


        [HttpGet("{name}", Name = "GetProductByFarmerName")]
        public async Task<IActionResult> GetProductsByFarmerName([FromRoute] string name)
        {
            var records = await _ProductServices.GetProductsByFarmerName(name);
            if (records == null)
                return NotFound($"there was no Products for this farmer name {name}");

            var farmerProducts = new List<ProductDetailsDto>();
            foreach (var record in records)
            {
                var order = new ProductDetailsDto()
                {

                Id = record.ProductId,
                FarmerId = record.FarmerId,
                FarmerName = record.Farmer.FarmerName,
                ProductName = record.ProductName,
                ProductWeight = record.ProductWeight,
                ProductQuality = record.ProductQuality,
                ProductPrice = record.ProductPrice,
                ProductDescribtion = record.ProductDescribtion,
                };
                farmerProducts.Add(order);
            }
            return Ok(farmerProducts);
        }


        [HttpGet("{ProductName}", Name = "GetProductByName")]
        public async Task<IActionResult> GetProductByName([FromRoute] string ProductName)
        {
            var Product = await _ProductServices.GetProductByName(ProductName);
            if (Product == null)
                return NotFound($"there is no avaliable Products for this name :- {ProductName}");

            return Ok(Product);
        }


    }
}
