using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduationApi.Services
{
    public class EngineerCompanyServices : IEngineerCompanyServices
    {

        private readonly ApplicationDbContext _context;

        public EngineerCompanyServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddEngineerCompany(EngineerCompany EngineerCompany)
        {
            await _context.EngineerCompanies.AddAsync(EngineerCompany);
            _context.SaveChanges();
            return "a new service deal added successfully";
        }

        public string DeleteEngineerCompany(EngineerCompany EngineerCompany)
        {
            _context.EngineerCompanies.Remove(EngineerCompany);
            _context.SaveChanges();
            return "An existing service deal deleted successfully";
        }

        public  async Task<IEnumerable<EngineerCompany>> GetAllEngineerCompanies()
        {
            return await _context.EngineerCompanies.ToListAsync();
        }

        public async  Task<EngineerCompany> GetEngineerCompanyById(int id)
        {
            return await _context.EngineerCompanies.FirstOrDefaultAsync(b => b.EngineerCompanyId == id);
        }

        public async Task<IEnumerable<EngineerCompany>> GetEngineerCompanyByCompanyId(int companyId)
        {
            return await _context.EngineerCompanies
            .Include(c => c.Company)
            .Where(c => c.CompanyId == companyId)
            .ToListAsync();
        }

        public async  Task<IEnumerable<EngineerCompany>> GetEngineerCompanyByCompanyName(string companyName)
        {
            return await _context.EngineerCompanies
            .Include(c => c.Company)
            .Where(c => c.Company.CompanyName == companyName)
            .ToListAsync();
        }

        public async  Task<IEnumerable<EngineerCompany>> GetEngineerCompanyByEngineerId(int engineerId)
        {
            return await _context.EngineerCompanies
            .Include(c => c.Engineer)
            .Where(c => c.EngineerId == engineerId)
            .ToListAsync();
        }

        public async  Task<IEnumerable<EngineerCompany>> GetEngineerCompanyByEngineerName(string engineerName)
        {
            return await _context.EngineerCompanies
            .Include(c => c.Engineer)
            .Where(c => c.Engineer.EngineerName == engineerName)
            .ToListAsync();
        }

        public string UpdateEngineerCompany(EngineerCompany EngineerCompany)
        {
            _context.EngineerCompanies.Update(EngineerCompany);
            _context.SaveChanges();
            return "An existing service deal updated successfully";
        }

    }
}
