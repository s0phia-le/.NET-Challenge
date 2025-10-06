using challenge.DTO;
using challenge.Models;

namespace challenge.Helpers
{
    public interface IMapper
    {
        Employee EmployeeDto_To_Employee(EmployeeDto employeeDto);
        Compensation CompensationDto_To_Compensation(CompensationDto compensationDto);
    }
}