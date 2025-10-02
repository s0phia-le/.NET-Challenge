using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public int numberOfReports { get; set; }
        public Employee Manager { get; set; }
    }
}