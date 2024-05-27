using System;
using System.Text.Json.Serialization;

namespace RentACarProject.Dto
{
    /// <summary>
    /// Data transfer object for representing rental information.
    /// </summary>
    public class RentDto
    {
        private string _pickupStreet;
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the customer renting the car.
        /// </summary>
        public int? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the car being rented.
        /// </summary>
        public int? CarId { get; set; }

        /// <summary>
        /// Gets or sets the start date of the rental.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the rental.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the price of the rental.
        /// </summary>
        public double RentPrice { get; set; }

        /// <summary>
        /// Gets or sets the pickup street for the rental.
        /// </summary>
        public string PickupStreet
        {
            get => _pickupStreet;
            set => _pickupStreet = value != null ? char.ToUpper(value[0]) + value.Substring(1).ToLower() : null;
        }
       
    }
}
