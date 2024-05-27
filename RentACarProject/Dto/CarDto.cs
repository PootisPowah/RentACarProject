using System.ComponentModel.DataAnnotations;

namespace RentACarProject.Dto
{
    /// <summary>
    /// Data transfer object for representing car information.
    /// </summary>
    public class CarDto
    {
        private string _brand;
        private string _model;

        /// <summary>
        /// Gets or sets the brand of the car.
        /// </summary>
        /// 
        public int id { get; set; }
        [StringLength(20)]
        public string Brand
        {
            get => _brand;
            set => _brand = value != null ? char.ToUpper(value[0]) + value.Substring(1).ToLower() : null;
        }

        /// <summary>
        /// Gets or sets the model of the car.
        /// </summary>
        [StringLength(20)]
        public string Model
        {
            get => _model;
            set => _model = value != null ? char.ToUpper(value[0]) + value.Substring(1).ToLower() : null;
        }

        /// <summary>
        /// Gets or sets the year of the car.
        /// </summary>
        [StringLength(4)]
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets the horsepower of the car.
        /// </summary>
        public double Horsepower { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the car is available.
        /// </summary>
        public bool IsAvailable { get; set; }
    }
}
