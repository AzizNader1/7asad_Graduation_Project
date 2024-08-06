using Graduation_Web_App.Models;
using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class EngineerFarmerDetailsDto
    {
        public int Id { get; set; }
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
        [Display(Name = "Engineer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EngineerName { get; set; }
        public int FarmerId { get; set; }
        [Display(Name = "Farmer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerName { get; set; }
        public string EngineerPhone { get; set; }

    }
}
