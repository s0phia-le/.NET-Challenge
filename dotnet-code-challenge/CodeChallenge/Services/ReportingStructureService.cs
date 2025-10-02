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
            var manager = _employeeService.GetById(id);
            if (manager == null) return null;
            var rcount = GetReportingCount(manager);

            return new ReportingStructure()
            {
                numberOfReports = rcount,
                Manager = manager
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