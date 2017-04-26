using System.Threading;
using System.Threading.Tasks;

namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface IBatchComparisonTask {
      Task StartComparison(string folderPath, CancellationToken token);
   }

   public class BatchComparisonTask : IBatchComparisonTask
   {
      public Task StartComparison(string folderPath, CancellationToken token)
      {
         throw new System.NotImplementedException();
      }
   }
}