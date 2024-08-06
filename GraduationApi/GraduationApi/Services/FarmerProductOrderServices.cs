using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduationApi.Services
{
    public class FarmerProductOrderServices : IFarmerProductOrderServices
    {

        private readonly ApplicationDbContext _context;

        public FarmerProductOrderServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddFarmerProductOrder(FarmerProductOrder productOrder)
        {
            await _context.FarmerProductOrders.AddAsync(productOrder);
            _context.SaveChanges();
            return "A new Product Order added successfully";
        }

        public string DeleteFarmerProductOrder(FarmerProductOrder productOrder)
        {
            _context.FarmerProductOrders.Remove(productOrder);
            _context.SaveChanges();
            return "An existing Product Order deleted successfully";
        }

        public async Task<IEnumerable<FarmerProductOrder>> GetAllFarmerProductOrders()
        {
            return await _context.FarmerProductOrders.ToListAsync();
        }

        public async Task<FarmerProductOrder> GetFarmerProductOrderById(int id)
        {
            return await _context.FarmerProductOrders.SingleOrDefaultAsync(f => f.FarmerProductOrderId == id);
        }

        public string UpdateFarmerProductOrder(FarmerProductOrder productOrder)
        {
            _context.FarmerProductOrders.Update(productOrder);
            _context.SaveChanges();
            return "An existing Product Order updated successfully";
        }

        public async Task<IEnumerable<FarmerProductOrder>> GetFarmerProductOrdersByBuyerId(int BuyerId)
        {
            return await _context.FarmerProductOrders
                .Include(c => c.BuyerFarmer)
                .Where(c => c.BuyerFarmerId == BuyerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<FarmerProductOrder>> GetFarmerProductOrdersByBuyerName(string BuyerName)
        {
            return await _context.FarmerProductOrders
                .Include(c => c.BuyerFarmer)
                .Where(c => c.BuyerFarmer.FarmerName == BuyerName)
                .ToListAsync();
        }

        public async Task<IEnumerable<FarmerProductOrder>> GetFarmerProductOrdersByFarmerName(string farmerName)
        {
            return await _context.FarmerProductOrders
               .Include(c => c.Farmer)
               .Where(c => c.Farmer.FarmerName == farmerName)
               .ToListAsync();
        }

        public async Task<IEnumerable<FarmerProductOrder>> GetFarmerProductOrdersByFarmerId(int farmerId)
        {
            return await _context.FarmerProductOrders
                .Include(c => c.Farmer)
                .Where(c => c.FarmerId == farmerId)
                .ToListAsync();
        }

    }
}

