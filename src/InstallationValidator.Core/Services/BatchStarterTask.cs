using System.IO;
using System.Threading;
using System.Threading.Tasks;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility;

namespace InstallationValidator.Core.Services
{
   public interface IBatchStarterTask
   {
      Task<ValidationRunSummary> StartBatch(string outputFolderPath, CancellationToken cancellationToken);
   }

   public class BatchStarterTask : IBatchStarterTask
   {
      private readonly IInstallationValidatorConfiguration _applicationConfiguration;
      private readonly IStartableProcessFactory _startableProcessFactory;
      private readonly ILogWatcherFactory _logWatcherFactory;

      public BatchStarterTask(IInstallationValidatorConfiguration applicationConfiguration, IStartableProcessFactory startableProcessFactory, ILogWatcherFactory logWatcherFactory)
      {
         _applicationConfiguration = applicationConfiguration;
         _startableProcessFactory = startableProcessFactory;
         _logWatcherFactory = logWatcherFactory;
      }

      private string logFilePath(string basePath) => Path.Combine(basePath, Constants.Tools.BATCH_LOG);

      public async Task<ValidationRunSummary> StartBatch(string outputFolderPath, CancellationToken cancellationToken)
      {
         var batchRunResult = new ValidationRunSummary
         {
            MoBiVersion = moBiVersion(),
            PKSimVersion = pkSimVersion(),
            OutputFolder = outputFolderPath,
            InputFolder = _applicationConfiguration.BatchInputsFolderPath
         };

         DirectoryHelper.CreateDirectory(outputFolderPath);

         return await Task.Run(() =>
         {
            var logFile = logFilePath(outputFolderPath);
            startBatchProcess(outputFolderPath, cancellationToken, logFile);
            return batchRunResult;
         }, cancellationToken);
      }

      private string moBiVersion()
      {
         return versionForPath(_applicationConfiguration.MoBiBinaryExecutablePath);
      }

      private string pkSimVersion()
      {
         return versionForPath(_applicationConfiguration.PKSimBinaryExecutablePath);
      }

      private static string versionForPath(string binaryExecutablePath)
      {
         return FileHelper.GetVersion(binaryExecutablePath);
      }

      private void startBatchProcess(string outputFolderPath, CancellationToken cancellationToken, string logFile)
      {
         var args = new[]
         {
            "-i",
            _applicationConfiguration.BatchInputsFolderPath.InQuotes(),
            "-o",
            outputFolderPath.InQuotes(),
            "-l",
            logFile.InQuotes(),
         };

         using (var process = _startableProcessFactory.CreateStartableProcess(_applicationConfiguration.PKSimBatchToolPath, args))
         using (var watcher = _logWatcherFactory.CreateLogWatcher(logFile, new [] {outputFolderPath}))
         {
            watcher.Watch();
            process.Start();
            process.Wait(cancellationToken);
         }
      }
   }
}