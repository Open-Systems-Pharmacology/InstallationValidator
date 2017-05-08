using System;
using System.Threading;
using System.Threading.Tasks;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Events;
using InstallationValidator.Core.Presentation.DTO;
using InstallationValidator.Core.Presentation.Views;
using InstallationValidator.Core.Services;
using InstallationValidator.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace InstallationValidator.Core.Presentation
{
   public interface ISimulationComparisonPresenter : IDisposablePresenter, IComparisonPresenter
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
      private readonly IValidationReportingTask _validationReportingTask;

      public SimulationComparisonPresenter(ISimulationComparisonView view, IInstallationValidatorConfiguration configuration, IDialogCreator dialogCreator, 
         IBatchComparisonTask batchComparisonTask, IValidationReportingTask validationReportingTask) : base(view)
      {
         _configuration = configuration;
         _dialogCreator = dialogCreator;
         _batchComparisonTask = batchComparisonTask;
         _validationReportingTask = validationReportingTask;
         view.BindTo(new FolderComparisonDTO(_firstFolderDTO, _secondFolderDTO));
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
            var comparisonResult = await _batchComparisonTask.StartComparison(_firstFolderDTO.FolderPath, _secondFolderDTO.FolderPath, _cancellationTokenSource.Token, Assets.Reporting.First, Assets.Reporting.Second);
            this.LogLine();

            this.LogLine(Logs.StartingReport);
            await _validationReportingTask.CreateReport(comparisonResult, _firstFolderDTO.FolderPath, _secondFolderDTO.FolderPath, openReport: true);
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

      public IComparisonView ComparisonView => View;
   }
}
