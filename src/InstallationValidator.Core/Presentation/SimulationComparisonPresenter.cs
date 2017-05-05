using System;
using System.Threading;
using System.Threading.Tasks;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Events;
using InstallationValidator.Core.Presentation.DTO;
using InstallationValidator.Core.Presentation.Views;
using InstallationValidator.Core.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace InstallationValidator.Core.Presentation
{

   public interface ISimulationComparisonPresenter : IDisposablePresenter,
      IListener<AppendTextToLogEvent>,
      IListener<AppendLineToLogEvent>
   {
      Task StartComparison();
      void Abort();
      void SelectFirstFolder();
      void SelectSecondFolder();
   }

   public class SimulationComparisonPresenter : AbstractDisposablePresenter<ISimulationComparisonView, ISimulationComparisonPresenter>, ISimulationComparisonPresenter
   {
      private readonly FolderDTO _firstFolderDTO = new FolderDTO();
      private readonly FolderDTO _secondFolderDTO = new FolderDTO();
      private readonly IInstallationValidatorConfiguration _configuration;
      private readonly IDialogCreator _dialogCreator;
      private CancellationTokenSource _cancellationTokenSource;
      private bool _comparisonRunning;
      private readonly IBatchComparisonTask _batchComparisonTask;
      private readonly IBatchComparisonReportingTask _batchComparisonReportingTask;

      public SimulationComparisonPresenter(ISimulationComparisonView view, IInstallationValidatorConfiguration configuration, IDialogCreator dialogCreator, 
         IBatchComparisonTask batchComparisonTask, IBatchComparisonReportingTask batchComparisonReportingTask) : base(view)
      {
         _configuration = configuration;
         _dialogCreator = dialogCreator;
         _batchComparisonTask = batchComparisonTask;
         _batchComparisonReportingTask = batchComparisonReportingTask;
         _secondFolderDTO.FolderPath = configuration.BatchOutputsFolderPath;
         view.BindTo(_firstFolderDTO, _secondFolderDTO);
      }

      private void logText(string textToLog, bool isHtml = true)
      {
         if (isHtml)
            logHTML(textToLog);
         else
            View.AppendText(textToLog);
      }

      private void logHTML(string htmlToLog)
      {
         View.AppendHTML(htmlToLog);
      }

      public void Handle(AppendTextToLogEvent eventToHandle)
      {
         logText(eventToHandle.Text, eventToHandle.IsHtml);
      }

      private void resetLog()
      {
         View.ResetText(string.Empty);
      }

      public void Handle(AppendLineToLogEvent eventToHandle)
      {
         logLine(eventToHandle.Line, eventToHandle.IsHtml);
      }

      public async Task StartComparison()
      {
         _cancellationTokenSource = new CancellationTokenSource();
         try
         {
            updateComparisonRunningState(running: true);
            resetLog();

            logLine(Logs.StartingComparison);
            var comparisonResult = await _batchComparisonTask.StartComparison(_firstFolderDTO.FolderPath, _secondFolderDTO.FolderPath, _cancellationTokenSource.Token, Assets.Reporting.First, Assets.Reporting.Second);
            logLine();

            logLine(Logs.StartingReport);
            await _batchComparisonReportingTask.CreateReport(comparisonResult, _firstFolderDTO.FolderPath, _secondFolderDTO.FolderPath, openReport: true);
            logLine();

            logLine(Logs.ComparisonCompleted);
         }
         catch (OperationCanceledException)
         {
            logLine(Captions.TheComparisonWasCanceled);
         }
         catch (Exception e)
         {
            logException(e);
         }
         finally
         {
            updateComparisonRunningState(running: false);
         }
      }

      private void logException(Exception e)
      {
         logLine();
         logHTML(Exceptions.ExceptionViewDescription(_configuration.IssueTrackerUrl));
         logLine();
         logLine(e.ExceptionMessageWithStackTrace());
         logLine();
      }

      private void updateComparisonRunningState(bool running)
      {
         _comparisonRunning = running;
         View.ComparisonIsRunning(_comparisonRunning);
      }

      private void logLine(string textToLog = "", bool isHtml = true)
      {
         if (isHtml)
            logHTML($"<br>{textToLog}");
         else
            logText($"{Environment.NewLine}{textToLog}", isHtml: false);
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
         selectFolder(_firstFolderDTO);
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
         selectFolder(_secondFolderDTO);
      }
   }
}
