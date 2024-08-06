using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationApi.Models
{
    public class Represintor
    {
        public int RepresintorId { get; set; }

        [Display(Name = "Represintor Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string RepresintorName { get; set; }

        [Display(Name = "Represintor Phone")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string RepresintorPhone { get; set; }

        [Display(Name = "Represintor Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="this field can not be empty")]
        public string RepresintorEmail { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

    }
}
