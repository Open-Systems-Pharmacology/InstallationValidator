using System;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Core.Services;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Events;
using OSPSuite.InstallationValidator.Core.Extensions;
using OSPSuite.InstallationValidator.Core.Presentation.DTO;
using OSPSuite.InstallationValidator.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;
using IMainView = OSPSuite.InstallationValidator.Core.Presentation.Views.IMainView;

namespace OSPSuite.InstallationValidator.Core.Presentation
{
   public interface IMainPresenter : IDisposablePresenter, IListener<LogAppendedEvent>, IListener<LogResetEvent>
   {
      void SelectOutputFolder();
      void Abort();
      Task StartInstallationValidation();
   }

   public class MainPresenter : AbstractDisposablePresenter<IMainView, IMainPresenter>, IMainPresenter
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IBatchStarterTask _batchStarterTask;
      private readonly IBatchComparisonTask _batchComparisonTask;
      private readonly IInstallationValidationConfiguration _configuration;
      private readonly FolderDTO _outputFolderDTO = new FolderDTO();
      private CancellationTokenSource _cancellationTokenSource;

      public MainPresenter(IMainView view, IDialogCreator dialogCreator, IBatchStarterTask batchStarterTask, IBatchComparisonTask batchComparisonTask, IInstallationValidationConfiguration configuration) : base(view)
      {
         _dialogCreator = dialogCreator;
         _batchStarterTask = batchStarterTask;
         _batchComparisonTask = batchComparisonTask;
         _configuration = configuration;
         view.BindTo(_outputFolderDTO);
      }

      public void SelectOutputFolder()
      {
         var outputFolder = _dialogCreator.AskForFolder(Constants.Captions.SelectOutputFolder, OSPSuite.Core.Domain.Constants.DirectoryKey.PROJECT);
         if (string.IsNullOrEmpty(outputFolder))
            return;

         _outputFolderDTO.FolderPath = outputFolder;
      }

      public void Abort()
      {
         _cancellationTokenSource?.Cancel();
      }

      public async Task StartInstallationValidation()
      {
         _cancellationTokenSource = new CancellationTokenSource();
         try
         {
            View.ValidationIsRunning(true);
            await _batchStarterTask.StartBatch(_outputFolderDTO.FolderPath, _cancellationTokenSource.Token);
            await _batchComparisonTask.StartComparison(_outputFolderDTO.FolderPath, _cancellationTokenSource.Token);
         }
         catch (OperationCanceledException)
         {
            _dialogCreator.MessageBoxInfo(Constants.Captions.TheValidationWasCanceled);
         }
         catch (Exception e)
         {
            View.AppendText(Constants.Captions.Exceptions.ExceptionSupportMessage(_configuration.IssueTrackerUrl));
            View.AppendText($"{Environment.NewLine}{Environment.NewLine}{e.ExceptionMessageWithStackTrace()}");
            View.AppendText($"{Environment.NewLine}{Environment.NewLine}");
         }
         finally
         {
            View.ValidationIsRunning(false);
         }
      }

      public void Handle(LogAppendedEvent eventToHandle)
      {
         View.AppendText(eventToHandle.NewText);
      }

      public void Handle(LogResetEvent eventToHandle)
      {
         View.ResetText(eventToHandle.NewText);
      }
   }
}