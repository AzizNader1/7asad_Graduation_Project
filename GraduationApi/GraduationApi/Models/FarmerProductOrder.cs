using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationApi.Models
{
    public enum FarmerProductOffersStatus
    {
        [Display(Name = "Pending")] Pending,
        [Display(Name = "Rejected")] Rejected,
        [Display(Name = "Accepted")] Accepted,
    }
    public class FarmerProductOrder
    {
        public int FarmerProductOrderId { get; set; }

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

        [ForeignKey("Farmer")]
        public int FarmerId { get; set; }

        public Farmer Farmer { get; set; }

        [ForeignKey("BuyerFarmer")]
        public int BuyerFarmerId { get; set;}

        public BuyerFarmer BuyerFarmer { get; set; }

    }
}
