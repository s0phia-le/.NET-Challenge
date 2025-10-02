using System;
using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        [Key]
        public String CompensationId { get; set; }
        [Required]
        public Employee Employee { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public DateTime EffectiveDate { get; set; }
    }
}