using Graduation_Web_App.Models;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class LandOrderDetailsDto 
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
        public LandRentStatus LandRentStatus { get; set; }
        public string LandOrderImageUrl { get; set; }
        public int FarmerId { get; set; }
        [Display(Name = "Farmer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerName { get; set; }
        public int CompanyId { get; set; }
        [Display(Name = "Company Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string CompanyName { get; set; }
        public string FarmerPhone { get; set; }

        public int LandId { get; set; }

    }
}
