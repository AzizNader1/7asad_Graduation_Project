using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduationApi.Services
{
    public class EngineerServices : IEngineerServices
    {

        private readonly ApplicationDbContext _context;
        private readonly IFileServices _fileServices;

        public EngineerServices(ApplicationDbContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<string> AddEngineer(Engineer engineer)
        {
            await _context.Engineers.AddAsync(engineer);
            _context.SaveChanges();
            return "a new engineer added successfully";
        }

        public string DeleteEngineer(Engineer engineer)
        {
            _context.Engineers.Remove(engineer);
            _context.SaveChanges();
            return "An existing engineer deleted successfully";
        }

        public async Task<IEnumerable<Engineer>> GetAllEngineers()
        {
            return await _context.Engineers.OrderBy(b => b.EngineerName).ToListAsync();
        }

        public async Task<Engineer> GetEngineerById(int id)
        {
            return await _context.Engineers.FirstOrDefaultAsync(b => b.EngineerId == id);
        }

        public async Task<Engineer> GetEngineerByName(string engineerName)
        {
            return await _context.Engineers.FirstOrDefaultAsync(b => b.EngineerName == engineerName);
        }

        public async Task<bool> IsValidEngineer(int id)
        {
            return await _context.Engineers.AnyAsync(g => g.EngineerId == id);
        }

        public string UpdateEngineer(Engineer engineer)
        {
            _context.Engineers.Update(engineer);
            _context.SaveChanges();
            return "An existing engineer updated successfully";
        }

        public async Task<List<EngineerImageDto>> GetEngineersWithImages()
        {
            var engineers = await GetAllEngineers();

            var engineersViewModels = new List<EngineerImageDto>();

            foreach (var engineer in engineers)
            {
                var latestFiles = await _fileServices.GetLatestFileNames("engineer", engineer.EngineerId);

                var engineerViewModel = new EngineerImageDto
                {
                    Engineer = engineer,
                    EngineerImageUrl = latestFiles.latestImageFileName
                };

                engineersViewModels.Add(engineerViewModel);
            }

            return engineersViewModels;

        }

        public async Task<EngineerImageDto> GetEngineerWithImage(int id)
        {
            var engineer = await GetEngineerById(id);

            var latestFiles = await _fileServices.GetLatestFileNames("engineer", engineer.EngineerId);
            var engineerViewModel = new EngineerImageDto()
            {
                Engineer = engineer,
                EngineerImageUrl = latestFiles.latestImageFileName
            };
            return engineerViewModel;
        }

    }
}
