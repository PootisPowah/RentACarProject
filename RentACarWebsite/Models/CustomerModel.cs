using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace RentACarWebsite.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50)]
        public string Fname { get; set; }


        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        public string Lname { get; set; }


        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(6)]
        [AllowedValues("Male","Female", ErrorMessage = "Gender must be either Male or Female.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(1, 150, ErrorMessage = "Age must be greater than 0.")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public double? Salary { get; set; }

        [Required(ErrorMessage = "Birthday is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }
    }
}
