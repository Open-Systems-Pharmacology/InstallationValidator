using System.Linq;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.InstallationValidator.Core.Presentation;
using OSPSuite.InstallationValidator.Core.Presentation.DTO;
using OSPSuite.Presentation.Extensions;
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

         _screenBinder.Bind(x => x.FolderPath).To(outputFolderButton);

         btnOk.Click += (o, e) => OnEvent(() => _presenter.StartInstallationValidation());
         btnCancel.Click += (o, e) => OnEvent(() => _presenter.Abort());
      }

      protected override void ExtraClicked()
      {
         _presenter.Abort();
         Close();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemOutputButton.Text = Core.Assets.Captions.OutputFolder.FormatForLabel();
         btnOk.Text = Core.Assets.Captions.Start;
         btnCancel.Text = Captions.Cancel;

         ExtraEnabled = true;
         ExtraVisible = true;
         btnExtra.Text = Captions.CloseButton;

         ShowInTaskbar = true;
         layoutControlItemDescription.TextVisible = false;
         labelValidationDescription.AsDescription();
         labelValidationDescription.Text = Core.Assets.Constants.Captions.ValidationDescription.FormatForDescription();
         ShowInTaskbar = true;
         layoutControlItemDescription.TextVisible = false;
         labelValidationDescription.AsDescription();
         labelValidationDescription.Text = Core.Assets.Constants.Captions.ValidationDescription.FormatForDescription();
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
         OkEnabled = !HasError && !validationRunning;
      }

      public void AppendText(string newText)
      {
         getInnerTextBox(memoEditLogMessages).AppendText(newText);
      }

      private TextBox getInnerTextBox(DevExpress.XtraEditors.TextEdit editor)
      {
         return editor?.Controls.OfType<TextBox>().FirstOrDefault();
      }

      public void ResetText(string newText)
      {
         getInnerTextBox(memoEditLogMessages).Text = newText;
      }
   }
}
