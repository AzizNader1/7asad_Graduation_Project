using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Web_App.Models
{
    public enum LandTypes
    {
        [Display(Name = "تربه جافه")] جافه,
        [Display(Name = "تربه رمليه")] رمليه,
        [Display(Name = "تربه طينيه")] طينيه,
        [Display(Name = "تربه طمييه")] طمييه,
        [Display(Name = "تربه عضوية")] عضويه
    }
    public class Land
    {
        public int LandId { get; set; }
        [Display(Name ="Land Location")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string LandLocation { get; set; }
        [Display(Name ="Land Size")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double LandSize { get; set; }
        [Display(Name ="Land Type")]
        [Required(ErrorMessage ="this field can not be empty")]
        public LandTypes LandType{ get; set; }
        [Display(Name = "Land Describtion")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string LandDescribtion { get; set; }
        
        [ForeignKey("Farmer")]
        public int FarmerId { get; set; }   
        public Farmer Farmer { get; set; }
    }
}
