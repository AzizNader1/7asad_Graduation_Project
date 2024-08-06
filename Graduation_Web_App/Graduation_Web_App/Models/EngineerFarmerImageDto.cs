using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class EngineerFarmerImageDto
    {
        public int Id { get; set; }
        [Display(Name = "Service Price")]
        [Required(ErrorMessage = "this field can not be empty")]
        public double ServicePrice { get; set; }
        [Display(Name = "Service Date")]
        [Required(ErrorMessage = "this field can not be empty")]
        public DateTime ServiveDate { get; set; }
        [Display(Name = "Service Status")]
        [Required(ErrorMessage = "this field can not be empty")]
        public ServiceStatusEF Status { get; set; }
        public int EngnieerId { get; set; }
        [Display(Name = "Engineer Name")]
        [Required(ErrorMessage = "this field can not be empty")]
        public string EngineerName { get; set; }
        public string FarmerImage {  get; set; }
        public Farmer Farmer { get; set; }
    }
}
