using GraduationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class EngineerAccountDto
    {
        [Display(Name = "Account Number")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string AccountNumber { get; set; }

        [Display(Name = "Account Balance")]
        [Required(ErrorMessage = "this field can not be empty")]
        public double AccountBalance { get; set; }

        [Display(Name = "CVV Number")]
        [Required(ErrorMessage = "this field can not be empty")]
        public int CvvNumber { get; set; }

        [Display(Name = "Expire Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime ExpireDate { get; set; }

        [Display(Name = "Account Type")]
        [Required(ErrorMessage ="this field can not be empty")]
        public EngineerAccountTypes AccountType { get; set; }

        public int EngineerId { get; set; }

        public int BankId { get; set; }

    }
}
