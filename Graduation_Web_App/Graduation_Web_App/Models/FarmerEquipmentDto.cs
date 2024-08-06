using Graduation_Web_App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Web_App.Models
{
    public class FarmerEquipmentDto
    {
        [Display(Name = "Rent Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double RentPrice { get; set; }
        [Display(Name = "Rent Start Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime RentStartDate { get; set; }
        [Display(Name = "Rent End Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime RentEndDate { get; set; }
        [Display(Name = "Rent Status")]
        [Required(ErrorMessage ="this field can not be empty")]
        public EquipmentRentStatus EquipmentRentStatus { get; set; }
        public int FarmerId { get; set; }
        public int EquipmentId { get; set; }
        public int BuyerFarmerId { get; set; }
    }
}
