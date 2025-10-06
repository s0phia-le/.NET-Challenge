using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            _logger = logger;
        }
        
        public ReportingStructure Create(string id)
        {
            var manager = _employeeService.GetById(id);
            
            // Return null if no employee found
            if (manager == null) return null;
            
            // Recursively Calculates Direct Reports Count
            var reportsCount = GetReportingCount(manager);
            
            return new ReportingStructure()
            {  
                NumberOfReports = reportsCount,
                Manager = manager
            };
        }

        private int GetReportingCount(Employee e)
        {
            int directReportsCount = e.DirectReports.Count;
            
            foreach (var employeeRef in e.DirectReports)
            {
                // Get employee with DirectReports references
                Employee employee = _employeeService.GetById(employeeRef.EmployeeId);
                
                // Recursive Call to add DirectReports of child
                directReportsCount += GetReportingCount(employee);
            }
            
            return directReportsCount;
        }
        
    }
}
