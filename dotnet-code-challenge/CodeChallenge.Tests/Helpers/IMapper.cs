using challenge.DTO;
using CodeChallenge.Models;

namespace CodeChallenge.Helpers
{
    public interface IMapper
    {
        Employee EmployeeDTO_To_Employee(EmployeeDto empDTO);
    }
}