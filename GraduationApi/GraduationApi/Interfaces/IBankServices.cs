using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IBankServices
    {
        Task<IEnumerable<Bank>> GetAllBanks();

        Task<Bank> GetBankById(int id);

        Task<Bank> GetBankByName(string bankName);

        Task<string> AddBank(Bank bank);

        Task<bool> IsValidBank(int id);

        string UpdateBank(Bank bank);

        string DeleteBank(Bank bank);

    }
}
