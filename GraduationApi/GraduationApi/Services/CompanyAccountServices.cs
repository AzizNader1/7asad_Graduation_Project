using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace GraduationApi.Services
{
    public class CompanyAccountServices : ICompanyAccountServices
    {

        private readonly ApplicationDbContext _context;

        public CompanyAccountServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddCompanyAccount(CompanyAccount companyAccount)
        {
            await _context.CompanyAccounts.AddAsync(companyAccount);
            _context.SaveChanges();
            return "a new company account added successfully";
        }

        public string DeleteCompanyAccount(CompanyAccount companyAccount)
        {
            _context.CompanyAccounts.Remove(companyAccount);
            _context.SaveChanges();
            return "An existing company account deleted successfully";
        }

        public async Task<IEnumerable<CompanyAccount>> GetAllCompanyAccounts()
        {
            return await _context.CompanyAccounts.ToListAsync();
        }

        public async Task<CompanyAccount> GetCompanyAccountById(int id)
        {
            return await _context.CompanyAccounts.FirstOrDefaultAsync(b => b.CompanyAccountId == id);
        }

        public async Task<IEnumerable<CompanyAccount>> GetCompanyAccountsByBankId(int id)
        {
            return await _context.CompanyAccounts
            .Include(c => c.Bank)
            .Where(c => c.BankId == id)
            .ToListAsync();
        }

        public async Task<IEnumerable<CompanyAccount>> GetCompanyAccountsByBankName(string bankName)
        {
            return await _context.CompanyAccounts
            .Include(c => c.Bank)
            .Where(c => c.Bank.BankName == bankName)
            .ToListAsync();
        }

        public async Task<IEnumerable<CompanyAccount>> GetCompanyAccountsByCompanyName(string companyName)
        {
            return await _context.CompanyAccounts
            .Include(c => c.Company)
            .Where(c => c.Company.CompanyName == companyName)
            .ToListAsync();
        }

        public async Task<IEnumerable<CompanyAccount>> GetCompanyAccountsByCompanyId(int id)
        {
            return await _context.CompanyAccounts
            .Include(c => c.Company)
            .Where(c => c.CompanyId == id)
            .ToListAsync();
        }

        public string UpdateCompanyAccount(CompanyAccount companyAccount)
        {
            _context.CompanyAccounts.Update(companyAccount);
            _context.SaveChanges();
            return "An existing company account updated successfully";
        }

    }
}
