using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class ReportingStructure
    {
        public int NumberOfReports { get; set; }
        public Employee Manager { get; set; }
    }
}
