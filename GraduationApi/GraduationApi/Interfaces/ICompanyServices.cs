using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface ICompanyServices
    {
        Task<IEnumerable<Company>> GetAllCompanies();

        Task<List<CompanyImageDto>> GetCompaniesWithImages();

        Task<Company> GetCompanyById(int id);

        Task<CompanyImageDto> GetCompanyWithImage(int id);

        Task<Company> GetCompanyByName(string companyName);

        Task<string> AddCompany(Company compnay);

        string UpdateCompany(Company compnay);

        string DeleteCompany(Company compnay);

        Task<bool> IsValidCompany(int id);

    }
}
