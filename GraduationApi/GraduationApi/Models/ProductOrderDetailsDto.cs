using GraduationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class ProductOrderDetailsDto 
    {
        public int Id { get; set; }

        [Display(Name = "Order Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double OrderPrice { get; set; }

        [Display(Name = "Order Weight")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double OrderWeight { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string ProductName { get; set; }

        [Display(Name = "Rent Status")]
        [Required(ErrorMessage ="this field can not be empty")]
        public ProductOffersStatus ProductOffersStatus { get; set; }

        public string ProductOrderImageUrl { get; set; }

        public int FarmerId { get; set; }

        [Display(Name ="Farmer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerName { get; set; }

        public int CompanyId { get; set; }

        [Display(Name ="Company Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string CompanyName { get; set;}
        public string FarmerPhone { get; set; }

    }
}
