using CodeChallenge.Models;
using System;
using CodeChallenge.DTO;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        Compensation GetById(String id);
        Compensation Create(CompensationDto compensationDto);
    }
}