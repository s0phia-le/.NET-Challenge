using CodeChallenge.DTO;
using CodeChallenge.Models;

namespace CodeChallenge.Helpers
{
    // Interface for mapping between DTOs and domain models
    public interface IMapper
    {
        // Maps an EmployeeDto to an Employee domain object
        Employee EmployeeDTO_To_Employee(EmployeeDto empDTO);

        // Maps a CompensationDto to a Compensation domain object
        Compensation CompensationDTO_To_Compensation(CompensationDto compDTO);
    }
}