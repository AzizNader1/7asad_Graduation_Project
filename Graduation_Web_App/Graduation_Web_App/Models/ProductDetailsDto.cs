using Graduation_Web_App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Web_App.Models
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
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
        [Display(Name="Farmer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerName { get; set; }
        
    }
}
