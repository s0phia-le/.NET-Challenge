using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    // Route all requests starting with /api/reporting-structure to this controller
    [Route("api/reporting-structure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;  // Logger for debug/info messages
        private readonly IReportingStructureService _reportingStructureService; // Service to handle reporting structure logic

        // Constructor injects logger and reporting structure service
        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        // GET /api/reporting-structure/{id}
        // Retrieve the reporting structure for an employee by ID
        [HttpGet("{id}", Name = "getEmployeeReportingStructureById")]
        public IActionResult GetEmployeeReportingStructureById(String id)
        {
            _logger.LogDebug($"Received employee get reporting structure request for '{id}'");
            
            // Call service to compute or fetch reporting structure
            ReportingStructure structure = _reportingStructureService.Create(id);

            // Return 404 if employee or structure not found
            if (structure == null)
                return NotFound();

            // Return 200 OK with the reporting structure
            return Ok(structure);
        }
    }
}
