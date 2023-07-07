using System;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repository;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _Customer;

        public CustomerController(ICustomerRepository Customer)
        {
            _Customer = Customer ?? throw new ArgumentNullException(nameof(Customer));
        }

        [HttpGet]
        [Route("GetCustomers")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _Customer.GetCustomers());
        }
         
        [HttpPost]
        [Route("AddCustomer")]
        public async Task<IActionResult> Post(List<Customer> customers)
        { 
            var result = await _Customer.InsertCustomers(customers);
            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");
            } 
            return Ok("Added Successfully");
        }
    }
}
