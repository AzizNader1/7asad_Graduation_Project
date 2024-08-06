using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface ICompanyAccountServices
    {
        Task<IEnumerable<CompanyAccount>> GetAllCompanyAccounts();

        Task<CompanyAccount> GetCompanyAccountById(int id);

        Task<IEnumerable<CompanyAccount>> GetCompanyAccountsByBankId(int id);

        Task<IEnumerable<CompanyAccount>> GetCompanyAccountsByCompanyId(int id);

        Task<IEnumerable<CompanyAccount>> GetCompanyAccountsByBankName(string bankName);

        Task<IEnumerable<CompanyAccount>> GetCompanyAccountsByCompanyName(string companyName);

        Task<string> AddCompanyAccount(CompanyAccount companyAccount);

        string UpdateCompanyAccount(CompanyAccount companyAccount);

        string DeleteCompanyAccount(CompanyAccount companyAccount);

    }
}
