using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    // Interface defining operations for managing Compensations
    public interface ICompensationRepository
    {
        // Retrieves a Compensation record for an employee 
        Compensation GetById(String employeeId);
        // Adds a new Compensation record to the repository
        Compensation Add(Compensation compensation);
        // Persists all changes made in the repository asynchronously
        Task SaveAsync();
    }
}