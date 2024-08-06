using Graduation_Web_App.Models;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class ProductDto
    {
        [Display(Name = "Product Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string ProductName { get; set; }
        [Display(Name = "Product Weight")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double ProductWeight { get; set; }
        [Display(Name = "Product Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double ProductPrice { get; set; }
        [Display(Name = "Product Quality")]
        [Required(ErrorMessage ="this field can not be empty")]
        public ProductQualityTypes ProductQuality { get; set; }
        [Display(Name = "Product Describtion")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string ProductDescribtion { get; set; }
        
        public int FarmerId { get; set; }
    }
}
