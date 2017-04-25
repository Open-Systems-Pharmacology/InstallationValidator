using System.IO;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.Utility;

namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface IBatchStarterTask
   {
      StartableProcess StartValidation(string outputFolderPath);
      void StopValidation(StartableProcess process);
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

      public StartableProcess StartValidation(string outputFolderPath)
      {
         var process = _startableProcessFactory.CreateStartableProcess(batchToolPath, $"-i \"{inputFolder}\" -o \"{outputFolderPath}\" -l \"{logFilePath(outputFolderPath)}\"");
         process.Start();
         return process;
      }

      public void StopValidation(StartableProcess process)
      {
         process?.Stop();
      }
   }
}
