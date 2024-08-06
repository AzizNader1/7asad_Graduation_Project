using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduationApi.Services
{
    public class CompanyServices : ICompanyServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileServices _fileServices;

        public CompanyServices(ApplicationDbContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<string> AddCompany(Company compnay)
        {
            await _context.Companies.AddAsync(compnay);
            _context.SaveChanges();
            return "a new company added successfully";
        }

        public string DeleteCompany(Company compnay)
        {
            _context.Companies.Remove(compnay);
            _context.SaveChanges();
            return "An existing company deleted successfully";
        }

        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            return await _context.Companies.OrderBy(b => b.CompanyName).ToListAsync();
        }

        public async Task<Company> GetCompanyById(int id)
        {
            return await _context.Companies.FirstOrDefaultAsync(b => b.CompanyId == id);
        }

        public async Task<Company> GetCompanyByName(string companyName)
        {
            return await _context.Companies.FirstOrDefaultAsync(b => b.CompanyName == companyName);
        }

        public string UpdateCompany(Company compnay)
        {
            _context.Companies.Update(compnay);
            _context.SaveChanges();
            return "An existing company updated successfully";
        }

        public async Task<bool> IsValidCompany(int id)
        {
            return await _context.Companies.AnyAsync(g => g.CompanyId == id);
        }

        public async Task<List<CompanyImageDto>> GetCompaniesWithImages()
        {
            var companies = await GetAllCompanies();

            var companiesViewModels = new List<CompanyImageDto>();

            foreach (var company in companies)
            {
                var latestFiles = await _fileServices.GetLatestFileNames("company", company.CompanyId);

                var companyViewModel = new CompanyImageDto
                {
                    Company = company,
                    CompanyImageUrl = latestFiles.latestImageFileName
                };

                companiesViewModels.Add(companyViewModel);
            }

            return companiesViewModels;
        }

        public async Task<CompanyImageDto> GetCompanyWithImage(int id)
        {
            var company = await GetCompanyById(id);

            var latestFiles = await _fileServices.GetLatestFileNames("company", company.CompanyId);
            var companyViewModel = new CompanyImageDto()
            {
                Company = company,
                CompanyImageUrl = latestFiles.latestImageFileName
            };
            return companyViewModel;
        }
    }
}
