using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Web_App.Models
{
    public enum LandRentStatus
    {
        [Display(Name = "Pending")] Pending,
        [Display(Name = "Rejected")] Rejected,
        [Display(Name = "Accepted")] Accepted,
    }
    public class LandOrder
    {
        public int LandOrderId { get; set; }
        [Display(Name ="Order Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double OrderPrice { get; set; }
        [Display(Name ="Land Size")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double LandSize { get; set; }
        [Display(Name ="Order Start Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime OrderStartDate { get; set; }
        [Display(Name ="Order End Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime OrderEndDate { get; set; }
        [Display(Name = "Rent Status")]
        [Required(ErrorMessage ="this field can not be empty")]
        public LandRentStatus LandRentStatus { get; set; }
        [ForeignKey("Farmer")]
        public int FarmerId { get; set; }
        public Farmer Farmer { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }  
        public Company Company { get; set; }

        [ForeignKey("Land")]
        public int LandId { get; set; }
        public Land Land { get; set; }

    }
}
