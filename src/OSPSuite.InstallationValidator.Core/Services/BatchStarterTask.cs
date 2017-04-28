using System.IO;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.InstallationValidator.Core.Extensions;

namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface IBatchStarterTask
   {
      Task StartBatch(string outputFolderPath, CancellationToken cancellationToken);
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

      public Task StartBatch(string outputFolderPath, CancellationToken cancellationToken)
      {
         return Task.Run(() =>
         {
            var logFile = logFilePath(outputFolderPath);
            startBatchProcess(outputFolderPath, cancellationToken, logFile);
         }, cancellationToken);
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
         using (var watcher = _logWatcherFactory.CreateLogWatcher(logFile))
         {
            watcher.Watch();
            process.Start();
            process.Wait(cancellationToken);
         }
      }
   }
}