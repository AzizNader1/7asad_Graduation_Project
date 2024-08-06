using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface ILandOrderServices
    {
        Task<IEnumerable<LandOrder>> GetAllLandOrders();

        Task<LandOrder> GetLandOrderById(int id);

        Task<string> AddLandOrder(LandOrder landOrder);

        string UpdateLandOrder(LandOrder landOrder);

        string DeleteLandOrder(LandOrder landOrder);

        Task<IEnumerable<LandOrder>> GetOrdersByFarmerId(int farmerId);

        Task<IEnumerable<LandOrder>> GetOrdersByCompanyId(int companyId);

        Task<IEnumerable<LandOrder>> GetOrdersByCompanyName(string companyName);

        Task<IEnumerable<LandOrder>> GetOrdersByFarmerName(string farmerName);


    }
}
