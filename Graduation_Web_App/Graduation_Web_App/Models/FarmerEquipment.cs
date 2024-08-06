using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Web_App.Models
{
    public enum EquipmentRentStatus
    {
        [Display(Name = "Pending")] Pending,
        [Display(Name = "Rejected")] Rejected,
        [Display(Name = "Accepted")] Accepted,
    }
    public class FarmerEquipment
    {
        public int FarmerEquipmentId { get; set; }
        [Display(Name ="Rent Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double RentPrice { get; set; }
        [Display(Name ="Rent Start Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime RentStartDate { get; set; }
        [Display(Name ="Rent End Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime RentEndDate { get; set;}
        [Display(Name = "Rent Status")]
        [Required(ErrorMessage ="this field can not be empty")]
        public EquipmentRentStatus EquipmentRentStatus { get; set; }
        [ForeignKey("Farmer")]
        public int FarmerId { get; set; }
        public Farmer Farmer { get; set; }
        [ForeignKey("Equipment")]
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

        [ForeignKey("BuyerFarmer")]
        public int BuyerFarmerId { get; set; }
        public BuyerFarmer BuyerFarmer { get; set; }
    }
}
