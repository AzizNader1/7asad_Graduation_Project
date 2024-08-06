using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IFarmerProductOrderServices
    {
        Task<IEnumerable<FarmerProductOrder>> GetAllFarmerProductOrders();

        Task<FarmerProductOrder> GetFarmerProductOrderById(int id);

        Task<string> AddFarmerProductOrder(FarmerProductOrder farmerProductOrder);

        string UpdateFarmerProductOrder(FarmerProductOrder farmerProductOrder);

        string DeleteFarmerProductOrder(FarmerProductOrder farmerProductOrder);

        Task<IEnumerable<FarmerProductOrder>> GetFarmerProductOrdersByFarmerId(int farmerId);

        Task<IEnumerable<FarmerProductOrder>> GetFarmerProductOrdersByBuyerId(int buyerId);

        Task<IEnumerable<FarmerProductOrder>> GetFarmerProductOrdersByBuyerName(string buyerName);

        Task<IEnumerable<FarmerProductOrder>> GetFarmerProductOrdersByFarmerName(string farmerName);

    }
}
