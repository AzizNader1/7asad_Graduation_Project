using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace GraduationApi.Services
{
    public class RepresintorServices : IRepresintorServices
    {

        private readonly ApplicationDbContext _context;
        private readonly IFileServices _fileServices;

        public RepresintorServices(ApplicationDbContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<string> AddRepresintor(Represintor represintor)
        {
            await _context.Represintors.AddAsync(represintor);
            _context.SaveChanges();
            return "a new represintor added successfully";
        }

        public string DeleteRepresintor(Represintor represintor)
        {
            _context.Represintors.Remove(represintor);
            _context.SaveChanges();
            return "An existing represintor deleted successfully";
        }

        public async Task<IEnumerable<Represintor>> GetAllRepresintors()
        {
            return await _context.Represintors.OrderBy(b => b.RepresintorName).ToListAsync();
        }

        public async Task<Represintor> GetRepresintorById(int id)
        {
            return await _context.Represintors.FirstOrDefaultAsync(b => b.RepresintorId == id);
        }

        public async Task<Represintor> GetRepresintorByName(string represintorName)
        {
            return await _context.Represintors.FirstOrDefaultAsync(b => b.RepresintorName == represintorName);
        }

        public async Task<IEnumerable<Represintor>> GetRepresintorsByCompanyId(int id)
        {
            return await _context.Represintors
            .Include(c => c.Company)
            .Where(c => c.CompanyId == id)
            .ToListAsync();
        }

        public async Task<IEnumerable<Represintor>> GetRepresintorsByCompanyName(string companyName)
        {
            return await _context.Represintors
           .Include(c => c.Company)
           .Where(c => c.Company.CompanyName == companyName)
           .ToListAsync();
        }

        public string UpdateRepresintor(Represintor represintor)
        {
            _context.Represintors.Update(represintor);
            _context.SaveChanges();
            return "An existing represintor updated successfully";
        }

        public async Task<bool> IsValidRepresintor(int id)
        {
            return await _context.Represintors.AnyAsync(g => g.RepresintorId == id);
        }

        public async Task<List<RepresintorImageDto>> GetRepresintorsWithImages()
        {
            var represintors = await GetAllRepresintors();

            var represintorsViewModels = new List<RepresintorImageDto>();

            foreach (var represintor in represintors)
            {
                var latestFiles = await _fileServices.GetLatestFileNames("represintor", represintor.RepresintorId);

                var represintorViewModel = new RepresintorImageDto
                {
                    Represintor = represintor,
                    RepresintorImageUrl = latestFiles.latestImageFileName
                };

                represintorsViewModels.Add(represintorViewModel);
            }

            return represintorsViewModels;
        }

        public async Task<RepresintorImageDto> GetRepresintorWithImage(int id)
        {
            var represintor = await GetRepresintorById(id);

            var latestFiles = await _fileServices.GetLatestFileNames("represintor", represintor.RepresintorId);
            var represintorViewModel = new RepresintorImageDto()
            {
                Represintor = represintor,
                RepresintorImageUrl = latestFiles.latestImageFileName
            };
            return represintorViewModel;
        }

    }
}
