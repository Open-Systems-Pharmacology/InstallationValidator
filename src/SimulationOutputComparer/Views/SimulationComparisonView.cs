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
using OSPSuite.UI;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;
using Captions = InstallationValidator.Core.Assets.Captions;

namespace SimulationOutputComparer.Views
{
   public partial class SimulationComparisonView : BaseView, ISimulationComparisonView
   {
      private ISimulationComparisonPresenter _presenter;
      private readonly ScreenBinder<FolderDTO> _screenBinderFolder1;
      private readonly ScreenBinder<FolderDTO> _screenBinderFolder2;
      private readonly ScreenBinder<FolderComparisonDTO> _screenBinder;

      public SimulationComparisonView()
      {
         InitializeComponent();
         _screenBinderFolder1 = new ScreenBinder<FolderDTO>();
         _screenBinderFolder2 = new ScreenBinder<FolderDTO>();
         _screenBinder = new ScreenBinder<FolderComparisonDTO>();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         ShowInTaskbar = true;

         layoutControlItemFolder1.Text = Captions.ComparisonFolder1.FormatForLabel();
         layoutControlItemFolder2.Text = Captions.ComparisonFolder2.FormatForLabel();
         layoutItemNumberOfCurvesToDisplay.Text = Captions.NumberOfCurvesToDisplay.FormatForLabel();
         layoutItemIgnoreAddedCurves.TextVisible = false;
         layoutItemIgnoreRemovedCurves.TextVisible = false;

         richEditControl.Document.Text = string.Empty;
         richEditControl.ActiveViewType = RichEditViewType.Simple;

         layoutItemButtonStart.AdjustSize(UIConstants.Size.LARGE_BUTTON_WIDTH, Constants.BUTTON_HEIGHT);
         startButton.InitWithImage(ApplicationIcons.Run, IconSizes.Size32x32, Captions.StartComparison);
         layoutItemButtonStart.TextVisible = false;
         layoutItemButtonStop.TextVisible = false;

         layoutItemButtonStop.AdjustSize(UIConstants.Size.LARGE_BUTTON_WIDTH, Constants.BUTTON_HEIGHT);
         stopButton.InitWithImage(ApplicationIcons.Stop, IconSizes.Size32x32, Captions.StopComparison);

         layoutItemButtonStop.Visibility = LayoutVisibilityConvertor.FromBoolean(false);
         defaultLookAndFeel.LookAndFeel.SetSkinStyle(Constants.DEFAULT_SKIN);

         Caption = Captions.SimulationComparisonViewTitle;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinderFolder1.Bind(x => x.FolderPath)
            .To(buttonEditFolder1);

         _screenBinderFolder2.Bind(x => x.FolderPath)
            .To(buttonEditFolder2);

         _screenBinder.Bind(x => x.NumberOfCurves)
            .To(textEditNumberOfCurves);


         _screenBinder.Bind(x => x.IgnoreAddedCurves)
            .To(chkIgnoreAddedCurves)
            .WithCaption(Captions.IgnoreAddedCurves);
         
         _screenBinder.Bind(x => x.IgnoreRemovedCurves)
            .To(chkIgnoreRemovedCurves)
            .WithCaption(Captions.IgnoreRemovedCurves);

         RegisterValidationFor(_screenBinderFolder1);
         RegisterValidationFor(_screenBinderFolder2);
         RegisterValidationFor(_screenBinder);


         startButton.Click += (o, e) => OnEvent(() => _presenter.StartComparison());
         stopButton.Click += (o, e) => OnEvent(() => _presenter.Abort());
         buttonEditFolder1.ButtonClick += (o, e) => OnEvent(() => _presenter.SelectFirstFolder());
         buttonEditFolder2.ButtonClick += (o, e) => OnEvent(() => _presenter.SelectSecondFolder());
      }

      public void AttachPresenter(ISimulationComparisonPresenter presenter)
      {
         _presenter = presenter;
      }

      public override bool HasError => _screenBinderFolder1.HasError || _screenBinderFolder2.HasError;

      protected override void OnValidationError(Control control, string error)
      {
         base.OnValidationError(control, error);
         setOkButtonEnable();
      }

      private void setOkButtonEnable()
      {
         layoutItemButtonStart.Enabled = !HasError;
      }

      protected override void OnClearError(Control control)
      {
         base.OnClearError(control);
         setOkButtonEnable();
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

      public void ComparisonIsRunning(bool comparisonRunning)
      {
         layoutItemButtonStop.Visibility = LayoutVisibilityConvertor.FromBoolean(comparisonRunning);
         layoutItemButtonStart.Visibility = LayoutVisibilityConvertor.FromBoolean(!comparisonRunning);
      }

      public void BindTo(FolderComparisonDTO folderComparisonDTO)
      {
         _screenBinder.BindToSource(folderComparisonDTO);
         _screenBinderFolder1.BindToSource(folderComparisonDTO.FirstFolder);
         _screenBinderFolder2.BindToSource(folderComparisonDTO.SecondFolder);
      }
   }
}