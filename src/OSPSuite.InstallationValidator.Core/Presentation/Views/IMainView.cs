using OSPSuite.InstallationValidator.Core.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.InstallationValidator.Core.Presentation.Views
{
   public interface IMainView : IView<IMainPresenter>
   {
      void AddLogView(ILogView logPresenterView);
      void BindTo(FolderDTO outputFolderDTO);
   }
}