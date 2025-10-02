using System.Collections.Generic;
using CodeChallenge.DTO;
using CodeChallenge.Models;
using CodeChallenge.Repositories;
using CodeChallenge.Services;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Helpers
{
    public class Mapper : IMapper
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<Mapper> _logger;

        // Constructor injects the employee repository and logger
        public Mapper(IEmployeeRepository empRepo, ILogger<Mapper> logger)
        {
            _employeeRepository = empRepo;
            _logger = logger;
        }

        // Converts an EmployeeDto into an Employee domain object.
        public Employee EmployeeDTO_To_Employee(EmployeeDto empDTO)
        {
            var newEmployee = new Employee()
            {
                FirstName = empDTO.FirstName,
                LastName = empDTO.LastName,
                Position = empDTO.Position,
                Department = empDTO.Department
            };

            // Convert DirectReports from list of IDs to list of Employee objects
            if (empDTO.DirectReports != null)
            {
                List<Employee> reports = new List<Employee>(empDTO.DirectReports.Count);
                foreach (var empID in empDTO.DirectReports)
                {
                    var empRef = _employeeRepository.GetById(empID);
                    if (empRef != null)
                        reports.Add(empRef);
                }

                newEmployee.DirectReports = reports;
                return newEmployee;
            }

            // No direct reports --> initialize empty list
            newEmployee.DirectReports = new List<Employee>();
            return newEmployee;
        }

        // Converts a CompensationDto into a Compensation domain object.
        public Compensation CompensationDTO_To_Compensation(CompensationDto compDTO)
        {
            // Look up the employee by ID
            var employee = _employeeRepository.GetById(compDTO.EmployeeID);
            if (employee == null) 
                return null; 

            // Create Compensation object
            return new Compensation()
            {
                Employee = employee,
                EffectiveDate = compDTO.EffectiveDate,
                Salary = compDTO.Salary
            };
        }
    }
}
