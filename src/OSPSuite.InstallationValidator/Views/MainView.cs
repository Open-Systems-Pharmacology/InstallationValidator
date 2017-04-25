using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.InstallationValidator.Core.Presentation;
using OSPSuite.InstallationValidator.Core.Presentation.DTO;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;
using IMainView = OSPSuite.InstallationValidator.Core.Presentation.Views.IMainView;

namespace OSPSuite.InstallationValidator.Views
{
   public partial class MainView : BaseModalView, IMainView
   {
      private IMainPresenter _presenter;
      private readonly ScreenBinder<FolderDTO> _screenBinder;

      public MainView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<FolderDTO>();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         outputFolderButton.ButtonClick += (o, e) => OnEvent(() => _presenter.SelectOutputFolder());
         RegisterValidationFor(_screenBinder);

         _screenBinder.Bind(x => x.TargetFolder).To(outputFolderButton);

         btnOk.Click += (o, e) => OnEvent(async () => await _presenter.StartInstallationValidation());
         btnCancel.Click += (o, e) => OnEvent(() => _presenter.Abort());
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemOutputButton.Text = Core.Assets.Constants.Captions.OutputFolder.FormatForLabel();
      }

      public void AttachPresenter(IMainPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddLogView(ILogView logPresenterView)
      {
         logPanel.FillWith(logPresenterView);
      }

      public void BindTo(FolderDTO outputFolderDTO)
      {
         _screenBinder.BindToSource(outputFolderDTO);
      }
   }
}
