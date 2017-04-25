using OSPSuite.InstallationValidator.Core.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.InstallationValidator.Core.Presentation.Views
{
   public interface IMainView : IModalView<IMainPresenter>
   {
      void AddLogView(IView view);
      void BindTo(FolderDTO outputFolderDTO);
   }
}