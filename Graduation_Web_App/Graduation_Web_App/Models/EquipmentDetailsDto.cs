using Graduation_Web_App.Models;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class EquipmentDetailsDto
    {
        public int Id { get; set; }
        [Display(Name = "Equipment Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EquipmentName { get; set; }
        [Display(Name = "Equipment Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double EquipmentPrice { get; set; }
        [Display(Name = "Equipment Describtion")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EquipmentDescribtion { get; set; }
        public int FarmerId { get; set; }
        [Display(Name ="Farmer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerName { get; set; }
        [Display(Name = "Equipment Image")]
        public string EquipmentImageUrl { get; set; }

    }
}
