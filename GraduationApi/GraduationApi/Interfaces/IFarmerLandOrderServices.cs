using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IFarmerLandOrderServices
    {
        Task<IEnumerable<FarmerLandOrder>> GetAllFarmerLandOrders();

        Task<FarmerLandOrder> GetFarmerLandOrderById(int id);

        Task<string> AddFarmerLandOrder(FarmerLandOrder farmerLandOrder);

        string UpdateFarmerLandOrder(FarmerLandOrder farmerLandOrder);

        string DeleteFarmerLandOrder(FarmerLandOrder farmerLandOrder);

        Task<IEnumerable<FarmerLandOrder>> GetFarmerOrdersByFarmerId(int farmerId);

        Task<IEnumerable<FarmerLandOrder>> GetFarmerOrdersByBuyerId(int buyerId);

        Task<IEnumerable<FarmerLandOrder>> GetFarmerOrdersByBuyerName(string buyerName);

        Task<IEnumerable<FarmerLandOrder>> GetFarmerOrdersByFarmerName(string farmerName);


    }
}
