using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public enum UserTypes
    {
        [Display(Name = "مزارع -- بائع ومشتري")] Farmer,
        [Display(Name = "مزارع -- مشتري")] BuyerFarmer,
        [Display(Name = "مهندس")] Engineer,
        [Display(Name = "شركه")] Company
    }
    public class UserView
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "this field can not be empty")]
        public string UserName { get; set; }
        [Display(Name = "User Phone")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "this field can not be empty")]
        public string UserPhone { get; set; }
        [Display(Name = "User Address")]
        [Required(ErrorMessage = "this field can not be empty")]
        public string UserAddress { get; set; }
        [Display(Name = "User Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "this field can not be empty")]
        public string UserEmail { get; set; }
        [Display(Name = "User Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "this field can not be empty")]
        public string UserPassword { get; set; }
        [Display(Name = "Company Type")]
        [Required(ErrorMessage = "this field can not be empty")]
        public CompanyTypes CompanyType { get; set; }
        [Display(Name = "User Type")]
        [Required(ErrorMessage = "this field can not be empty")]
        public UserTypes UserType { get; set; }
    }
}
