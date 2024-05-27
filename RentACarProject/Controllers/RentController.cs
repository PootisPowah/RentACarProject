using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Dto;
using RentACarProject.Interfaces;
using RentACarProject.Models;
using RentACarProject.Repository;

namespace RentACarProject.Controllers
{
    /// <summary>
    /// Rent controller.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RentController : Controller
    {
        private readonly IRentRepository _rentRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="RentController"/> class.
        /// </summary>
        /// <param name="rentRepository">Rent repository</param>
        public RentController(IRentRepository rentRepository,
            ICustomerRepository customerRepository,
            ICarRepository carRepository,
            IMapper mapper)
        {
            _rentRepository = rentRepository;
            _customerRepository = customerRepository;
            _carRepository = carRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Returns a list of all rents.
        /// </summary>
        /// <returns>A list of rents.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/rent
        ///
        /// </remarks>
        /// <response code="200">Returns the list of rents.</response>
        /// <response code="400">If there is an error in the request.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Rent>))]
        [ProducesResponseType(400)]
        public IActionResult GetRents()
        {
            var rents = _mapper.Map<List<RentDto>>(_rentRepository.GetRents());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(rents);
        }
        [HttpGet("{rentId}")]
        public IActionResult GetByRentId(int rentId)
        {
            var rentDto = _mapper.Map<RentDto>(_rentRepository.GetByRentId(rentId));
            if (rentDto == null)
            {
                return NotFound(); // Return 404 Not Found if rent with given Id is not found
            }

            return Ok(rentDto); // Return 200 OK with the RentDto
        }
        /// <summary>
        /// Get a rent by customer ID and car ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="carId">The ID of the car.</param>
        /// <returns>The rent details for the specified customer and car.</returns>
        /// <response code="200">Returns the rent details.</response>
        /// <response code="400">If there is an error in the request.</response>
        /// <response code="404">If the rent is not found.</response>
        [HttpGet("{customerId}/{carId}")]
        [ProducesResponseType(200, Type = typeof(Rent))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult GetRent(int customerId, int carId)
        {
            var rent = _rentRepository.GetRent(customerId, carId);
            if (rent == null)
            {
                return NotFound();
            }

            var rentDto = _mapper.Map<RentDto>(rent);
            return Ok(rentDto);
        }



        /// <summary>
        /// Create a new rent.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="carId">The ID of the car.</param>
        /// <param name="rentCreate">The rent details.</param>
        /// <returns>The created rent details.</returns>
        /// <response code="201">Rent created successfully.</response>
        /// <response code="400">If the rent is null or there is an error in the request.</response>
        /// <response code="422">If the rent already exists.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateRent([FromBody] RentDto rentCreate)
        {
            if (rentCreate == null)
            {
                return BadRequest("Request data is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the car and customer exist and if the car is available
            var car = _rentRepository.GetCarById(rentCreate.CarId);
            if (car == null)
            {
                return BadRequest("Car not found.");
            }
            if (!car.IsAvailable)
            {
                return BadRequest("Car is not available for rent.");
            }

            var customer = _rentRepository.GetCustomerById(rentCreate.CustomerId);
            if (customer == null)
            {
                return BadRequest("Customer not found.");
            }

            // Check if rent already exists
            var existingRent = _rentRepository.GetRent(customer.Id, car.Id);
            if (existingRent != null)
            {
                ModelState.AddModelError("", "Rent already exists for this car and customer.");
                return StatusCode(422, ModelState);
            }

            // Prepare the Rent object
            var rent = new Rent
            {
                CustomerId = customer.Id,
                CarId = car.Id,
                StartDate = rentCreate.StartDate,
                EndDate = rentCreate.EndDate,
                RentPrice = rentCreate.RentPrice,
                PickupStreet = rentCreate.PickupStreet
            };

            // Attempt to create the rent
            bool result = _rentRepository.CreateRent(rent, customer, car);
            if (result)
            {
                return Ok("Rent created successfully.");
            }
            else
            {
                return BadRequest("Unable to create rent with provided customer and car details.");
            }
        }

        /// <summary>
        /// Update an existing rent.
        /// </summary>
        /// <param name="rentId">The ID of the customer.</param>
        /// <param name="updatedRent">The updated rent details.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Rent updated successfully.</response>
        /// <response code="400">If the rent is null or there is an error in the request.</response>
        /// <response code="404">If the rent is not found.</response>
        /// <response code="422">If there is a conflict while updating the rent.</response>
        [HttpPut("{rentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public IActionResult UpdateRent(int rentId, [FromBody] RentDto updatedRent)
        {
            if (updatedRent == null)
            {
                return BadRequest(ModelState);
            }

            var existingRent = _rentRepository.GetByRentId(rentId);
            if (existingRent == null)
            {
                return NotFound(); // Return 404 Not Found if rent with given Id is not found
            }

            // Manually map the properties that are allowed to be updated
            if (updatedRent.CustomerId.HasValue)
                existingRent.CustomerId = updatedRent.CustomerId.Value;
            else
                updatedRent.CustomerId = existingRent.CustomerId;

            if (updatedRent.CarId.HasValue)
                existingRent.CarId = updatedRent.CarId.Value;
            else
                updatedRent.CarId = existingRent.CarId;

            existingRent.StartDate = updatedRent.StartDate;
            existingRent.EndDate = updatedRent.EndDate;
            existingRent.RentPrice = updatedRent.RentPrice;
            existingRent.PickupStreet = updatedRent.PickupStreet;

            if (!_rentRepository.UpdateRent(existingRent))
            {
                ModelState.AddModelError("", "Something went wrong while updating the rent");
                return StatusCode(500, ModelState);
            }

            return NoContent(); // Return 204 No Content indicating successful update
        }



        /// <summary>
        /// Delete a rent.
        /// </summary>
        /// <param name="rentId">The ID of the rent.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Rent deleted successfully.</response>
        /// <response code="400">If there is an error in the request.</response>
        /// <response code="404">If the rent is not found.</response>
        /// <response code="500">If there is an error while deleting the rent.</response>
        [HttpDelete("{rentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteRent(int rentId)
        {
            if (!_rentRepository.RentExists(rentId))
            {
                return NotFound();
            }
            var rentToDelete = _rentRepository.GetByRentId(rentId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_rentRepository.DeleteRent(rentToDelete))
            {
                ModelState.AddModelError("", "Cannot delete rentToDelete with active rents.");
                return StatusCode(422, ModelState);
            }
            return NoContent();
        }
    }
}
