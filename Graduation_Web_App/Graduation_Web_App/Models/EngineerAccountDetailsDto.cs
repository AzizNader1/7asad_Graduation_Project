﻿using Graduation_Web_App.Models;
using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class EngineerAccountDetailsDto 
    {
        public int Id { get; set; }
        [Display(Name = "Account Number")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string AccountNumber { get; set; }
        [Display(Name = "Account Balance")]
        [Required(ErrorMessage = "this field can not be empty")]
        public double AccountBalance { get; set; }
        [Display(Name = "CVV Number")]
        [Required(ErrorMessage = "this field can not be empty")]
        public int CvvNumber { get; set; }
        [Display(Name = "Expire Date")]
        [Required(ErrorMessage ="this field can not be empty")]
        public DateTime ExpireDate { get; set; }
        [Display(Name = "Account Type")]
        [Required(ErrorMessage ="this field can not be empty")]
        public EngineerAccountTypes AccountType { get; set; }
        public int EngineerId { get; set; }
        [Display(Name = "Engineer Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string EngineerName { get; set; }
        public int BankId { get; set; }
        [Display(Name = "Bank Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string BankName { get; set; }
    }
}