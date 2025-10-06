using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    // Service layer handles reporting structure logic
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeService _employeeService; // To fetch Employee objects
        private readonly ILogger<ReportingStructureService> _logger; // Logger for debug/info

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            _logger = logger;
        }
        
        // Create ReportingStructure for a manager by employee ID
        public ReportingStructure Create(string id)
        {
            var manager = _employeeService.GetById(id);
            
            // Return null if no employee found
            if (manager == null) return null;
            
            // Recursively calculate total number of direct and indirect reports
            var reportsCount = GetReportingCount(manager);
            
            return new ReportingStructure()
            {  
                NumberOfReports = reportsCount,
                Manager = manager
            };
        }

        // Recursively count all direct and indirect reports for an employee
        private int GetReportingCount(Employee e)
        {
            int directReportsCount = e.DirectReports.Count; // Start with immediate direct reports
            
            foreach (var employeeRef in e.DirectReports)
            {
                // Fetch full Employee object to access their DirectReports
                Employee employee = _employeeService.GetById(employeeRef.EmployeeId);
                
                // Recursive call to include reports of child employees
                directReportsCount += GetReportingCount(employee);
            }
            
            return directReportsCount;
        }
    }
}
