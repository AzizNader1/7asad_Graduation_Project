using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Web_App.Models
{
    public class LogingUser
    {
        public int LogingUserId { get; set; }
        [Display(Name ="User Email")]
        [Required(ErrorMessage ="This field can not be empty")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [Display(Name = "User Password")]
        [Required(ErrorMessage = "This field can not be empty")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
        [Display(Name = "User Role")]
        [Required(ErrorMessage = "This field can not be empty")]
        public string UserRole { get; set; }
    }
}
