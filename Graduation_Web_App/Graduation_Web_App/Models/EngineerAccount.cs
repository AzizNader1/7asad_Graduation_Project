using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Web_App.Models
{
    public enum EngineerAccountTypes
    {
        [Display(Name = "Current")] Current,
        [Display(Name = "Ordinal")] Ordinal
    }
    public class EngineerAccount
    {
        public int EngineerAccountId { get; set; }
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
        [ForeignKey("Engineer")]
        public int EngineerId { get; set; }
        public virtual Engineer Engineer {  get; set; }
        [ForeignKey("Bank")]
        public int BankId { get; set;}
        public virtual Bank Bank { get; set; }
    }
}
