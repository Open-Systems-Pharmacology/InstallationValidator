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

   public interface ISimulationComparisonView : IView<ISimulationComparisonPresenter>
   {
      void ResetText(string empty);
      void AppendText(string textToLog);
      void AppendHTML(string htmlToLog);
      void ValidationIsRunning(bool validationRunning);
      void BindTo(FolderDTO firstFolderDTO, FolderDTO secondFolderDTO);
   }
}