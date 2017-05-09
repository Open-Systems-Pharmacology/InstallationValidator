using InstallationValidator.Core.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace InstallationValidator.Core.Presentation.Views
{
   public interface ISimulationComparisonView : ILoggerView, IView<ISimulationComparisonPresenter>
   {
      void ComparisonIsRunning(bool comparisonRunning);
      void BindTo(FolderComparisonDTO folderComparisonDTO);
   }
}