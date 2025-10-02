using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetById(String employeeId);
        Compensation Add(Compensation compensation);
        Task SaveAsync();
    }
}