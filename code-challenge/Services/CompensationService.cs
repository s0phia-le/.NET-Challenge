using System;
using challenge.DTO;
using challenge.Models;
using challenge.Repositories;
using challenge.Helpers;

namespace challenge.Services
{
    // Service layer handles business logic for Compensation
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository; // Repository for DB access
        private readonly IMapper _mapper; // Mapper to convert DTOs to domain models

        public CompensationService(ICompensationRepository compensationRepository, IMapper mapper)
        {
            _compensationRepository = compensationRepository;
            _mapper = mapper;
        }

        // Create a new Compensation from DTO
        public Compensation Create(CompensationDto compensationDto)
        {
            // Return null if input is invalid
            if (compensationDto == null) return null;
            
            // Convert DTO to Compensation model
            var newCompensation = _mapper.CompensationDto_To_Compensation(compensationDto);
            
            // Return null if Employee not found or mapping failed
            if (newCompensation == null) return null;
                
            // Add to repository and persist changes
            var compensation = _compensationRepository.Add(newCompensation);
            _compensationRepository.SaveAsync().Wait(); // Wait for async save to complete
                
            return compensation;
        }

        // Retrieve Compensation by Employee ID
        public Compensation GetById(string employeeId)
        {
            if (!String.IsNullOrEmpty(employeeId))
            {
                return _compensationRepository.GetById(employeeId);
            }

            // Return null if employeeId is invalid
            return null;
        }
    }
}
