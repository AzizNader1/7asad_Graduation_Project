using GraduationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationApi.Interfaces
{
    public interface IEquipmentServices
    {
        Task<IEnumerable<Equipment>> GetAllEquipments();

        Task<List<EquipmentImageDto>> GetEquipmentsWithImages();

        Task<Equipment> GetEquipmentById(int id);

        Task<EquipmentImageDto> GetEquipmentWithImage(int id);

        Task<IEnumerable<Equipment>> GetEquipmentsByFarmerId(int id);

        Task<IEnumerable<Equipment>> GetEquipmentsByFarmerName(string farmerName);

        Task<string> AddEquipment(Equipment Equipment);

        string UpdateEquipment(Equipment Equipment);

        string DeleteEquipment(Equipment Equipment);

        Task<bool> IsValidEquipment(int id);

    }
}

