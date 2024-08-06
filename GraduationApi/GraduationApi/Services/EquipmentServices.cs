using GraduationApi.Interfaces;
using GraduationApi.Models;
using GraduationApi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationApi.Services
{
    public class EquipmentServices : IEquipmentServices
    {

        private readonly ApplicationDbContext _context;
        private readonly IFileServices _fileServices;

        public EquipmentServices(ApplicationDbContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<string> AddEquipment(Equipment Equipment)
        {
            await _context.Equipments.AddAsync(Equipment);
            _context.SaveChanges();
            return "a new Equipment added successfully";
        }

        public string DeleteEquipment(Equipment Equipment)
        {
            _context.Equipments.Remove(Equipment);
            _context.SaveChanges();
            return "An existing Equipment deleted successfully";
        }

        public async Task<IEnumerable<Equipment>> GetAllEquipments()
        {
            return await _context.Equipments.ToListAsync();
        }

        public async Task<Equipment> GetEquipmentById(int id)
        {
            return await _context.Equipments.FirstOrDefaultAsync(b => b.EquipmentId == id);
        }

        public async Task<IEnumerable<Equipment>> GetEquipmentsByFarmerId(int id)
        {
            return await _context.Equipments
            .Include(c => c.Farmer)
            .Where(c => c.FarmerId == id)
            .ToListAsync();
        }

        public async Task<IEnumerable<Equipment>> GetEquipmentsByFarmerName(string farmerName)
        {
            return await _context.Equipments
           .Include(c => c.Farmer)
           .Where(c => c.Farmer.FarmerName == farmerName)
           .ToListAsync();
        }

        public string UpdateEquipment(Equipment Equipment)
        {
            _context.Equipments.Update(Equipment);
            _context.SaveChanges();
            return "An existing Equipment updated successfully";
        }
        public async Task<bool> IsValidEquipment(int id)
        {
            return await _context.Equipments.AnyAsync(g => g.EquipmentId == id);
        }

        public async Task<List<EquipmentImageDto>> GetEquipmentsWithImages()
        {
            var equipments = await GetAllEquipments();

            var equipmentViewModels = new List<EquipmentImageDto>();

            foreach (var equipment in equipments)
            {
                var latestFiles = await _fileServices.GetLatestFileNames("equipment", equipment.FarmerId);

                var equipmentViewModel = new EquipmentImageDto
                {
                    Equipment = equipment,
                    EquipmentImageUrl = latestFiles.latestImageFileName
                };

                equipmentViewModels.Add(equipmentViewModel);
            }

            return equipmentViewModels;
        }

        public async Task<EquipmentImageDto> GetEquipmentWithImage(int id)
        {
            var equipment = await GetEquipmentById(id);

            var latestFiles = await _fileServices.GetLatestFileNames("equipment", equipment.EquipmentId);
            var equipmentViewModel = new EquipmentImageDto()
            {
                Equipment = equipment,
                EquipmentImageUrl = latestFiles.latestImageFileName
            };
            return equipmentViewModel;
        }

    }

}
