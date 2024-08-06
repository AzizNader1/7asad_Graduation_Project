using GraduationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class FarmerAccountDetailsDto
    {
        public int Id { get; set; }

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
        public FarmerAccountTypes AccountType { get; set; }

        public int FarmerId { get; set; }

        [Display(Name = "Farmer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerName { get; set; }

        public int BankId { get; set; }

        [Display(Name = "Bank Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string BankName { get; set; }

    }
}
