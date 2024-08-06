using Graduation_Web_App.Models;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class FarmerProductOrderDetailsDto 
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
        public FarmerProductOffersStatus ProductOffersStatus { get; set; }

        public string ProductOrderImageUrl { get; set; }

        public int FarmerId { get; set; }

        [Display(Name ="Farmer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerName { get; set; }

        public int BuyerFarmerId { get; set; }

        [Display(Name ="Buyer Farmer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string BuyerFarmerName { get; set;}
        public string OwnerPhone { get; set; }

    }
}
