using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class LogingView
    {
        [Display(Name = "User Email")]
        [Required(ErrorMessage = "This field can not be empty")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [Display(Name = "User Password")]
        [Required(ErrorMessage = "This field can not be empty")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}
