using System.ComponentModel.DataAnnotations;

namespace RentACarWebsite.Models
{
    public class RentModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Customer Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Customer Id must be a positive number.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Car Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Car Id must be a positive number.")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Rent Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Rent Price must be a positive number.")]
        public double RentPrice { get; set; }

        [Required(ErrorMessage = "Pickup Street is required")]
        [StringLength(100, ErrorMessage = "Pickup Street cannot be longer than 100 characters.")]
        public string PickupStreet { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "End Date must be greater than Start Date.",
                    new[] { nameof(EndDate) });
            }
        }
    }
}
