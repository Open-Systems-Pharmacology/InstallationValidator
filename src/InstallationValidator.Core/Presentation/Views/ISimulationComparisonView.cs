using InstallationValidator.Core.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace InstallationValidator.Core.Presentation.Views
{
   public interface ISimulationComparisonView : IView<ISimulationComparisonPresenter>
   {
      void ResetText(string empty);
      void AppendText(string textToLog);
      void AppendHTML(string htmlToLog);
      void ComparisonIsRunning(bool comparisonRunning);
      void BindTo(FolderDTO firstFolderDTO, FolderDTO secondFolderDTO);
   }
}