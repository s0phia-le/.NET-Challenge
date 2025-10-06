using System.Collections.Generic;
using challenge.DTO;
using challenge.Models;
using challenge.Repositories;
using challenge.Services;
using Microsoft.Extensions.Logging;

namespace challenge.Helpers
{
    public class Mapper : IMapper
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<Mapper> _logger;

        public Mapper(ILogger<Mapper> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }
        
        public Employee EmployeeDto_To_Employee(EmployeeDto employeeDto)
        {
            // Create new employee without DirectReports
            var newEmployee = new Employee()
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Position = employeeDto.Position,
                Department = employeeDto.Department
            };
            
            // Add DirectReports to Employee if DTO has it
            if (employeeDto.DirectReports != null)
            {
                List<Employee> directReports = new List<Employee>(employeeDto.DirectReports.Count);
                
                foreach (var employeeId in employeeDto.DirectReports)
                {
                    var employeeRef = _employeeRepository.GetById(employeeId);
                    
                    if(employeeRef != null)
                        directReports.Add(employeeRef);
                }

                newEmployee.DirectReports = directReports;
                
                return newEmployee;
            }
            
            // Initialize empty DirectReports if DTO does specify it
            newEmployee.DirectReports = new List<Employee>();
            
            return newEmployee;
        }

        public Compensation CompensationDto_To_Compensation(CompensationDto compensationDto) 
        {
            var employee = _employeeRepository.GetById(compensationDto.EmployeeID);
            if (employee == null) return null;
                
            return new Compensation() {
                Employee = employee,
                EffectiveDate = compensationDto.EffectiveDate,
                Salary = compensationDto.Salary
            };
        }
    }
}