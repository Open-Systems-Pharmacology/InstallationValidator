using System.Windows.Forms;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraRichEdit;
using InstallationValidator.Core;
using InstallationValidator.Core.Presentation;
using InstallationValidator.Core.Presentation.DTO;
using InstallationValidator.Core.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;
using Captions = InstallationValidator.Core.Assets.Captions;

namespace InstallationValidator.Views
{
   public partial class MainView : BaseView, IMainView
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
         _screenBinder.Bind(x => x.FolderPath)
            .To(outputFolderButton);

         RegisterValidationFor(_screenBinder);


         startButton.Click += (o, e) => OnEvent(() => _presenter.StartInstallationValidation());
         stopButton.Click += (o, e) => OnEvent(() => _presenter.Abort());
         outputFolderButton.ButtonClick += (o, e) => OnEvent(() => _presenter.SelectOutputFolder());
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemOutputButton.Text = Captions.OutputFolder.FormatForLabel();

         ShowInTaskbar = true;
         StartPosition = FormStartPosition.CenterScreen;

         layoutControlItemDescription.TextVisible = false;
         labelDescription.Caption = Captions.ValidationDescription;

         richEditControl.Document.Text = string.Empty;
         richEditControl.ActiveViewType = RichEditViewType.Simple;

         layoutItemButtonStart.AdjustButtonSize(OSPSuite.UI.UIConstants.Size.LARGE_BUTTON_WIDTH, Constants.BUTTON_HEIGHT);
         startButton.InitWithImage(ApplicationIcons.Run, IconSizes.Size32x32, Captions.StartValidation);
         layoutItemButtonStart.TextVisible = false;
         layoutItemButtonStop.TextVisible = false;

         layoutItemButtonStop.AdjustButtonSize(OSPSuite.UI.UIConstants.Size.LARGE_BUTTON_WIDTH, Constants.BUTTON_HEIGHT);
         stopButton.InitWithImage(ApplicationIcons.Stop, IconSizes.Size32x32, Captions.StopValidation);

         layoutItemButtonStop.Visibility = LayoutVisibilityConvertor.FromBoolean(false);
         defaultLookAndFeel.LookAndFeel.SetSkinStyle(Constants.DEFAULT_SKIN);

         Caption = Captions.MainViewTitle;
      }

      protected override void OnValidationError(Control control, string error)
      {
         base.OnValidationError(control, error);
         setOkButtonEnable();
      }

      protected override void OnClearError(Control control)
      {
         base.OnClearError(control);
         setOkButtonEnable();
      }

      private void setOkButtonEnable()
      {
         layoutItemButtonStart.Enabled = !HasError;
      }

      public void AttachPresenter(IMainPresenter presenter)
      {
         _presenter = presenter;
      }

      public override bool HasError => _screenBinder.HasError;

      public void BindTo(FolderDTO outputFolderDTO)
      {
         _screenBinder.BindToSource(outputFolderDTO);
      }

      public void ValidationIsRunning(bool validationRunning)
      {
         layoutItemButtonStop.Visibility = LayoutVisibilityConvertor.FromBoolean(validationRunning);
         layoutItemButtonStart.Visibility = LayoutVisibilityConvertor.FromBoolean(!validationRunning);
      }

      public void AppendText(string newText)
      {
         richEditControl.Document.AppendText(newText);
      }

      public void ResetText(string newText)
      {
         richEditControl.Document.Text = newText;
      }

      public void AppendHTML(string htmlToLog)
      {
         richEditControl.Document.AppendHtmlText(htmlToLog);
      }
   }
}