using System.Collections.Generic;
using CodeChallenge.DTO;
using CodeChallenge.Models;
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

        public Mapper(ILogger<Mapper> logger, IEmployeeRepository empRepo)
        {
            _employeeRepository = empRepo;
            _logger = logger;
        }

        public Employee EmployeeDTO_To_Employee(EmployeeDto empDTO)
        {
            var newEmployee = new Employee()
            {
                FirstName = empDTO.FirstName,
                LastName = empDTO.LastName,
                Position = empDTO.Position,
                Department = empDTO.Department
            };

            if (empDTO.DirectReports != null)
            {
                List<Employee> reports = new List<Employee>(empDTO.DirectReports.Count);
                foreach (var empID in empDTO.DirectReports)
                {
                    var empRef = _employeeRepository.GetById(empID);
                    if (empRef != null) reports.Add(empRef);
                }

                newEmployee.DirectReports = reports;
                return newEmployee;
            }
            newEmployee.DirectReports = new List<Employee>();
            return newEmployee;
        }

        public Compensation CompensationDTO_To_Compensation(CompensationDto compDTO)
        {
            var employee = _employeeRepository.GetById(compDTO.EmployeeID);
            if (employee == null) return null;

            return new Compensation()
            {
                Employee = employee,
                EffectiveDate = compDTO.EffectiveDate,
                Salary = compDTO.Salary
            };
        }
    }
}
