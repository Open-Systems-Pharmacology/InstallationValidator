using InstallationValidator.Core.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace InstallationValidator.Core.Presentation.Views
{
   public interface IMainView : ILoggerView, IView<IMainPresenter>
   {
      void BindTo(FolderDTO outputFolderDTO);
      void BindToReportOptions(ReportOptionsDTO reportOptionsDTO);
      void ValidationIsRunning(bool validationRunning);
   }
}