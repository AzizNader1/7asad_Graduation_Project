using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IBuyerFarmerServices
    {
        Task<IEnumerable<BuyerFarmer>> GetAllBuyerFarmers();

        Task<List<BuyerFarmerImageDto>> GetBuyerFarmersWithImages();

        Task<BuyerFarmer> GetBuyerFarmerById(int id);

        Task<BuyerFarmerImageDto> GetBuyerFarmerWithImage(int id);

        Task<BuyerFarmer> GetBuyerFarmerByName(string farmerName);

        Task<string> AddBuyerFarmer(BuyerFarmer farmer);

        Task<bool> IsValidBuyerFarmer(int id);

        string UpdateBuyerFarmer(BuyerFarmer buyerFarmer);

        string DeleteBuyerFarmer(BuyerFarmer buyerFarmer);

    }
}
