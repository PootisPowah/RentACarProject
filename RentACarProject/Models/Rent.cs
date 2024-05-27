using System;
using System.ComponentModel.DataAnnotations;

namespace RentACarProject.Models
{
    /// <summary>
    /// Represents a rental transaction between a customer and a car.
    /// </summary>
    public class Rent
    {
        /// <summary>
        /// Gets or sets the unique identifier for the rental transaction.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the customer renting the car.
        /// </summary>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the car being rented.
        /// </summary>
        [Required]
        public int CarId { get; set; }

        /// <summary>
        /// Gets or sets the start date of the rental period.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the rental period.
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the price of the rent.
        /// </summary>
        [Required]
        public double RentPrice { get; set; }

        /// <summary>
        /// Gets or sets the pickup street for the rental.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string PickupStreet { get; set; }

        // Navigation properties

        /// <summary>
        /// Gets or sets the customer associated with the rental.
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the car associated with the rental.
        /// </summary>
        public virtual Car Car { get; set; }
    }
}
