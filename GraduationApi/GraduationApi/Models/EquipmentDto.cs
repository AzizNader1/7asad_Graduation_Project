using GraduationApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationApi.Models
{
    public class EquipmentDto
    {

        [Display(Name = "Equipment Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EquipmentName { get; set; }

        [Display(Name = "Equipment Price")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double EquipmentPrice { get; set; }

        [Display(Name = "Equipment Describtion")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EquipmentDescribtion { get; set; }

        public int FarmerId { get; set; }

    }
}
