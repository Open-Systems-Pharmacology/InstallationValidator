using OSPSuite.Core.Services;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Presentation.DTO;
using OSPSuite.InstallationValidator.Core.Services;
using OSPSuite.Presentation.Presenters;
using IMainView = OSPSuite.InstallationValidator.Core.Presentation.Views.IMainView;

namespace OSPSuite.InstallationValidator.Core.Presentation
{
   public interface IMainPresenter : IDisposablePresenter
   {
      void SelectOutputFolder();
      void Abort();
      void StartInstallationValidation();
   }

   public class MainPresenter : AbstractDisposablePresenter<IMainView, IMainPresenter>, IMainPresenter
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IBatchStarterTask _batchStarterTask;
      private readonly FolderDTO _outputFolderDTO = new FolderDTO();
      private StartableProcess _process;

      public MainPresenter(IMainView view, ILogPresenter logPresenter, IDialogCreator dialogCreator, IBatchStarterTask batchStarterTask) : base(view)
      {
         _dialogCreator = dialogCreator;
         _batchStarterTask = batchStarterTask;
         view.AddLogView(logPresenter.View);
         view.BindTo(_outputFolderDTO);
      }

      public void SelectOutputFolder()
      {
         var outputFolder = _dialogCreator.AskForFolder(Constants.Captions.SelectOutputFolder, OSPSuite.Core.Domain.Constants.DirectoryKey.PROJECT);
         if (string.IsNullOrEmpty(outputFolder))
            return;

         _outputFolderDTO.FolderPath = outputFolder;
      }

      public override void ViewChanged()
      {
         _view.OkEnabled = _view.HasError;
      }

      public void Abort()
      {
         _batchStarterTask.StopValidation(_process);
      }

      public void StartInstallationValidation()
      {
         _process = _batchStarterTask.StartValidation(_outputFolderDTO.FolderPath);
      }
   }
}