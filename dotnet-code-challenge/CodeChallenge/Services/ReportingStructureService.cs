using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;
using CodeChallenge.Models;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IEReportingStructureService
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeService> _logger;

        public ReportingStructureService(ILogger<EmployeeService> logger, IEmployeeService empService)
        {
            _employeeService = empService;
            _logger = logger;
        }

        public ReportingStructure Create(string id)
        {
            Employee e = _employeeService.GetById(id);
            return new ReportingStructure()
            {
                ParentEmployee = e,
                numberOfReports = 0
            };
        }
    }
}