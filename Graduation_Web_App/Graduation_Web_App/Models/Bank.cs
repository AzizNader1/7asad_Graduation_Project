using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class Bank
    {
        public int BankId { get; set; }
        [Display(Name ="Bank Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string BankName { get; set; }
        public virtual ICollection<EngineerAccount> EngineerAccounts { get; set;} 
        public virtual ICollection<FarmerAccount> FarmerAccounts { get; set;} 
        public virtual ICollection<CompanyAccount> CompanyAccounts { get; set;}
    }
}
