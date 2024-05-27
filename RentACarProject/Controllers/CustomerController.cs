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
    /// Customer controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]

    public class CustomerController: Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="customerRepository">Customer repository.</param>
        public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Returns a list of customers.
        /// </summary>
        /// <returns>A list of customers.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/customer
        /// </remarks>
        /// <response code="200">Returns a list of customers.</response>
        /// <response code="400">If there is an error in the request.</response>
        [HttpGet]
        [ProducesResponseType(200, Type =typeof(IEnumerable<Customer>))]
        [ProducesResponseType(400)]
        public IActionResult GetCustomers()
        {
            var customers = _mapper.Map<List<CustomerDto>>(_customerRepository.GetCustomers());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(customers);
        }
        /// <summary>
        /// Get a customer by ID.
        /// </summary>
        /// <param name="custId">The ID of the customer.</param>
        /// <returns>The customer with the specified ID.</returns>
        /// <response code="200">Returns the customer with the specified ID.</response>
        /// <response code="400">If there is an error in the request.</response>
        /// <response code="404">If the customer is not found.</response>
        [HttpGet("{custId}")]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerById(int custId)
        {
            if (!_customerRepository.CustomerExists(custId))
            {
                return NotFound();
            }
            var customer  = _mapper.Map<CustomerDto>(_customerRepository.GetCustomerById(custId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(customer);
        }
        ///// <summary>
        ///// Get a customer by first name.
        ///// </summary>
        ///// <param name="fname">The first name of the customer.</param>
        ///// <returns>The customer with the specified first name.</returns>
        ///// <response code="200">Returns the customer with the specified first name.</response>
        ///// <response code="400">If there is an error in the request.</response>
        ///// <response code="404">If the customer is not found.</response>
        //[HttpGet("{fname}")]
        //[ProducesResponseType(200, Type = typeof(Customer))]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //public IActionResult GetCustomerByName(string fname)
        //{
        //    if (!_customerRepository.CustomerExistsByName(fname))
        //    {
        //        return NotFound();
        //    }
        //    var customers = _mapper.Map<CustomerDto>(_customerRepository.GetCustomerByName(fname));
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    return Ok(customers);
        //}

        /// <summary>
        /// Create a new customer.
        /// </summary>
        /// <param name="customerCreate">The customer to create.</param>
        /// <returns>A newly created customer.</returns>
        /// <response code="201">Customer created successfully.</response>
        /// <response code="400">If the customer is null or model state is invalid.</response>
        /// <response code="422">If the customer already exists.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateCustomer([FromBody] CustomerDto customerCreate)
        {
            if(customerCreate == null)
            {
                return BadRequest(ModelState);
            }
            var customer = _customerRepository.GetCustomers()
                .Where(c => c.Fname.ToUpper() == customerCreate.Fname.ToUpper() 
                && c.Lname.ToUpper() == customerCreate.Lname.ToUpper()
                && c.Gender.ToUpper() == customerCreate.Gender.ToUpper()
                && c.Age == customerCreate.Age
                && c.Salary == customerCreate.Salary
                && c.Birthday == customerCreate.Birthday).FirstOrDefault();

            if(customer != null)
            {
                ModelState.AddModelError("", "Customer already exists");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerMap = _mapper.Map<Customer>(customerCreate);
            if (!_customerRepository.CreateCustomer(customerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully created");
        }

        /// <summary>
        /// Update an existing customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer to update.</param>
        /// <param name="updatedCustomer">The updated customer details.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Customer updated successfully.</response>
        /// <response code="400">If the customer is null or model state is invalid.</response>
        /// <response code="404">If the customer is not found.</response>
        [HttpPut("{customerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult UpdateCustomer(int customerId, [FromBody] CustomerDto updatedCustomer)
        {
            if(updatedCustomer == null)
            {
                return BadRequest(ModelState);

            }
            
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingCustomer = _customerRepository.GetCustomerById(customerId);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            // Map the updated properties from the DTO to the existing entity
            _mapper.Map(updatedCustomer, existingCustomer);
            if (!_customerRepository.UpdateCustomer(existingCustomer))
            {
                ModelState.AddModelError("", "Something went wrong updating customer");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        /// <summary>
        /// Delete a customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Customer deleted successfully.</response>
        /// <response code="400">If model state is invalid.</response>
        /// <response code="404">If the customer is not found.</response>
        /// <response code="422">If the customer cannot be deleted due to active rents.</response>
        [HttpDelete("{customerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public IActionResult DeleteCustomer(int customerId)
        {
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }
            var customerToDelete = _customerRepository.GetCustomerById(customerId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!_customerRepository.DeleteCustomer(customerToDelete))
            {
                    ModelState.AddModelError("", "Cannot delete customer with active rents.");
                    return StatusCode(422, ModelState);
            }
            return NoContent();
        }
    }
}
