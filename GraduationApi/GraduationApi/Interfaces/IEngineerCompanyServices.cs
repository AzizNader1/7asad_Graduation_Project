using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IEngineerCompanyServices
    {
        Task<IEnumerable<EngineerCompany>> GetAllEngineerCompanies();

        Task<EngineerCompany> GetEngineerCompanyById(int id);

        Task<string> AddEngineerCompany(EngineerCompany EngineerCompany);

        string UpdateEngineerCompany(EngineerCompany EngineerCompany);

        string DeleteEngineerCompany(EngineerCompany EngineerCompany);

        Task<IEnumerable<EngineerCompany>> GetEngineerCompanyByEngineerId(int engineerId);

        Task<IEnumerable<EngineerCompany>> GetEngineerCompanyByCompanyId(int companyId);

        Task<IEnumerable<EngineerCompany>> GetEngineerCompanyByCompanyName(string companyName);

        Task<IEnumerable<EngineerCompany>> GetEngineerCompanyByEngineerName(string engineerName);
        
    }
}
