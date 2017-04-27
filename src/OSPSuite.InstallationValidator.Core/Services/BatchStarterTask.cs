﻿using System.IO;
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
      private readonly ILogWatcherFactory _logWatcherFactory;

      public BatchStarterTask(IInstallationValidationConfiguration applicationConfiguration, IStartableProcessFactory startableProcessFactory, ILogWatcherFactory logWatcherFactory)
      {
         _applicationConfiguration = applicationConfiguration;
         _startableProcessFactory = startableProcessFactory;
         _logWatcherFactory = logWatcherFactory;
      }

      private string batchToolPath => Path.Combine(pkSimInstallPath, Constants.Tools.PKSimBatchTool);

      private string pkSimInstallPath => FileHelper.FolderFromFileFullPath(_applicationConfiguration.PKSimPath);

      private string inputFolder => Path.Combine(pkSimInstallPath, Constants.Tools.BatchInputs);

      private string logFilePath(string basePath) => Path.Combine(basePath, Constants.Tools.BatchLog);

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
         using (var process = _startableProcessFactory.CreateStartableProcess(batchToolPath, "-i", inputFolder.SurroundWith("\""), "-o", outputFolderPath.SurroundWith("\""), " -l", logFile.SurroundWith("\"")))
         using (var watcher = _logWatcherFactory.CreateLogWatcher(logFile))
         {
            watcher.Watch();
            process.Start();
            process.Wait(cancellationToken);
         }
      }
   }
}