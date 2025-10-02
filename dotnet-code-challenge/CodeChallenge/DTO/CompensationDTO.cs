using System;
using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.DTO
{
    public class CompensationDto
    {
        public String EmployeeID { get; set; }
        public decimal Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}