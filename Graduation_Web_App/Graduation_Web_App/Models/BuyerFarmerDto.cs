﻿using System.ComponentModel.DataAnnotations;

namespace Graduation_Web_App.Models
{
    public class BuyerFarmerDto
    {

        [Display(Name = "Farmer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerName { get; set; }


        [Display(Name = "Farmer Phone")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerPhone { get; set; }


        [Display(Name = "Farmer Address")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerAddress { get; set; }


        [Display(Name = "Farmer Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerEmail { get; set; }


        [Display(Name = "Farmer Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string FarmerPassword { get; set; }

        }

}
