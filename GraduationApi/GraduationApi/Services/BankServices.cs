using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduationApi.Services
{
    public class BankServices : IBankServices
    {

        private readonly ApplicationDbContext _context;

        public BankServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddBank(Bank bank)
        {
            await _context.Banks.AddAsync(bank);
            _context.SaveChanges();
            return "a new bank added successfully";
        }

        public string DeleteBank(Bank bank)
        {
            _context.Banks.Remove(bank);
            _context.SaveChanges();
            return "An existing bank deleted successfully";
        }

        public async Task<IEnumerable<Bank>> GetAllBanks()
        {
            return await _context.Banks.OrderBy(b => b.BankName).ToListAsync();
        }

        public async Task<Bank> GetBankById(int id)
        {
            return await _context.Banks.FirstOrDefaultAsync(b => b.BankId == id);
        }

        public async Task<Bank> GetBankByName(string bankName)
        {
            return await _context.Banks.FirstOrDefaultAsync(b => b.BankName == bankName);
        }

        public string UpdateBank(Bank bank)
        {
            _context.Banks.Update(bank);
            _context.SaveChanges();
            return "An existing bank updated successfully";
        }

        public async Task<bool> IsValidBank(int id)
        {
            return await _context.Banks.AnyAsync(g => g.BankId == id);
        }

    }
}
