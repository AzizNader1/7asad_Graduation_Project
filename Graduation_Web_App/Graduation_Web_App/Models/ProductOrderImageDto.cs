using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class ProductOrderImageDto
    {
        public int Id { get; set; }
        [Display(Name = "Order Price")]
        [Required(ErrorMessage = "this field can not be empty")]
        public double OrderPrice { get; set; }
        [Display(Name = "Order Weight")]
        [Required(ErrorMessage = "this field can not be empty")]
        public double OrderWeight { get; set; }
        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "this field can not be empty")]
        public string ProductName { get; set; }
        [Display(Name = "Rent Status")]
        [Required(ErrorMessage = "this field can not be empty")]
        public ProductOffersStatus ProductOffersStatus { get; set; }
        public Company Company { get; set; }
        public Product Product { get; set; }
        public string CompanyImage { get; set; }
    }
}
