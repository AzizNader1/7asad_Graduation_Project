using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Web_App.Models
{
    public enum FarmerAccountTypes
    {
        [Display(Name = "Current")] Current,
        [Display(Name = "Ordinal")] Ordinal
    }
    public class FarmerAccount
    {
        public int FarmerAccountId { get; set; }
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
        [ForeignKey("Farmer")]
        public int FarmerId { get; set; }
        public Farmer Farmer { get; set; }
        [ForeignKey("Bank")]
        public int BankId { get; set;}
        public Bank Bank { get; set; }
    }
}
