using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduationApi.Services
{
    public class EngineerFarmerServices : IEngineerFarmerServices
    {

        private readonly ApplicationDbContext _context;

        public EngineerFarmerServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddEngineerFarmer(EngineerFarmer EngineerFarmer)
        {
            await _context.EngineerFarmers.AddAsync(EngineerFarmer);
            _context.SaveChanges();
            return "a new service deal added successfully";
        }

        public string DeleteEngineerFarmer(EngineerFarmer EngineerFarmer)
        {
            _context.EngineerFarmers.Remove(EngineerFarmer);
            _context.SaveChanges();
            return "An existing service deal deleted successfully";
        }

        public async Task<IEnumerable<EngineerFarmer>> GetAllEngineerFarmers()
        {
            return await _context.EngineerFarmers.ToListAsync();
        }

        public async Task<EngineerFarmer> GetEngineerFarmerById(int id)
        {
            return await _context.EngineerFarmers.FirstOrDefaultAsync(b => b.EngineerFarmerId == id);
        }

        public async Task<IEnumerable<EngineerFarmer>> GetEngineerFarmerByFarmerId(int FarmerId)
        {
            return await _context.EngineerFarmers
            .Include(c => c.Farmer)
            .Where(c => c.FarmerId == FarmerId)
            .ToListAsync();
        }

        public async Task<IEnumerable<EngineerFarmer>> GetEngineerFarmerByFarmerName(string FarmerName)
        {
            return await _context.EngineerFarmers
            .Include(c => c.Farmer)
            .Where(c => c.Farmer.FarmerName == FarmerName)
            .ToListAsync();
        }

        public async Task<IEnumerable<EngineerFarmer>> GetEngineerFarmerByEngineerId(int engineerId)
        {
            return await _context.EngineerFarmers
            .Include(c => c.Engineer)
            .Where(c => c.EngineerId == engineerId)
            .ToListAsync();
        }

        public async Task<IEnumerable<EngineerFarmer>> GetEngineerFarmerByEngineerName(string engineerName)
        {
            return await _context.EngineerFarmers
            .Include(c => c.Engineer)
            .Where(c => c.Engineer.EngineerName == engineerName)
            .ToListAsync();
        }

        public string UpdateEngineerFarmer(EngineerFarmer EngineerFarmer)
        {
            _context.EngineerFarmers.Update(EngineerFarmer);
            _context.SaveChanges();
            return "An existing service deal updated successfully";
        }

    }
}
