using challenge.Models;
using System;
using challenge.DTO;

namespace challenge.Services
{
    public interface ICompensationService
    {
        Compensation GetById(String id);
        Compensation Create(CompensationDto compensationDto);
    }
}
