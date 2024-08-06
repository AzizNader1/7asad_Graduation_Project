namespace Graduation_Web_App.Models
{
    public class BuyerFarmerAppliedOffersDto
    {
        public List<FarmerLandOrderDetailsDto> LandOrder { get; set; }
        public List<FarmerProductOrderDetailsDto> ProductOrder { get; set; }
    }
}
