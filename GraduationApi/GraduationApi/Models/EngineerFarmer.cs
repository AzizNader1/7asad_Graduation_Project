using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationApi.Models
{
    public enum ServiceStatusEF
    {
        [Display(Name = "Pending")] Pending,
        [Display(Name = "Rejected")] Rejected,
        [Display(Name = "Accepted")] Accepted,
        [Display(Name = "Completed")] Completed,
        [Display(Name = "UnderProcessing")] UnderProcessing
    }
    public class EngineerFarmer
    {
        public int EngineerFarmerId { get; set; }

        [Display(Name ="Service Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double ServicePrice { get; set; }

        [Display(Name = "Service Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime ServiveDate { get; set; }

        [Display(Name = "Service Status")]
        [Required(ErrorMessage ="this field can not be empty")]
        public ServiceStatusEF Status { get; set; }

        public int FarmerId { get; set;}

        public Farmer Farmer { get; set; }
        public int EngineerId { get;set;}

        public Engineer Engineer { get; set; }

    }
}
