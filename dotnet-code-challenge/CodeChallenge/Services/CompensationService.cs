using System;
using CodeChallenge.DTO;
using CodeChallenge.Models;
using CodeChallenge.Repositories;
using CodeChallenge.Helpers;

namespace CodeChallenge.Services
{
    // Creates and retrieves employee compensations
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly IMapper _mapper;

        public CompensationService(ICompensationRepository compensationRepository, IMapper mapper)
        {
            _compensationRepository = compensationRepository;
            _mapper = mapper;
        }

        // Creates a new Compensation record for an employee
        public Compensation Create(CompensationDto compensationDto)
        {
            if (compensationDto == null) return null;

            // map DTO to entity
            var newCompensation = _mapper.CompensationDTO_To_Compensation(compensationDto);

            // No employee with that id found
            if (newCompensation == null) return null;

            // Persists the compensation
            var compensation = _compensationRepository.Add(newCompensation);
            _compensationRepository.SaveAsync().Wait();

            return compensation;
        }

        // Retrieves the compensation for a given employee 
        public Compensation GetById(string employeeId)
        {
            if (!String.IsNullOrEmpty(employeeId))
            {
                return _compensationRepository.GetById(employeeId);
            }

            return null;
        }
    }
}