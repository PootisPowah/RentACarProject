using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Dto;
using RentACarProject.Interfaces;
using RentACarProject.Models;
using RentACarProject.Repository;

namespace RentACarProject.Controllers
{
    /// <summary>
    /// Car controller.
    /// </summary>
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class CarController:Controller
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarController"/> class.
        /// </summary>
        /// <param name="carRepository">Car repository.</param>
        public CarController(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Returns list of cars.
        /// </summary>
        /// <returns>A list of cars.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/car
        /// </remarks>
        /// <response code="200">Returns a list of cars.</response>
        [HttpGet]
        [ProducesResponseType(200,Type=typeof(IEnumerable<Car>))]
        public IActionResult GetCars()
        {
            var cars = _mapper.Map<List<CarDto>>(_carRepository.GetCars());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(cars);
        }
        /// <summary>
        /// Get a car by ID.
        /// </summary>
        /// <param name="carId">The ID of the car.</param>
        /// <returns>The car with the specified ID.</returns>
        /// <response code="200">Returns the car with the specified ID.</response>
        /// <response code="404">If the car is not found.</response>
        [HttpGet("{carId}")]
        [ProducesResponseType(200, Type = typeof(Car))]
        [ProducesResponseType(404)]
        public IActionResult GetCarById(int carId)
        {
            if (!_carRepository.CarExists(carId))
            {
                return NotFound();
            }
            var car = _mapper.Map<CarDto>(_carRepository.GetCarById(carId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(car);
        }

        /// <summary>
        /// Create a new car.
        /// </summary>
        /// <param name="carCreate">The car to create.</param>
        /// <returns>A newly created car.</returns>
        /// <response code="201">Car created successfully.</response>
        /// <response code="400">If the car is null or model state is invalid.</response>
        /// <response code="422">If the car already exists.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateCar([FromBody] CarDto carCreate)
        {
            if (carCreate == null)
            {
                return BadRequest(ModelState);
            }
            var car = _carRepository.GetCars()
                .Where(c => c.Brand == carCreate.Brand && c.Model == carCreate.Model
                && c.Year == carCreate.Year
                && c.Horsepower == carCreate.Horsepower
                && c.IsAvailable == carCreate.IsAvailable).FirstOrDefault();

            if (car != null)
            {
                ModelState.AddModelError("", "Car already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carMap = _mapper.Map<Car>(carCreate);
            if (!_carRepository.CreateCar(carMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully created");
        }

        /// <summary>
        /// Update an existing car.
        /// </summary>
        /// <param name="carId">The ID of the car to update.</param>
        /// <param name="updatedCar">The updated car details.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Car updated successfully.</response>
        /// <response code="400">If the car is null or model state is invalid.</response>
        /// <response code="404">If the car is not found.</response>
        [HttpPut("{carId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCar(int carId, [FromBody] CarDto updatedCar)
        {
            if (updatedCar == null)
            {
                return BadRequest(ModelState);
            }

            if (!_carRepository.CarExists(carId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the existing car from the database
            var existingCar = _carRepository.GetCarById(carId);
            if (existingCar == null)
            {
                return NotFound();
            }

            // Map the updated properties from the DTO to the existing entity
            _mapper.Map(updatedCar, existingCar);

            // Update the car in the repository
            if (!_carRepository.UpdateCar(existingCar))
            {
                ModelState.AddModelError("", "Something went wrong updating car");
                return StatusCode(500, ModelState);
            }

            return NoContent(); // Return 204 No Content to indicate success
        }

        /// <summary>
        /// Delete a car.
        /// </summary>
        /// <param name="carId">The ID of the car to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Car deleted successfully.</response>
        /// <response code="400">If model state is invalid.</response>
        /// <response code="404">If the car is not found.</response>
        /// <response code="422">If the car cannot be deleted due to active rents.</response>
        [HttpDelete("{carId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public IActionResult DeleteCar(int carId)
        {
            if (!_carRepository.CarExists(carId))
            {
                return NotFound();
            }
            var customerToDelete = _carRepository.GetCarById(carId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_carRepository.DeleteCar(customerToDelete))
            {
                ModelState.AddModelError("", "Cannot delete car with active rents.");
                return StatusCode(422, ModelState);
            }
            return NoContent();
        }
    }
}
