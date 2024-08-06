using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IEngineerServices
    {
        Task<IEnumerable<Engineer>> GetAllEngineers();

        Task<List<EngineerImageDto>> GetEngineersWithImages();

        Task<Engineer> GetEngineerById(int id);

        Task<EngineerImageDto> GetEngineerWithImage(int id);

        Task<Engineer> GetEngineerByName(string engineerName);

        Task<string> AddEngineer(Engineer compnay);

        string UpdateEngineer(Engineer compnay);

        string DeleteEngineer(Engineer compnay);

        Task<bool> IsValidEngineer(int id);

    }
}
