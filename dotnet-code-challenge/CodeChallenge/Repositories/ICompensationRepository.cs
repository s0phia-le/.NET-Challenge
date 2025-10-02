using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetById(String employeeId);
        Compensation Add(Compensation compensation);
        Task SaveAsync();
    }
}