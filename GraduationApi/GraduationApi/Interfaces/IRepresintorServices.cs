using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IRepresintorServices
    {
        Task<IEnumerable<Represintor>> GetAllRepresintors();

        Task<List<RepresintorImageDto>> GetRepresintorsWithImages();

        Task<Represintor> GetRepresintorById(int id);

        Task<RepresintorImageDto> GetRepresintorWithImage(int id);

        Task<Represintor> GetRepresintorByName(string represintorName);

        Task<IEnumerable<Represintor>> GetRepresintorsByCompanyId(int id);

        Task<IEnumerable<Represintor>> GetRepresintorsByCompanyName(string companyName);

        Task<string> AddRepresintor(Represintor represintor);

        string UpdateRepresintor(Represintor represintor);

        string DeleteRepresintor(Represintor represintor);

        Task<bool> IsValidRepresintor(int id);

    }
}
