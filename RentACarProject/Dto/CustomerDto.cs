using System;
using System.ComponentModel.DataAnnotations;

namespace RentACarProject.Dto
{
    /// <summary>
    /// Data transfer object for representing customer information.
    /// </summary>
    public class CustomerDto
    {
        private string _fname;
        private string _lname;
        private string _gender;
        public int id { get; set; }
        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        [StringLength(50)]
        public string Fname
        {
            get => _fname;
            set => _fname = value != null ? char.ToUpper(value[0]) + value.Substring(1).ToLower() : null;
        }

        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        [StringLength(50)]
        public string Lname
        {
            get => _lname;
            set => _lname = value != null ? char.ToUpper(value[0]) + value.Substring(1).ToLower() : null;
        }

        /// <summary>
        /// Gets or sets the gender of the customer.
        /// </summary>
        [StringLength(6)]
        public string Gender
        {
            get => _gender;
            set => _gender = value != null ? char.ToUpper(value[0]) + value.Substring(1).ToLower() : null;
        }

        /// <summary>
        /// Gets or sets the age of the customer.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the salary of the customer.
        /// </summary>
        public double Salary { get; set; }

        /// <summary>
        /// Gets or sets the birthday of the customer.
        /// </summary>
        public DateTime Birthday { get; set; }
    }
}
