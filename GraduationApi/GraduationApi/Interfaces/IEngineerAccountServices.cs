using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IEngineerAccountServices
    {
        Task<IEnumerable<EngineerAccount>> GetAllEngineerAccounts();

        Task<EngineerAccount> GetEngineerAccountById(int id);

        Task<IEnumerable<EngineerAccount>> GetEngineerAccountsByBankId(int id);

        Task<IEnumerable<EngineerAccount>> GetEngineerAccountsByEngineerId(int id);

        Task<IEnumerable<EngineerAccount>> GetEngineerAccountsByBankName(string bankName);

        Task<IEnumerable<EngineerAccount>> GetEngineerAccountsByEngineerName(string engineerName);

        Task<string> AddEngineerAccount(EngineerAccount EngineerAccount);

        string UpdateEngineerAccount(EngineerAccount EngineerAccount);

        string DeleteEngineerAccount(EngineerAccount EngineerAccount);

    }
}
