using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class EngineerDto
    {
        [Display(Name = "Engineer Name")]
        public string EngineerName { get; set; }
        [Display(Name = "Engineer Phone")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EngineerPhone { get; set; }
        [Display(Name = "Engineer Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EngineerEmail { get; set; }
        [Display(Name = "Engineer Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EngineerPassword { get; set; }
        [Display(Name = "Engineer Address")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EngineerAddress { get; set; }
        
    }
}
