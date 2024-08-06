using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduationApi.Services
{
    public class EngineerAccountServices : IEngineerAccountServices
    {

        private readonly ApplicationDbContext _context;

        public EngineerAccountServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddEngineerAccount(EngineerAccount EngineerAccount)
        {
            await _context.EngineerAccounts.AddAsync(EngineerAccount);
            _context.SaveChanges();
            return "a new engineer account added successfully";
        }

        public string DeleteEngineerAccount(EngineerAccount EngineerAccount)
        {
            _context.EngineerAccounts.Remove(EngineerAccount);
            _context.SaveChanges();
            return "An existing engineer account deleted successfully";
        }

        public async Task<IEnumerable<EngineerAccount>> GetAllEngineerAccounts()
        {
            return await _context.EngineerAccounts.ToListAsync();
        }

        public async Task<EngineerAccount> GetEngineerAccountById(int id)
        {
            return await _context.EngineerAccounts.FirstOrDefaultAsync(b => b.EngineerAccountId == id);
        }

        public async Task<IEnumerable<EngineerAccount>> GetEngineerAccountsByBankId(int id)
        {
            return await _context.EngineerAccounts
            .Include(c => c.Bank)
            .Where(c => c.BankId == id)
            .ToListAsync();
        }

        public async Task<IEnumerable<EngineerAccount>> GetEngineerAccountsByBankName(string bankName)
        {
            return await _context.EngineerAccounts
            .Include(c => c.Bank)
            .Where(c => c.Bank.BankName == bankName)
            .ToListAsync();
        }

        public async Task<IEnumerable<EngineerAccount>> GetEngineerAccountsByEngineerId(int id)
        {
            return await _context.EngineerAccounts
            .Include(c => c.Engineer)
            .Where(c => c.EngineerId == id)
            .ToListAsync();
        }

        public async Task<IEnumerable<EngineerAccount>> GetEngineerAccountsByEngineerName(string engineerName)
        {
            return await _context.EngineerAccounts
             .Include(c => c.Engineer)
             .Where(c => c.Engineer.EngineerName == engineerName)
             .ToListAsync();
        }

        public string UpdateEngineerAccount(EngineerAccount EngineerAccount)
        {
            _context.EngineerAccounts.Update(EngineerAccount);
            _context.SaveChanges();
            return "An existing engineer account updated successfully";
        }

    }
}
