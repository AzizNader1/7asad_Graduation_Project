using GraduationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class EngineerFarmerDto
    {

        [Display(Name = "Service Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double ServicePrice { get; set; }

        [Display(Name = "Service Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime ServiveDate { get; set; }

        [Display(Name = "Service Status")]
        [Required(ErrorMessage ="this field can not be empty")]
        public ServiceStatusEF Status { get; set; }

        public int EngnieerId { get; set; }

        public int FarmerId { get; set; }

    }
}
