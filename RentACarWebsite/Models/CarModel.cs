using System.ComponentModel.DataAnnotations;

public class CarModel
{

    public int Id { get; set; }

    [Required]
    [StringLength(20)] 
    public string Brand { get; set; }

    [Required]
    [StringLength(20)] 
    public string Model { get; set; }

    [Required]
    [Range(2000, 2024, ErrorMessage = "Year must be between 2000 and 2024.")]
    public string Year { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Horsepower must be a positive number.")]
    public double Horsepower { get; set; }

    public bool IsAvailable { get; set; }
}
