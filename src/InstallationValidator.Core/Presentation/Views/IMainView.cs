using InstallationValidator.Core.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace InstallationValidator.Core.Presentation.Views
{
   public interface IMainView : IView<IMainPresenter>
   {
      void BindTo(FolderDTO outputFolderDTO);
      void ValidationIsRunning(bool validationRunning);
      void AppendText(string newText);
      void ResetText(string newText);
      void AppendHTML(string htmlToLog);
   }
}