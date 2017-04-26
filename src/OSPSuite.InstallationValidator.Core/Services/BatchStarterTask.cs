using System.IO;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Extensions;
using OSPSuite.Utility;

namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface IBatchStarterTask
   {
      Task StartBatch(string outputFolderPath, CancellationToken cancellationToken);
   }

   public class BatchStarterTask : IBatchStarterTask
   {
      private readonly IInstallationValidationConfiguration _applicationConfiguration;
      private readonly IStartableProcessFactory _startableProcessFactory;

      public BatchStarterTask(IInstallationValidationConfiguration applicationConfiguration, IStartableProcessFactory startableProcessFactory)
      {
         _applicationConfiguration = applicationConfiguration;
         _startableProcessFactory = startableProcessFactory;
      }

      private string batchToolPath => Path.Combine(baseInstallPath, Constants.Tools.PKSimBatchTool);

      private string baseInstallPath => FileHelper.FolderFromFileFullPath(_applicationConfiguration.PKSimPath);

      private string inputFolder => Path.Combine(baseInstallPath, Constants.Tools.BatchInputs);

      private string logFilePath(string basePath) => Path.Combine(basePath, Constants.Tools.BatchLog);

      public Task StartBatch(string outputFolderPath, CancellationToken cancellationToken)
      {
         return Task.Run(() =>
         {
            using (var process = _startableProcessFactory.CreateStartableProcess(batchToolPath, "-i", inputFolder.SurroundWith("\""), "-o", outputFolderPath.SurroundWith("\""), " -l", logFilePath(outputFolderPath).SurroundWith("\"")))
            {
               process.Start();
               process.Wait(cancellationToken);
            }
         }, cancellationToken);
      }


   }
}
