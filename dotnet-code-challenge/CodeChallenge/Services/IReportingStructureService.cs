using CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    // Interface defining operations for managing ReportingStructures
    public interface IReportingStructureService
    {
        // Creates a ReportingStructure for a given employee
        ReportingStructure Create(String id);
    }
}