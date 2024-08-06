using GraduationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class CompanyDto
    {
        [Display(Name = "Company Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Address")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string CompanyAddress { get; set; }

        [Display(Name = "Company Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string CompanyEmail { get; set; }

        [Display(Name = "Company Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string CompanyPassword { get; set; }

        [Display(Name = "Company Type")]
        [Required(ErrorMessage ="this field can not be empty")]
        public CompanyTypes CompanyType { get; set; }

    }
}
