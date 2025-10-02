using System;
using challenge.DTO;
using challenge.Models;
using challenge.Repositories;
using CodeChallenge.Helpers;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly IMapper _mapper;

        public CompensationService(ICompensationRepository compensationRepository,IMapper mapper)
        {
            _compensationRepository = compensationRepository;
            _mapper = mapper;
        }

        public Compensation Create(CompensationDto compensationDto)
        {
            if (compensationDto == null) return null;
            
            var newCompensation = _mapper.CompensationDto_To_Compensation(compensationDto);
            
            // No employee with that id found
            if (newCompensation == null) return null;
                
            var compensation = _compensationRepository.Add(newCompensation);
            _compensationRepository.SaveAsync().Wait();
                
            return compensation;
        }

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