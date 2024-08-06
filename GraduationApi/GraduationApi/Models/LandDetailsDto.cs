using GraduationApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationApi.Models
{
    public class LandDetailsDto
    {
        public int Id { get; set; }

        [Display(Name = "Land Location")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string LandLocation { get; set; }

        [Display(Name = "Land Size")]
        [Required(ErrorMessage ="this field can not be empty")]
        public double LandSize { get; set; }

        [Display(Name = "Land Type")]
        [Required(ErrorMessage ="this field can not be empty")]
        public LandTypes LandType { get; set; }

        [Display(Name = "Land Describtion")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string LandDescribtion { get; set; }

        public int FarmerId { get; set; }

        [Display(Name = "Farmer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerName { get; set; }

    }
}
