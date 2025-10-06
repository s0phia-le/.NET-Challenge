using System;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    // Handles database operations for Compensation entities
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext; // EF Core context for accessing the database

        public CompensationRepository(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        // Add a new Compensation record to the database
        public Compensation Add(Compensation compensation)
        {
            // Generate a new unique ID for the Compensation
            compensation.CompensationId = Guid.NewGuid().ToString();

            // Add Compensation to DbSet (not saved yet)
            _employeeContext.Compensations.Add(compensation);

            return compensation;
        }

        // Retrieve Compensation by the associated Employee's ID
        public Compensation GetById(string employeeId)
        {
            // Query Compensations, include Employee navigation property for full object graph
            return _employeeContext.Compensations
                .Where(c => c.Employee.EmployeeId == employeeId)
                .Include(c => c.Employee)
                .FirstOrDefault(); // Return null if not found
        }

        // Save changes asynchronously to the database
        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
