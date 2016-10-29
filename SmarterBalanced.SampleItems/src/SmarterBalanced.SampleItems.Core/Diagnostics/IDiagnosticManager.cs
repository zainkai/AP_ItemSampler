using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Diagnostics
{
    public interface IDiagnosticManager
    {
        Task<string> GetDiagnosticStatusesAsync(int level);

        Task<string> GetDiagnosticStatusAsync(int level);

    }
}
