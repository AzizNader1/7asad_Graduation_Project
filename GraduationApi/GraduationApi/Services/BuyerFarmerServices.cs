using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationApi.Services
{
    public class BuyerFarmerServices : IBuyerFarmerServices
    {

        private readonly ApplicationDbContext _context;
        private readonly IFileServices _fileServices;

        public BuyerFarmerServices(ApplicationDbContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }
        public async Task<string> AddBuyerFarmer(BuyerFarmer farmer)
        {
            await _context.BuyerFarmers.AddAsync(farmer);
            _context.SaveChanges();
            return "A new farmer added successfully";
        }

        public string DeleteBuyerFarmer(BuyerFarmer farmer)
        {
            _context.BuyerFarmers.Remove(farmer);
            _context.SaveChanges();
            return "An existing farmer deleted successfully";
        }

        public async Task<IEnumerable<BuyerFarmer>> GetAllBuyerFarmers()
        {
            return await _context.BuyerFarmers.OrderBy(f => f.FarmerName).ToListAsync();
        }

        public async Task<BuyerFarmer> GetBuyerFarmerById(int id)
        {
            return await _context.BuyerFarmers.SingleOrDefaultAsync(f => f.BuyerFarmerId == id);
        }

        public async Task<BuyerFarmer> GetBuyerFarmerByName(string farmerName)
        {
            return await _context.BuyerFarmers.FirstOrDefaultAsync(b => b.FarmerName == farmerName);
        }

        public string UpdateBuyerFarmer(BuyerFarmer farmer)
        {
            _context.BuyerFarmers.Update(farmer);
            _context.SaveChanges();
            return "An existing farmer updated successfully";
        }

        public async Task<bool> IsValidBuyerFarmer(int id)
        {
            return await _context.BuyerFarmers.AnyAsync(g => g.BuyerFarmerId == id);
        }

        public async Task<List<BuyerFarmerImageDto>> GetBuyerFarmersWithImages()
        {
            var farmers = await GetAllBuyerFarmers();

            var farmersViewModels = new List<BuyerFarmerImageDto>();

            foreach (var farmer in farmers)
            {
                var latestFiles = await _fileServices.GetLatestFileNames("buyerfarmer", farmer.BuyerFarmerId);

                var farmerViewModel = new BuyerFarmerImageDto
                {
                    BuyerFarmer = farmer,
                    BuyerFarmerImageUrl = latestFiles.latestImageFileName
                };

                farmersViewModels.Add(farmerViewModel);
            }

            return farmersViewModels;
        }

        public async Task<BuyerFarmerImageDto> GetBuyerFarmerWithImage(int id)
        {
            var farmer = await GetBuyerFarmerById(id);

            var latestFiles = await _fileServices.GetLatestFileNames("buyerfarmer", farmer.BuyerFarmerId);
            var farmerViewModel = new BuyerFarmerImageDto()
                {
                    BuyerFarmer = farmer,
                    BuyerFarmerImageUrl = latestFiles.latestImageFileName
                };
            return farmerViewModel;
        }

    }
}

