using GraduationApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationApi.Models
{
    public class FarmerLandOrderDetailsDto 
    {
        public int Id { get; set; }

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

        public string LandOrderImageUrl { get; set; }

        public int FarmerId { get; set; }

        [Display(Name = "Farmer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerName { get; set; }

        public int BuyerFarmerId { get; set; }

        [Display(Name = "Buyer Farmer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string BuyerFarmerName { get; set; }

        public int LandId { get; set; }

        public string OwnerPhone { get; set; }
    }
}
