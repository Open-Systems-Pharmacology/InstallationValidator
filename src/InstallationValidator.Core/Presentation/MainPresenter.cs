﻿using System;
using System.Threading;
using System.Threading.Tasks;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Domain;
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
   public interface IMainPresenter : IDisposablePresenter,
      IListener<AppendTextToLogEvent>,
      IListener<AppendLineToLogEvent>
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
            resetLog();

            var startTime = DateTime.Now;

            logLine(Logs.StartingBatchCalculation);
            logLine();
            var runSummary = await _batchStarterTask.StartBatch(_outputFolderDTO.FolderPath, _cancellationTokenSource.Token);
            runSummary.StartTime = startTime;

            logLine(Logs.StartingComparison);
            validationResult.ComparisonResult = await _batchComparisonTask.StartComparison(_outputFolderDTO.FolderPath, _cancellationTokenSource.Token);
            logLine();

            runSummary.EndTime = DateTime.Now;
            validationResult.RunSummary = runSummary;

            logLine(Logs.StartingReport);
            await _validationReportingTask.CreateReport(validationResult, _outputFolderDTO.FolderPath, openReport: true);
            logLine();

            logLine(Logs.ValidationCompleted);

            return validationResult;
         }
         catch (OperationCanceledException)
         {
            logLine(Captions.TheValidationWasCanceled);
            return validationResult;
         }
         catch (Exception e)
         {
            logException(e);
            return validationResult;
         }
         finally
         {
            updateValidationRunningState(running: false);
         }
      }

      private void logLine(string textToLog = "", bool isHtml = true)
      {
         if (isHtml)
            logHTML($"<br>{textToLog}");
         else
            logText($"{Environment.NewLine}{textToLog}", isHtml: false);
      }

      private void updateValidationRunningState(bool running)
      {
         _validationRunning = running;
         View.ValidationIsRunning(_validationRunning);
      }

      private void logText(string textToLog, bool isHtml = true)
      {
         if (isHtml)
            logHTML(textToLog);
         else
            View.AppendText(textToLog);
      }

      private void logException(Exception e)
      {
         logLine();
         logHTML(Exceptions.ExceptionViewDescription(_configuration.IssueTrackerUrl));
         logLine();
         logLine(e.ExceptionMessageWithStackTrace());
         logLine();
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
   }
}