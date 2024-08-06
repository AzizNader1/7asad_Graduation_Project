using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace GraduationApi.Models
{
    public enum ServiceStatusEC 
    {
        [Display(Name ="Pending")] Pending,
        [Display(Name = "Rejected")] Rejected,
        [Display(Name = "Accepted")] Accepted,
        [Display(Name = "Completed")] Completed,
        [Display(Name = "UnderProcessing")] UnderProcessing
    }
    public class EngineerCompany
    {
        public int EngineerCompanyId { get; set; }

        [Display(Name ="Service Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double ServicePrice { get; set; }

        [Display(Name = "Service Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime ServiveDate { get; set; }

        [Display(Name ="Service Status")]
        [Required(ErrorMessage ="this field can not be empty")]
        public ServiceStatusEC Status { get; set; }

        public Company Company { get; set; }

        public int CompanyId { get; set; }

        public Engineer Engineer { get; set; }

        public int EngineerId { get; set; }

    }
}
