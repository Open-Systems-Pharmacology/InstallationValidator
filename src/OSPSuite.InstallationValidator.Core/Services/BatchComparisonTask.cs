using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface IBatchComparisonTask
   {
      Task StartComparison(string folderPath, CancellationToken token);
      Task StartComparison(string folderPath1, string folderPath2, CancellationToken token);
   }

   public class BatchComparisonTask : IBatchComparisonTask
   {
      private readonly IInstallationValidatorConfiguration _validatorConfiguration;

      public BatchComparisonTask(IInstallationValidatorConfiguration validatorConfiguration)
      {
         _validatorConfiguration = validatorConfiguration;
      }

      public Task StartComparison(string folderPath, CancellationToken token)
      {
         return Task.FromResult(false);
      }

      public Task StartComparison(string folderPath1, string folderPath2, CancellationToken token)
      {
         throw new System.NotImplementedException();
      }
   }
}