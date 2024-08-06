using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationApi.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }

        [Display(Name ="Equipment Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EquipmentName { get; set; }

        [Display(Name = "Equipment Price")] 
        [Required(ErrorMessage ="this field can not be empty")]
        public double EquipmentPrice { get; set; }

        [Display(Name = "Equipment Describtion")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EquipmentDescribtion { get; set; }

        [ForeignKey("Farmer")]
        public int FarmerId { get; set; }

        public Farmer Farmer { get; set; }

        public virtual ICollection<FarmerEquipment> FarmerEquipments { get; set; }

        public virtual ICollection<FileInformation> FileInformations { get; set; }

    }
}
