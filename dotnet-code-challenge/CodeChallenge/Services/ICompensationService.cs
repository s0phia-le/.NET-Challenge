using CodeChallenge.Models;
using System;
using CodeChallenge.DTO;

namespace CodeChallenge.Services
{
    // Interface defining operations for managing Compensations
    public interface ICompensationService
    {
        // Retrieves the Compensation for a given employee
        Compensation GetById(String id);
        // Creates a new Compensation record for an employee
        Compensation Create(CompensationDto compensationDto);
    }
}