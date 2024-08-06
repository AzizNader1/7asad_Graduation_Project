using Graduation_Web_App.Models;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class FarmerLandOrderDto
    {

        [Display(Name = "Order Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double OrderPrice { get; set; }

        [Display(Name = "Land Size")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double LandSize { get; set; }

        [Display(Name = "Order Start Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime OrderStartDate { get; set; }
            
        [Display(Name = "Order End Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime OrderEndDate { get; set; }

        [Display(Name = "Rent Status")]
        [Required(ErrorMessage ="this field can not be empty")]
        public FarmerLandRentStatus LandRentStatus { get; set; }

        public int FarmerId { get; set; }

        public int BuyerFarmerId { get; set; }
        public int LandId { get; set; }

    }
}
