using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace GraduationApi.Services
{
    public class LandOrderServices : ILandOrderServices
    {

        private readonly ApplicationDbContext _context;

        public LandOrderServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddLandOrder(LandOrder landOrder)
        {
            await _context.LandOrders.AddAsync(landOrder);
            _context.SaveChanges();
            return "A new Land Order added successfully";
        }

        public string DeleteLandOrder(LandOrder landOrder)
        {
            _context.LandOrders.Remove(landOrder);
            _context.SaveChanges();
            return "An existing Land Order deleted successfully";
        }

        public async Task<IEnumerable<LandOrder>> GetAllLandOrders()
        {
            return await _context.LandOrders.ToListAsync();
        }

        public async Task<LandOrder> GetLandOrderById(int id)
        {
            return await _context.LandOrders.SingleOrDefaultAsync(f => f.LandOrderId == id);
        }

        public async Task<IEnumerable<LandOrder>> GetOrdersByCompanyId(int companyId)
        {
            return await _context.LandOrders
                .Include(c => c.Company)
                .Where(c => c.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<LandOrder>> GetOrdersByCompanyName(string companyName)
        {
            return await _context.LandOrders
                .Include(c => c.Company)
                .Where(c => c.Company.CompanyName == companyName)
                .ToListAsync();
        }

        public async Task<IEnumerable<LandOrder>> GetOrdersByFarmerName(string farmerName)
        {
            return await _context.LandOrders
               .Include(c => c.Farmer)
               .Where(c => c.Farmer.FarmerName == farmerName)
               .ToListAsync();
        }

        public async Task<IEnumerable<LandOrder>> GetOrdersByFarmerId(int farmerId)
        {
            return await _context.LandOrders
                .Include(c => c.Farmer)
                .Where(c => c.FarmerId == farmerId)
                .ToListAsync();
        }

        public string UpdateLandOrder(LandOrder landOrder)
        {
            _context.LandOrders.Update(landOrder);
            _context.SaveChanges();
            return "An existing Land Order updated successfully";
        }

    }
}