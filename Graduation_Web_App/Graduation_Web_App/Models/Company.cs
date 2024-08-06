using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public enum CompanyTypes
    {
        [Display(Name = "فرد")] Individual,
        [Display(Name = "عمل")] Business,
        [Display(Name = "شركات اشخاص")] Partnirship,
        [Display(Name = "شركات مساهمه")] Corporation
    }
    public class Company
    {
        public int CompanyId { get; set; }
        [Display(Name = "Company Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string CompanyName { get; set;}
        [Display(Name = "Company Address")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string CompanyAddress { get; set;}
        [Display(Name = "Company Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string CompanyEmail { get; set;}
        [Display(Name = "Company Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string CompanyPassword { get; set;}
        [Display(Name = "Company Type")]
        [Required(ErrorMessage ="this field can not be empty")]
        public CompanyTypes CompanyType { get; set;}
        public virtual ICollection<Represintor> Represintors { get; set;}
        public virtual ICollection<CompanyAccount> CompanyAccounts { get; set;}
        public virtual ICollection<EngineerCompany> EngineerCompanies { get; set;}
        public virtual ICollection<LandOrder> LandOrders { get; set;}
        public virtual ICollection<ProductOrder> ProductOrders { get; set;}
        public virtual ICollection<FileInformation> FileInformations { get; set;}
    }
}
