using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [Route("api/reporting-structure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        // Constructor injects logger and the service that calculates reporting structures
        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        // GET api/reporting-structure/{id}
        // Returns the ReportingStructure for the given employeeId
        [HttpGet("{id}", Name = "getEmployeeReportingStructureById")]
        public IActionResult GetEmployeeReportingStructureById(String id) 
        {
            _logger.LogDebug($"Received employee get reporting structure request for '{id}'");

            // Use the service to calculate the reporting structure
            ReportingStructure st = _reportingStructureService.Create(id);

            if (st == null) return NotFound();

            return Ok(st);
        }
    }
}
