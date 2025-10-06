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
    [Route("api/reporting-structure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        [HttpGet("{id}", Name = "getEmployeeReportingStructureById")]
        public IActionResult GetEmployeeReportingStructureById(String id)
        {
            _logger.LogDebug($"Received employee get reporting Structure for '{id}'");
            
            ReportingStructure structure = _reportingStructureService.Create(id);

            if (structure == null)
                return NotFound();

            return Ok(structure);
        }
    }
}
