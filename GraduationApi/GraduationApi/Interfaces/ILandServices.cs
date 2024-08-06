using GraduationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationApi.Interfaces
{
    public interface ILandServices
    {
        Task<IEnumerable<Land>> GetAllLands();

        Task<List<LandImageDto>> GetLandsWithImages();

        Task<Land> GetLandById(int id);

        Task<LandImageDto> GetLandWithImage(int id);

        Task<IEnumerable<Land>> GetLandsByFarmerId(int id);

        Task<IEnumerable<Land>> GetLandsByFarmerName(string farmerName);

        Task<string> AddLand(Land Land);

        string UpdateLand(Land Land);

        string DeleteLand(Land Land);

        Task<bool> IsValidLand(int id);

    }
}
