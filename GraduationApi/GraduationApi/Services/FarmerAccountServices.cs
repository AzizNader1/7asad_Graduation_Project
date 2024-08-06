using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace GraduationApi.Services
{
    public class FarmerAccountServices : IFarmerAccountServices
    {

        private readonly ApplicationDbContext _context;

        public FarmerAccountServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddFarmerAccount(FarmerAccount farmerAccount)
        {
            await _context.FarmerAccounts.AddAsync(farmerAccount);
            _context.SaveChanges();
            return "a new farmer account added successfully";
        }

        public string DeleteFarmerAccount(FarmerAccount farmerAccount)
        {
            _context.FarmerAccounts.Remove(farmerAccount);
            _context.SaveChanges();
            return "An existing farmer account deleted successfully";
        }

        public async Task<IEnumerable<FarmerAccount>> GetAllFarmerAccounts()
        {
            return await _context.FarmerAccounts.ToListAsync();
        }

        public async Task<FarmerAccount> GetFarmerAccountById(int id)
        {
            return await _context.FarmerAccounts.FirstOrDefaultAsync(b => b.FarmerAccountId == id);
        }

        public async Task<IEnumerable<FarmerAccount>> GetFarmerAccountsByBankId(int id)
        {
            return await _context.FarmerAccounts
            .Include(c => c.Bank)
            .Where(c => c.BankId == id)
             .ToListAsync();
        }

        public async Task<IEnumerable<FarmerAccount>> GetFarmerAccountsByBankName(string bankName)
        {
            return await _context.FarmerAccounts
            .Include(c => c.Bank)
            .Where(c => c.Bank.BankName == bankName)
            .ToListAsync();
        }

        public async Task<IEnumerable<FarmerAccount>> GetFarmerAccountsByFarmerName(string farmerName)
        {
            return await _context.FarmerAccounts
            .Include(c => c.Farmer)
            .Where(c => c.Farmer.FarmerName == farmerName)
            .ToListAsync();
        }

        public async Task<IEnumerable<FarmerAccount>> GetFarmerAccountsByFarmerId(int id)
        {
            return await _context.FarmerAccounts
            .Include(c => c.Farmer)
            .Where(c => c.FarmerId == id)
            .ToListAsync();
        }

        public string UpdateFarmerAccount(FarmerAccount farmerAccount)
        {
            _context.FarmerAccounts.Update(farmerAccount);
            _context.SaveChanges();
            return "An existing farmer account updated successfully";
        }

    }
}
