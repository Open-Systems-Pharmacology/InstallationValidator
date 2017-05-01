using OSPSuite.InstallationValidator.Core.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.InstallationValidator.Core.Presentation.Views
{
   public interface IMainView : IModalView<IMainPresenter>
   {
      void BindTo(FolderDTO outputFolderDTO);
      void ValidationIsRunning(bool validationRunning);
      void AppendText(string newText);
      void ResetText(string newText);
      void AppendHTML(string htmlToLog);
   }
}