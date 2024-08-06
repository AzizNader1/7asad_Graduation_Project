using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace GraduationApi.Services
{
    public class FarmerLandOrderServices : IFarmerLandOrderServices
    {

        private readonly ApplicationDbContext _context;

        public FarmerLandOrderServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddFarmerLandOrder(FarmerLandOrder landOrder)
        {
            await _context.FarmerLandOrders.AddAsync(landOrder);
            _context.SaveChanges();
            return "A new Land Order added successfully";
        }

        public string DeleteFarmerLandOrder(FarmerLandOrder landOrder)
        {
            _context.FarmerLandOrders.Remove(landOrder);
            _context.SaveChanges();
            return "An existing Land Order deleted successfully";
        }

        public async Task<IEnumerable<FarmerLandOrder>> GetAllFarmerLandOrders()
        {
            return await _context.FarmerLandOrders.ToListAsync();
        }

        public async Task<FarmerLandOrder> GetFarmerLandOrderById(int id)
        {
            return await _context.FarmerLandOrders.SingleOrDefaultAsync(f => f.FarmerLandOrderId == id);
        }

        public async Task<IEnumerable<FarmerLandOrder>> GetFarmerOrdersByBuyerId(int BuyerId)
        {
            return await _context.FarmerLandOrders
                .Include(c => c.BuyerFarmer)
                .Where(c => c.BuyerFarmerId == BuyerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<FarmerLandOrder>> GetFarmerOrdersByBuyerName(string BuyerName)
        {
            return await _context.FarmerLandOrders
                .Include(c => c.BuyerFarmer)
                .Where(c => c.BuyerFarmer.FarmerName == BuyerName)
                .ToListAsync();
        }

        public async Task<IEnumerable<FarmerLandOrder>> GetFarmerOrdersByFarmerName(string farmerName)
        {
            return await _context.FarmerLandOrders
               .Include(c => c.Farmer)
               .Where(c => c.Farmer.FarmerName == farmerName)
               .ToListAsync();
        }

        public async Task<IEnumerable<FarmerLandOrder>> GetFarmerOrdersByFarmerId(int farmerId)
        {
            return await _context.FarmerLandOrders
                .Include(c => c.Farmer)
                .Where(c => c.FarmerId == farmerId)
                .ToListAsync();
        }

        public string UpdateFarmerLandOrder(FarmerLandOrder landOrder)
        {
            _context.FarmerLandOrders.Update(landOrder);
            _context.SaveChanges();
            return "An existing Land Order updated successfully";
        }

    }
}