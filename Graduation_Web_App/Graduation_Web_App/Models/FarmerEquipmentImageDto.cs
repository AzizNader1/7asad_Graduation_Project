using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class FarmerEquipmentImageDto
    {
        public int Id { get; set; }
        [Display(Name = "Rent Price")]
        [Required(ErrorMessage = "this field can not be empty")]
        public double RentPrice { get; set; }
        [Display(Name = "Rent Start Date")]
        [Required(ErrorMessage = "this field can not be empty")]
        public DateTime RentStartDate { get; set; }
        [Display(Name = "Rent End Date")]
        [Required(ErrorMessage = "this field can not be empty")]
        public DateTime RentEndDate { get; set; }
        [Display(Name = "Rent Status")]
        [Required(ErrorMessage = "this field can not be empty")]
        public EquipmentRentStatus EquipmentRentStatus { get; set; }
        public string FarmerEquipmentImageUrl { get; set; }

        public Farmer Farmer {  get; set; }

        public Equipment Equipment { get; set;}
        public BuyerFarmer BuyerFarmer { get; set; }
    }
}
