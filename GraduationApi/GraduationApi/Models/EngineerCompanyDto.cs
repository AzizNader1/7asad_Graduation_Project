using GraduationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class EngineerCompanyDto
    {

        [Display(Name ="Service Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double ServicePrice { get; set; }

        [Display(Name = "Service Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime ServiveDate { get; set; }

        [Display(Name = "Service Status")]
        [Required(ErrorMessage ="this field can not be empty")]
        public ServiceStatusEC Status { get; set; }

        public int EngineerId { get; set; }

        public int CompanyId { get; set; }

    }
}
