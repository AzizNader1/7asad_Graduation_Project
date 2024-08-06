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
    public class LandServices : ILandServices
    {

        private readonly ApplicationDbContext _context;
        private readonly IFileServices _fileServices;

        public LandServices(ApplicationDbContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<string> AddLand(Land Land)
        {
            await _context.Lands.AddAsync(Land);
            _context.SaveChanges();
            return "a new Land added successfully";
        }

        public string DeleteLand(Land Land)
        {
            _context.Lands.Remove(Land);
            _context.SaveChanges();
            return "An existing Land deleted successfully";
        }

        public async Task<IEnumerable<Land>> GetAllLands()
        {
            return await _context.Lands.ToListAsync();
        }

        public async Task<Land> GetLandById(int id)
        {
            return await _context.Lands.FirstOrDefaultAsync(b => b.LandId == id);
        }

        public async Task<IEnumerable<Land>> GetLandsByFarmerId(int id)
        {
            return await _context.Lands
            .Include(c => c.Farmer)
            .Where(c => c.FarmerId == id)
            .ToListAsync();
        }

        public async Task<IEnumerable<Land>> GetLandsByFarmerName(string farmerName)
        {
            return await _context.Lands
           .Include(c => c.Farmer)
           .Where(c => c.Farmer.FarmerName == farmerName)
           .ToListAsync();
        }

        public string UpdateLand(Land Land)
        {
            _context.Lands.Update(Land);
            _context.SaveChanges();
            return "An existing Land updated successfully";
        }

        public async Task<bool> IsValidLand(int id)
        {
            return await _context.Lands.AnyAsync(g => g.LandId == id);
        }

        public async Task<List<LandImageDto>> GetLandsWithImages()
        {
            var lands = await GetAllLands();

            var landsViewModels = new List<LandImageDto>();

            foreach (var land in lands)
            {
                var latestFiles = await _fileServices.GetLatestFileNames("land", land.LandId);

                var landViewModel = new LandImageDto
                {
                    Land = land,
                    LandImageUrl = latestFiles.latestImageFileName
                };

                landsViewModels.Add(landViewModel);
            }

            return landsViewModels;
        }

        public async Task<LandImageDto> GetLandWithImage(int id)
        {
            var land = await GetLandById(id);

            var latestFiles = await _fileServices.GetLatestFileNames("land", land.LandId);
            var landViewModel = new LandImageDto()
            {
                Land = land,
                LandImageUrl = latestFiles.latestImageFileName
            };
            return landViewModel;
        }
    }
}
