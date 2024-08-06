using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationApi.Models
{
    public enum ProductQualityTypes 
    {
        [Display(Name = "جوده ضئيله")] ضئيله,
        [Display(Name = "جوده متوسطه")] متوسطه,
        [Display(Name = "جوده عاليه")] عاليه
    }
    public class Product
    {
        public int ProductId { get; set; }

        [Display(Name ="Product Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string ProductName { get; set; }

        [Display(Name ="Product Weight")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double ProductWeight { get; set; }

        [Display(Name ="Product Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double ProductPrice { get; set; }

        [Display(Name ="Product Quality")]
        [Required(ErrorMessage ="this field can not be empty")]
        public ProductQualityTypes ProductQuality { get; set; }

        [Display(Name = "Product Describtion")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string ProductDescribtion { get; set; }

        [ForeignKey("Farmer")]
        public int FarmerId { get; set; }

        public Farmer Farmer { get; set; }

    }
}
