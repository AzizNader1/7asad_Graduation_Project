using System.ComponentModel.DataAnnotations;

namespace GraduationApi.Models
{
    public class RepresintorDetailsDto
    {
        public int Id { get; set; }

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

        [Display(Name = "Represintor Image")]
        public string RepresintorImageUrl { get; set; }

        public int CompanyId { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage ="this field can not be empty")]
        public string CompanyName { get; set; }

    }
}
