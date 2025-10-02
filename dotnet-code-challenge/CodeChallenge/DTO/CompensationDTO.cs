using System;
using System.ComponentModel.DataAnnotations;

namespace challenge.DTO
{
    public class CompensationDto
    {
        public String EmployeeID { get; set; }
        public decimal Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}