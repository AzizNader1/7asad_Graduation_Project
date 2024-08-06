using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class FarmerProductOrderImageDto
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
        public FarmerProductOffersStatus ProductOffersStatus { get; set; }
        public BuyerFarmer BuyerFarmer { get; set; }
        public Product Product { get; set; }
        public string BuyerImage { get; set; }
        public int FarmerId { get; set; }

    }
}
