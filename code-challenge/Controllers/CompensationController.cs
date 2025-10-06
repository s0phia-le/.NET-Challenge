using System;
using challenge.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    // Route prefix for all actions in this controller
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        // Constructor injects logger and service for handling compensation logic
        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        // POST api/compensation
        // Creates a new compensation record for an employee
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] CompensationDto compensationDto)
        {
            _logger.LogDebug($"Received compensation create request for '{compensationDto.EmployeeID}'");

            // Delegate creation logic to service layer
            Compensation newCompensation = _compensationService.Create(compensationDto);

            // If creation fails (e.g., employee not found), return 404
            if (newCompensation == null)
                return NotFound();

            // Return 201 Created with route to fetch this compensation
            return CreatedAtRoute(
                "getCompensationByEmployeeId", 
                new { employeeId = newCompensation.Employee.EmployeeId }, 
                newCompensation
            );
        }

        // GET api/compensation/{employeeId}
        // Retrieves a compensation record for a given employee
        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(String employeeId)
        {
            _logger.LogDebug($"Received compensation get request for '{employeeId}'");

            // Use service layer to fetch compensation
            var compensation = _compensationService.GetById(employeeId);

            // If not found, return 404
            if (compensation == null)
                return NotFound();

            // Return 200 OK with compensation data
            return Ok(compensation);
        }
    }
}
