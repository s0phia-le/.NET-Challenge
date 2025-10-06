using System.Collections.Generic;
using challenge.DTO;
using challenge.Models;
using challenge.Repositories;
using challenge.Services;
using Microsoft.Extensions.Logging;

namespace challenge.Helpers
{
    // Mapper handles conversion between DTOs and domain models
    public class Mapper : IMapper
    {
        private readonly IEmployeeRepository _employeeRepository; // To fetch employee references for DirectReports or Compensation
        private readonly ILogger<Mapper> _logger;                 // Logger to track mapping issues or debug info

        public Mapper(ILogger<Mapper> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }
        
        // Convert EmployeeDto to Employee model
        public Employee EmployeeDto_To_Employee(EmployeeDto employeeDto)
        {
            // Create new Employee without setting DirectReports yet
            var newEmployee = new Employee()
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Position = employeeDto.Position,
                Department = employeeDto.Department
            };
            
            // If DTO has DirectReports, resolve each ID to an Employee reference
            if (employeeDto.DirectReports != null)
            {
                List<Employee> directReports = new List<Employee>(employeeDto.DirectReports.Count);
                
                foreach (var employeeId in employeeDto.DirectReports)
                {
                    var employeeRef = _employeeRepository.GetById(employeeId);
                    
                    // Only add valid references
                    if(employeeRef != null)
                        directReports.Add(employeeRef);
                }

                newEmployee.DirectReports = directReports;
                
                return newEmployee;
            }
            
            // Initialize empty DirectReports if none provided in DTO
            newEmployee.DirectReports = new List<Employee>();
            
            return newEmployee;
        }

        // Convert CompensationDto to Compensation model
        public Compensation CompensationDto_To_Compensation(CompensationDto compensationDto) 
        {
            // Fetch employee by ID; return null if not found
            var employee = _employeeRepository.GetById(compensationDto.EmployeeID);
            if (employee == null) return null;
                
            // Create new Compensation object
            return new Compensation() {
                Employee = employee,
                EffectiveDate = compensationDto.EffectiveDate,
                Salary = compensationDto.Salary
            };
        }
    }
}
