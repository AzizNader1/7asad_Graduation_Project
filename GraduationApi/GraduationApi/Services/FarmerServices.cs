using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationApi.Services
{
    public class FarmerServices : IFarmerServices
    {

        private readonly ApplicationDbContext _context;
        private readonly IFileServices _fileServices;

        public FarmerServices(ApplicationDbContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }
        public async Task<string> AddFarmer(Farmer farmer)
        {
            await _context.Farmers.AddAsync(farmer);
            _context.SaveChanges();
            return "A new farmer added successfully";
        }

        public string DeleteFarmer(Farmer farmer)
        {
            _context.Farmers.Remove(farmer);
            _context.SaveChanges();
            return "An existing farmer deleted successfully";
        }

        public async Task<IEnumerable<Farmer>> GetAllFarmers()
        {
            return await _context.Farmers.OrderBy(f => f.FarmerName).ToListAsync();
        }

        public async Task<Farmer> GetFarmerById(int id)
        {
            return await _context.Farmers.SingleOrDefaultAsync(f => f.FarmerId == id);
        }

        public async Task<Farmer> GetFarmerByName(string farmerName)
        {
            return await _context.Farmers.FirstOrDefaultAsync(b => b.FarmerName == farmerName);
        }

        public string UpdateFarmer(Farmer farmer)
        {
            _context.Farmers.Update(farmer);
            _context.SaveChanges();
            return "An existing farmer updated successfully";
        }

        public async Task<bool> IsValidFarmer(int id)
        {
            return await _context.Farmers.AnyAsync(g => g.FarmerId == id);
        }

        public async Task<List<FarmerImageDto>> GetFarmersWithImages()
        {
            var farmers = await GetAllFarmers();

            var farmersViewModels = new List<FarmerImageDto>();

            foreach (var farmer in farmers)
            {
                var latestFiles = await _fileServices.GetLatestFileNames("farmer", farmer.FarmerId);

                var farmerViewModel = new FarmerImageDto
                {
                    farmer = farmer,
                    FarmerImageUrl = latestFiles.latestImageFileName
                };

                farmersViewModels.Add(farmerViewModel);
            }

            return farmersViewModels;
        }

        public async Task<FarmerImageDto> GetFarmerWithImage(int id)
        {
            var farmer = await GetFarmerById(id);

            var latestFiles = await _fileServices.GetLatestFileNames("farmer", farmer.FarmerId);
            var farmerViewModel = new FarmerImageDto()
                {
                    farmer = farmer,
                    FarmerImageUrl = latestFiles.latestImageFileName
                };
            return farmerViewModel;
        }

    }
}

