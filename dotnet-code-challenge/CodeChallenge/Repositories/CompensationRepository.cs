using System;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    // Repository for managing Compensation entities in the database
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;

        public CompensationRepository(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        // Adds a new Compensation to the context. CompensationId is generated here
        public Compensation Add(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        // Retrieves a Compensation by employee ID
        public Compensation GetById(string employeeId)
        {
            return _employeeContext.Compensations.Where(c => c.Employee.EmployeeId == employeeId)
                .Include(c => c.Employee)
                .FirstOrDefault();
        }

        // Persists changes made to the context asynchronously
        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}