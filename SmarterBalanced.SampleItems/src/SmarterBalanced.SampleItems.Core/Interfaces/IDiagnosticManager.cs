using SmarterBalanced.SampleItems.Core.Models.DiagnosticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Interfaces
{
    public interface IDiagnosticManager
    {
        Task<string> GetDiagnosticStatusesAsync(int level);

        Task<string> GetDiagnosticStatusAsync(int level);

    }
}
