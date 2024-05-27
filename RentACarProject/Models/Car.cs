using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentACarProject.Models
{
  
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Brand { get; set; }

      
        [Required]
        [StringLength(20)]
        public string Model { get; set; }

        
        [Required]
        [StringLength(4)]
        public string Year { get; set; }

       
        [Required]
        public double Horsepower { get; set; }

       
        [Required]  
        public bool IsAvailable { get; set; }

       
    }
}
