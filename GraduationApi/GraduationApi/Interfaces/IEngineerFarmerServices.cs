using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IEngineerFarmerServices
    {
        Task<IEnumerable<EngineerFarmer>> GetAllEngineerFarmers();

        Task<EngineerFarmer> GetEngineerFarmerById(int id);

        Task<string> AddEngineerFarmer(EngineerFarmer EngineerFarmer);

        string UpdateEngineerFarmer(EngineerFarmer EngineerFarmer);

        string DeleteEngineerFarmer(EngineerFarmer EngineerFarmer);

        Task<IEnumerable<EngineerFarmer>> GetEngineerFarmerByFarmerId(int farmerId);

        Task<IEnumerable<EngineerFarmer>> GetEngineerFarmerByEngineerId(int engineerId);

        Task<IEnumerable<EngineerFarmer>> GetEngineerFarmerByEngineerName(string engineerName);

        Task<IEnumerable<EngineerFarmer>> GetEngineerFarmerByFarmerName(string farmerName);

    }
}
