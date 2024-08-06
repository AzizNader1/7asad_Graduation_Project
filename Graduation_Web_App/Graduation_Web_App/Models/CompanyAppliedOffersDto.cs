using GraduationApi.Models;

namespace Graduation_Web_App.Models
{
    public class CompanyAppliedOffersDto
    {
        public List<EngineerCompanyDetailsDto> EngineerCompany { get; set; }
        public List<LandOrderDetailsDto> LandOrder { get; set; }
        public List<ProductOrderDetailsDto> ProductOrder { get; set; }
    }
}
