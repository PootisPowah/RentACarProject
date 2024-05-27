using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentACarProject.Models
{
    /// <summary>
    /// Represents a customer of the car rental service.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the customer.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Fname { get; set; }

        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Lname { get; set; }

        /// <summary>
        /// Gets or sets the gender of the customer.
        /// </summary>
        [Required]
        [StringLength(6)]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the age of the customer.
        /// </summary>
        [Required]
        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets the salary of the customer.
        /// </summary>
        [Required]
        public double? Salary { get; set; }

        /// <summary>
        /// Gets or sets the birthday of the customer.
        /// </summary>
        [Required]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Gets or sets the collection of rents associated with the customer.
        /// </summary>
        public virtual ICollection<Rent> Rents { get; set; }
    }
}
