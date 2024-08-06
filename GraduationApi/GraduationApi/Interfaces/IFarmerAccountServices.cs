using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IFarmerAccountServices
    {
        Task<IEnumerable<FarmerAccount>> GetAllFarmerAccounts();

        Task<FarmerAccount> GetFarmerAccountById(int id);

        Task<IEnumerable<FarmerAccount>> GetFarmerAccountsByBankId(int id);

        Task<IEnumerable<FarmerAccount>> GetFarmerAccountsByFarmerId(int id);

        Task<IEnumerable<FarmerAccount>> GetFarmerAccountsByBankName(string bankName);

        Task<IEnumerable<FarmerAccount>> GetFarmerAccountsByFarmerName(string farmerName);

        Task<string> AddFarmerAccount(FarmerAccount farmerAccount);

        string UpdateFarmerAccount(FarmerAccount farmerAccount);

        string DeleteFarmerAccount(FarmerAccount farmerAccount);

    }
}
