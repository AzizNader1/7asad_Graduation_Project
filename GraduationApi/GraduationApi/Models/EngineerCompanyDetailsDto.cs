using GraduationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class EngineerCompanyDetailsDto 
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
        public ServiceStatusEC Status { get; set; }

        public int EngineerId { get; set; }

        [Display(Name = "Engineer Name")]
        public string EngineerName { get; set; }

        public int CompanyId { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public string EngineerPhone { get; set; }

    }
}
