using CodeChallenge.DTO;
using CodeChallenge.Models;
using CodeChallenge.Models;

namespace CodeChallenge.Helpers
{
    public interface IMapper
    {
        Employee EmployeeDTO_To_Employee(EmployeeDto empDTO);
        Compensation CompensationDTO_To_Compensation(CompensationDto compDTO);
    }
}