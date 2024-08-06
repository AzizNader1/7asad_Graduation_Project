using GraduationApi.Interfaces;
using GraduationApi.Models;
using GraduationApi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationApi.Services
{
    public class ProductServices : IProductServices
    {

        private readonly ApplicationDbContext _context;
        private readonly IFileServices _fileServices;

        public ProductServices(ApplicationDbContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<string> AddProduct(Product Product)
        {
            await _context.Products.AddAsync(Product);
            _context.SaveChanges();
            return "a new Product added successfully";
        }

        public string DeleteProduct(Product Product)
        {
            _context.Products.Remove(Product);
            _context.SaveChanges();
            return "An existing Product deleted successfully";
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.OrderBy(b => b.ProductName).ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(b => b.ProductId == id);
        }

        public async Task<Product> GetProductByName(string productName)
        {
            return await _context.Products.FirstOrDefaultAsync(b => b.ProductName == productName);
        }

        public async Task<IEnumerable<Product>> GetProductsByFarmerId(int id)
        {
            return await _context.Products
            .Include(c => c.Farmer)
            .Where(c => c.FarmerId == id)
            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByFarmerName(string farmerName)
        {
            return await _context.Products
           .Include(c => c.Farmer)
           .Where(c => c.Farmer.FarmerName == farmerName)
           .ToListAsync();
        }

        public string UpdateProduct(Product Product)
        {
            _context.Products.Update(Product);
            _context.SaveChanges();
            return "An existing Product updated successfully";
        }

        public async Task<bool> IsValidProduct(int id)
        {
            return await _context.Products.AnyAsync(g => g.ProductId == id);
        }

        public async Task<List<ProductImageDto>> GetProductsWithImages()
        {
            var products = await GetAllProducts();

            var productsViewModels = new List<ProductImageDto>();

            foreach (var product in products)
            {
                var latestFiles = await _fileServices.GetLatestFileNames("product", product.ProductId);

                var productViewModel = new ProductImageDto
                {
                    Product = product,
                    ProductImageUrl = latestFiles.latestImageFileName
                };

                productsViewModels.Add(productViewModel);
            }

            return productsViewModels;
        }

        public async Task<ProductImageDto> GetProductWithImage(int id)
        {
            var product = await GetProductById(id);

            var latestFiles = await _fileServices.GetLatestFileNames("product", product.ProductId);
            var productViewModel = new ProductImageDto()
            {
                Product = product,
                ProductImageUrl = latestFiles.latestImageFileName
            };
            return productViewModel;
        }

    }
}
