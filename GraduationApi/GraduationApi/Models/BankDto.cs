using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class BankDto
    {

        [Required(ErrorMessage = "this field can not be empty")]
        [Display(Name ="Bank Name")]
        public string BankName { get; set; }

    }
}
