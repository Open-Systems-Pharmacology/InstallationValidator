using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Events;
using InstallationValidator.Core.Extensions;
using InstallationValidator.Core.Presentation.DTO;
using InstallationValidator.Core.Presentation.Views;
using InstallationValidator.Core.Services;
using Newtonsoft.Json;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace InstallationValidator.Core.Presentation
{
   public interface ISimulationComparisonPresenter : IDisposablePresenter, ILoggerPresenter
   {
      Task StartComparison();
      void Abort();
      void SelectFirstFolder();
      void SelectSecondFolder();
      void SelectExclusionList();
   }

   public class SimulationComparisonPresenter : AbstractDisposablePresenter<ISimulationComparisonView, ISimulationComparisonPresenter>, ISimulationComparisonPresenter
   {
      private readonly IInstallationValidatorConfiguration _configuration;
      private readonly IDialogCreator _dialogCreator;
      private CancellationTokenSource _cancellationTokenSource;
      private bool _comparisonRunning;
      private readonly IBatchComparisonTask _batchComparisonTask;
      private readonly IValidationReportingTask _validationReportingTask;
      private readonly FolderComparisonDTO _folderComparisonDTO;

      public SimulationComparisonPresenter(ISimulationComparisonView view, IInstallationValidatorConfiguration configuration, IDialogCreator dialogCreator,
         IBatchComparisonTask batchComparisonTask, IValidationReportingTask validationReportingTask) : base(view)
      {
         _configuration = configuration;
         _dialogCreator = dialogCreator;
         _batchComparisonTask = batchComparisonTask;
         _validationReportingTask = validationReportingTask;
         _folderComparisonDTO = new FolderComparisonDTO();
         view.BindTo(_folderComparisonDTO);
      }

      public void Handle(AppendTextToLogEvent eventToHandle)
      {
         this.LogText(eventToHandle.Text, eventToHandle.IsHtml);
      }

      public void Handle(AppendLineToLogEvent eventToHandle)
      {
         this.LogLine(eventToHandle.Line, eventToHandle.IsHtml);
      }

      public async Task StartComparison()
      {
         _cancellationTokenSource = new CancellationTokenSource();
         try
         {
            updateComparisonRunningState(running: true);
            this.ResetLog();

            this.LogLine(Logs.StartingComparison);
            var comparisonResult = await _batchComparisonTask.StartComparison(comparisonSettingsFromDTO(), _cancellationTokenSource.Token);
            this.LogLine();

            this.LogLine(Logs.StartingReport);
            await _validationReportingTask.CreateReport(comparisonResult, _folderComparisonDTO.FirstFolder.FolderPath, _folderComparisonDTO.SecondFolder.FolderPath, openReport: true);
            this.LogLine();

            this.LogLine(Logs.ComparisonCompleted);
         }
         catch (OperationCanceledException)
         {
            this.LogLine(Captions.TheComparisonWasCanceled);
         }
         catch (Exception e)
         {
            this.LogException(e, _configuration.IssueTrackerUrl);
         }
         finally
         {
            updateComparisonRunningState(running: false);
         }
      }

      private ComparisonSettings comparisonSettingsFromDTO()
      {
         return new ComparisonSettings
         {
            FolderPath1 = _folderComparisonDTO.FirstFolder.FolderPath,
            FolderPath2 = _folderComparisonDTO.SecondFolder.FolderPath,
            NumberOfCurves = _folderComparisonDTO.NumberOfCurves,
            IgnoreAddedCurves = _folderComparisonDTO.IgnoreAddedCurves,
            IgnoreRemovedCurves = _folderComparisonDTO.IgnoreRemovedCurves,
            Exclusions = exclusionListFrom(_folderComparisonDTO.ExclusionFile),
         };
      }

      private void updateComparisonRunningState(bool running)
      {
         _comparisonRunning = running;
         View.ComparisonIsRunning(_comparisonRunning);
      }

      public void Abort()
      {
         if (!_comparisonRunning)
            return;

         if (_dialogCreator.MessageBoxYesNo(Captions.ReallyCancelFolderComparison) == ViewResult.No)
            return;

         _cancellationTokenSource?.Cancel();
      }

      public void SelectFirstFolder()
      {
         selectFolder(_folderComparisonDTO.FirstFolder);
      }

      private void selectFolder(FolderDTO folderDTO)
      {
         {
            var outputFolder = _dialogCreator.AskForFolder(Captions.SelectOutputFolder, OSPSuite.Core.Domain.Constants.DirectoryKey.PROJECT);
            if (string.IsNullOrEmpty(outputFolder))
               return;

            folderDTO.FolderPath = outputFolder;
         }
      }

      public void SelectSecondFolder()
      {
         selectFolder(_folderComparisonDTO.SecondFolder);
      }

      public void SelectExclusionList()
      {
         var file = _dialogCreator.AskForFileToOpen("Open Exclusion List File", OSPSuite.Core.Domain.Constants.Filter.JSON_FILE_FILTER, OSPSuite.Core.Domain.Constants.DirectoryKey.PROJECT);
         if (string.IsNullOrEmpty(file))
            return;

         _folderComparisonDTO.ExclusionFile = file;
      }

      private IReadOnlyList<string> exclusionListFrom(string exclusionFile)
      {
         if (string.IsNullOrEmpty(exclusionFile))
            return null;

         var settings = new JsonSerializerSettings();
         var exclusions = JsonConvert.DeserializeObject<IEnumerable<string>>(File.ReadAllText(exclusionFile), settings);
         return exclusions?.ToList();
      }

      public ILoggerView LoggerView => View;
   }
}