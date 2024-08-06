using Graduation_Web_App.Models;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class LandDto
    {
        [Display(Name = "Land Location")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string LandLocation { get; set; }
        [Display(Name = "Land Size")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double LandSize { get; set; }
        [Display(Name = "Land Type")]
        [Required(ErrorMessage ="this field can not be empty")]
        public LandTypes LandType { get; set; }
        [Display(Name = "Land Describtion")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string LandDescribtion { get; set; }
        public int FarmerId { get; set; }
        
    }
}
