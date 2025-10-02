using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    // Computes reporting structures for employees
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeService empService)
        {
            _employeeService = empService;
            _logger = logger;
        }

        // Creates a ReportingStructure for an employee
        public ReportingStructure Create(string id)
        {
            var manager = _employeeService.GetById(id);
            if (manager == null) return null;
            var rcount = GetReportingCount(manager);

            return new ReportingStructure()
            {
                numberOfReports = rcount,
                Manager = manager
            };
        }

        // Recursively counts all direct and indirect reports for a given employee
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