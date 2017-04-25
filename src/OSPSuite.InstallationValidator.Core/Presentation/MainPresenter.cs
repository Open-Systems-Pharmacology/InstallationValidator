using System.Threading.Tasks;
using OSPSuite.Core.Services;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using IMainView = OSPSuite.InstallationValidator.Core.Presentation.Views.IMainView;

namespace OSPSuite.InstallationValidator.Core.Presentation
{
   public interface IMainPresenter : IDisposablePresenter
   {
      void SelectOutputFolder();
      void Abort();
      Task StartInstallationValidation();
   }

   public class MainPresenter : AbstractDisposablePresenter<IMainView, IMainPresenter>, IMainPresenter
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly FolderDTO _outputFolderDTO = new FolderDTO();

      public MainPresenter(IMainView view, ILogPresenter logPresenter, IDialogCreator dialogCreator) : base(view)
      {
         _dialogCreator = dialogCreator;
         view.AddLogView(logPresenter.View);
         view.BindTo(_outputFolderDTO);
      }

      public void SelectOutputFolder()
      {
         var outputFolder = _dialogCreator.AskForFolder(Constants.Captions.SelectOutputFolder, OSPSuite.Core.Domain.Constants.DirectoryKey.PROJECT);
         if (string.IsNullOrEmpty(outputFolder))
            return;

         _outputFolderDTO.TargetFolder = outputFolder;
      }

      public void Abort()
      {
         throw new System.NotImplementedException();
      }

      public Task StartInstallationValidation()
      {
         throw new System.NotImplementedException();
      }
   }
}