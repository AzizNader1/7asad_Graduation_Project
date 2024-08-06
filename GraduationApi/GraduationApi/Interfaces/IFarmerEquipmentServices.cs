using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IFarmerEquipmentServices
    {
        Task<IEnumerable<FarmerEquipment>> GetAllFarmerEquipments();

        Task<FarmerEquipment> GetFarmerEquipmentById(int id);

        Task<string> AddFarmerEquipment(FarmerEquipment FarmerEquipment);

        string UpdateFarmerEquipment(FarmerEquipment FarmerEquipment);

        string DeleteFarmerEquipment(FarmerEquipment FarmerEquipment);

        Task<IEnumerable<FarmerEquipment>> GetFarmerEquipmentsByFarmerId(int farmerId);

        Task<IEnumerable<FarmerEquipment>> GetFarmerEquipmentsByEquipmentId(int EquipmentId);

        Task<IEnumerable<FarmerEquipment>> GetFarmerEquipmentsByEquipmentName(string EquipmentName);

        Task<IEnumerable<FarmerEquipment>> GetFarmerEquipmentsByFarmerName(string farmerName);

        Task<List<FarmerEquipmentImageDto>> GetFarmerEquipmentsWithImages();

    }
}
