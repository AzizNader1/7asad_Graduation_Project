using GraduationApi.Models;

namespace Graduation_Web_App.Models
{
    public class FarmerAppliedOffersDto
    {
        public List<EngineerFarmerDetailsDto> EngineerFarmer { get; set; }
        public List<FarmerEquipmentDetailsDto> FarmerEquipment { get; set;}
        public List<FarmerLandOrderDetailsDto> LandOrder { get; set; }
        public List<FarmerProductOrderDetailsDto> ProductOrder { get; set; }
    }
}
