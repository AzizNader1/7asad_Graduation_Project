using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduationApi.Services
{
    public class FarmerEquipmentServices : IFarmerEquipmentServices
    {

        private readonly ApplicationDbContext _context;
        private readonly IFileServices _fileServices;

        public FarmerEquipmentServices(ApplicationDbContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<string> AddFarmerEquipment(FarmerEquipment FarmerEquipment)
        {
            await _context.FarmerEquipments.AddAsync(FarmerEquipment);
            _context.SaveChanges();
            return "A new equipment rent Order added successfully";
        }

        public string DeleteFarmerEquipment(FarmerEquipment FarmerEquipment)
        {
            _context.FarmerEquipments.Remove(FarmerEquipment);
            _context.SaveChanges();
            return "An existing equipment rent Order deleted successfully";
        }

        public async Task<IEnumerable<FarmerEquipment>> GetAllFarmerEquipments()
        {
            return await _context.FarmerEquipments.ToListAsync();
        }

        public async Task<FarmerEquipment> GetFarmerEquipmentById(int id)
        {
            return await _context.FarmerEquipments.SingleOrDefaultAsync(f => f.FarmerEquipmentId == id);
        }

        public string UpdateFarmerEquipment(FarmerEquipment FarmerEquipment)
        {
            _context.FarmerEquipments.Update(FarmerEquipment);
            _context.SaveChanges();
            return "An existing equipment rent Order updated successfully";
        }

        public async Task<IEnumerable<FarmerEquipment>> GetFarmerEquipmentsByEquipmentId(int EquipmentId)
        {
            return await _context.FarmerEquipments
                .Include(c => c.Equipment)
                .Where(c => c.EquipmentId == EquipmentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<FarmerEquipment>> GetFarmerEquipmentsByEquipmentName(string EquipmentName)
        {
            return await _context.FarmerEquipments
                .Include(c => c.Equipment)
                .Where(c => c.Equipment.EquipmentName == EquipmentName)
                .ToListAsync();
        }

        public async Task<IEnumerable<FarmerEquipment>> GetFarmerEquipmentsByFarmerName(string farmerName)
        {
            return await _context.FarmerEquipments
               .Include(c => c.Farmer)
               .Where(c => c.Farmer.FarmerName == farmerName)
               .ToListAsync();
        }

        public async Task<IEnumerable<FarmerEquipment>> GetFarmerEquipmentsByFarmerId(int farmerId)
        {
            return await _context.FarmerEquipments
                .Include(c => c.Farmer)
                .Where(c => c.FarmerId == farmerId)
                .ToListAsync();
        }

        public async Task<List<FarmerEquipmentImageDto>> GetFarmerEquipmentsWithImages()
        {
            var farmersEquipments = await GetAllFarmerEquipments();

            var farmersEquipmentViewModels = new List<FarmerEquipmentImageDto>();

            foreach (var farmerEquipment in farmersEquipments)
            {
                var latestFiles = await _fileServices.GetLatestFileNames("equipment", farmerEquipment.EquipmentId);

                var farmerEquipmentViewModel = new FarmerEquipmentImageDto
                {
                    FarmerEquipment = farmerEquipment,
                    FarmerEquipmentImageUrl = latestFiles.latestImageFileName
                };

                farmersEquipmentViewModels.Add(farmerEquipmentViewModel);
            }

            return farmersEquipmentViewModels;
        }

    }
}

