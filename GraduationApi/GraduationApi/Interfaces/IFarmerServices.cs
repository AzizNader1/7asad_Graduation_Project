using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IFarmerServices
    {
        Task<IEnumerable<Farmer>> GetAllFarmers();

        Task<List<FarmerImageDto>> GetFarmersWithImages();

        Task<Farmer> GetFarmerById(int id);

        Task<FarmerImageDto> GetFarmerWithImage(int id);

        Task<Farmer> GetFarmerByName(string farmerName);

        Task<string> AddFarmer(Farmer farmer);

        Task<bool> IsValidFarmer(int id);

        string UpdateFarmer(Farmer farmer);

        string DeleteFarmer(Farmer farmer);

    }
}
