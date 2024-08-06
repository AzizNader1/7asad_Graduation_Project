using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduationApi.Services
{
    public class ProductOrderServices : IProductOrderServices
    {

        private readonly ApplicationDbContext _context;

        public ProductOrderServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddProductOrder(ProductOrder productOrder)
        {
            await _context.ProductOrders.AddAsync(productOrder);
            _context.SaveChanges();
            return "A new Product Order added successfully";
        }

        public string DeleteProductOrder(ProductOrder productOrder)
        {
            _context.ProductOrders.Remove(productOrder);
            _context.SaveChanges();
            return "An existing Product Order deleted successfully";
        }

        public async Task<IEnumerable<ProductOrder>> GetAllProductOrders()
        {
            return await _context.ProductOrders.ToListAsync();
        }

        public async Task<ProductOrder> GetProductOrderById(int id)
        {
            return await _context.ProductOrders.SingleOrDefaultAsync(f => f.ProductOrderId == id);
        }

        public string UpdateProductOrder(ProductOrder productOrder)
        {
            _context.ProductOrders.Update(productOrder);
            _context.SaveChanges();
            return "An existing Product Order updated successfully";
        }

        public async Task<IEnumerable<ProductOrder>> GetProductsByCompanyId(int companyId)
        {
            return await _context.ProductOrders
                .Include(c => c.Company)
                .Where(c => c.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductOrder>> GetProductsByCompanyName(string companyName)
        {
            return await _context.ProductOrders
                .Include(c => c.Company)
                .Where(c => c.Company.CompanyName == companyName)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductOrder>> GetProductsByFarmerName(string farmerName)
        {
            return await _context.ProductOrders
               .Include(c => c.Farmer)
               .Where(c => c.Farmer.FarmerName == farmerName)
               .ToListAsync();
        }

        public async Task<IEnumerable<ProductOrder>> GetProductsByFarmerId(int farmerId)
        {
            return await _context.ProductOrders
                .Include(c => c.Farmer)
                .Where(c => c.FarmerId == farmerId)
                .ToListAsync();
        }

    }
}

