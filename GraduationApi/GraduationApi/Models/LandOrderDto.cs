using GraduationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class LandOrderDto
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
        public LandRentStatus LandRentStatus { get; set; }

        public int FarmerId { get; set; }

        public int CompanyId { get; set; }
        public int LandId { get; set; }

    }
}
