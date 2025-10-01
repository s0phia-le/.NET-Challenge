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
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeService empService)
        {
            _employeeService = empService;
            _logger = logger;
        }

        public ReportingStructure Create(string id)
        {
            var parentEmployee = _employeeService.GetById(id);
            var reportsCount = GetReportingCount(parentEmployee);

            return new ReportingStructure()
            {
                numberOfReports = reportsCount,
                Manager = parentEmployee
            };
        }

        private int GetReportingCount(Employee employee)
        {
            int rcount = employee.DirectReports.Count;
            foreach (var emp in employee.DirectReports)
            {
                Employee e = _employeeService.GetById(emp.EmployeeId);
                rcount += GetReportingCount(employee);
            }
            return rcount;
        }
    }
}