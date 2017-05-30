using System;
using System.Threading;
using System.Threading.Tasks;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Events;
using InstallationValidator.Core.Extensions;
using InstallationValidator.Core.Presentation.DTO;
using InstallationValidator.Core.Presentation.Views;
using InstallationValidator.Core.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace InstallationValidator.Core.Presentation
{
   public interface IMainPresenter : IDisposablePresenter, ILoggerPresenter
   {
      void SelectOutputFolder();
      void Abort();
      Task<InstallationValidationResult> StartInstallationValidation();
   }

   public class MainPresenter : AbstractDisposablePresenter<IMainView, IMainPresenter>, IMainPresenter
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IBatchStarterTask _batchStarterTask;
      private readonly IBatchComparisonTask _batchComparisonTask;
      private readonly IInstallationValidatorConfiguration _configuration;
      private readonly IValidationReportingTask _validationReportingTask;
      private readonly FolderDTO _outputFolderDTO = new FolderDTO(folderMustExist: false);
      private CancellationTokenSource _cancellationTokenSource;
      private bool _validationRunning;

      public MainPresenter(IMainView view, IDialogCreator dialogCreator, IBatchStarterTask batchStarterTask, IBatchComparisonTask batchComparisonTask, IInstallationValidatorConfiguration configuration, IValidationReportingTask validationReportingTask) : base(view)
      {
         _dialogCreator = dialogCreator;
         _batchStarterTask = batchStarterTask;
         _batchComparisonTask = batchComparisonTask;
         _configuration = configuration;
         _validationReportingTask = validationReportingTask;
         _outputFolderDTO.FolderPath = configuration.DefaultOutputPath;
         view.BindTo(_outputFolderDTO);
      }

      public void SelectOutputFolder()
      {
         var outputFolder = _dialogCreator.AskForFolder(Captions.SelectOutputFolder, OSPSuite.Core.Domain.Constants.DirectoryKey.PROJECT);
         if (string.IsNullOrEmpty(outputFolder))
            return;

         _outputFolderDTO.FolderPath = outputFolder;
      }

      public void Abort()
      {
         if (!_validationRunning)
            return;

         if (_dialogCreator.MessageBoxYesNo(Captions.ReallyCancelInstallationValidation) == ViewResult.No)
            return;

         _cancellationTokenSource?.Cancel();
      }

      public async Task<InstallationValidationResult> StartInstallationValidation()
      {
         _cancellationTokenSource = new CancellationTokenSource();
         var validationResult = new InstallationValidationResult();
         try
         {
            updateValidationRunningState(running: true);
            this.ResetLog();

            var startTime = DateTime.Now;

            this.LogLine(Logs.StartingBatchCalculation);
            this.LogLine();
            var runSummary = await _batchStarterTask.StartBatch(_outputFolderDTO.FolderPath, _cancellationTokenSource.Token);
            runSummary.StartTime = startTime;

            this.LogLine(Logs.StartingComparison);
            validationResult.ComparisonResult = await _batchComparisonTask.StartComparison(_outputFolderDTO.FolderPath, _cancellationTokenSource.Token);
            this.LogLine();

            runSummary.EndTime = DateTime.Now;
            validationResult.RunSummary = runSummary;

            this.LogLine(Logs.StartingReport);
            await _validationReportingTask.CreateReport(validationResult, _outputFolderDTO.FolderPath, openReport: true);
            this.LogLine();

            this.LogLine(Logs.ValidationCompleted);

            return validationResult;
         }
         catch (OperationCanceledException)
         {
            this.LogLine(Captions.TheValidationWasCanceled);
            return validationResult;
         }
         catch (Exception e)
         {
            this.LogException(e, _configuration.IssueTrackerUrl);
            return validationResult;
         }
         finally
         {
            updateValidationRunningState(running: false);
         }
      }

      private void updateValidationRunningState(bool running)
      {
         _validationRunning = running;
         View.ValidationIsRunning(_validationRunning);
      }

      public void Handle(AppendTextToLogEvent eventToHandle)
      {
         this.LogText(eventToHandle.Text, eventToHandle.IsHtml);
      }

      public void Handle(AppendLineToLogEvent eventToHandle)
      {
         this.LogLine(eventToHandle.Line, eventToHandle.IsHtml);
      }

      public ILoggerView LoggerView => View;
   }
}