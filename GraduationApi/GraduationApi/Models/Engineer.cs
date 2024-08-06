using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class Engineer
    {
        public int EngineerId { get; set; }

        [Display(Name = "Engineer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EngineerName { get; set;}

        [Display(Name = "Engineer Phone")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EngineerPhone { get; set;}

        [Display(Name ="Engineer Address")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string  EngineerAddress { get; set; }

        [Display(Name = "Engineer Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EngineerEmail { get; set;}

        [Display(Name = "Engineer Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EngineerPassword { get; set;}

        public virtual ICollection<EngineerAccount> EngineerAccounts { get; set;}

        public virtual ICollection<EngineerCompany> EngineerCompanies { get; set;}

        public virtual ICollection<EngineerFarmer> EngineerFarmers { get; set;}

        public virtual ICollection<FileInformation> FileInformations { get; set;}

    }
}
