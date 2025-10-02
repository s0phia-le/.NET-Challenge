using System;
using CodeChallenge.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        // Constructor injects logger and compensation service
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

            // Delegate creation to the compensation service
            Compensation newCompensation = _compensationService.Create(compensationDto);

            if (newCompensation == null)
                return NotFound();

            return CreatedAtRoute("getCompensationByEmployeeId",
                                  new { employeeId = newCompensation.Employee.EmployeeId },
                                  newCompensation);
        }

        // GET api/compensation/{employeeId}
        // Retrieves compensation for a given employee
        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(String employeeId)
        {
            _logger.LogDebug($"Received compensation get request for '{employeeId}'");

            // Retrieve the compensation record from the service
            var compensation = _compensationService.GetById(employeeId);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}
