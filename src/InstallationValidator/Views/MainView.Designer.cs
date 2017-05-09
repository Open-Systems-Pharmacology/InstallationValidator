namespace InstallationValidator.Views
{
   partial class MainView
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         _screenBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.stopButton = new DevExpress.XtraEditors.SimpleButton();
         this.startButton = new DevExpress.XtraEditors.SimpleButton();
         this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
         this.labelValidationDescription = new DevExpress.XtraEditors.LabelControl();
         this.outputFolderButton = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemRichEdit = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemButtonStart = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemOutputButton = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemButtonStop = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.outputFolderButton.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemRichEdit)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonStart)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemOutputButton)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonStop)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.stopButton);
         this.layoutControl.Controls.Add(this.startButton);
         this.layoutControl.Controls.Add(this.richEditControl);
         this.layoutControl.Controls.Add(this.labelValidationDescription);
         this.layoutControl.Controls.Add(this.outputFolderButton);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1078, 855, 450, 400);
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(1047, 770);
         this.layoutControl.TabIndex = 38;
         this.layoutControl.Text = "layoutControl";
         // 
         // stopButton
         // 
         this.stopButton.Location = new System.Drawing.Point(178, 736);
         this.stopButton.Name = "stopButton";
         this.stopButton.Size = new System.Drawing.Size(62, 22);
         this.stopButton.StyleController = this.layoutControl;
         this.stopButton.TabIndex = 9;
         this.stopButton.Text = "stopButton";
         // 
         // startButton
         // 
         this.startButton.Location = new System.Drawing.Point(400, 736);
         this.startButton.Name = "startButton";
         this.startButton.Size = new System.Drawing.Size(635, 22);
         this.startButton.StyleController = this.layoutControl;
         this.startButton.TabIndex = 8;
         this.startButton.Text = "startButton";
         // 
         // richEditControl
         // 
         this.richEditControl.Location = new System.Drawing.Point(12, 93);
         this.richEditControl.Name = "richEditControl";
         this.richEditControl.Options.Export.Rtf.ExportTheme = true;
         this.richEditControl.Size = new System.Drawing.Size(1023, 629);
         this.richEditControl.TabIndex = 7;
         this.richEditControl.Text = "richEditControl";
         // 
         // labelValidationDescription
         // 
         this.labelValidationDescription.Location = new System.Drawing.Point(168, 12);
         this.labelValidationDescription.Name = "labelValidationDescription";
         this.labelValidationDescription.Size = new System.Drawing.Size(121, 13);
         this.labelValidationDescription.StyleController = this.layoutControl;
         this.labelValidationDescription.TabIndex = 6;
         this.labelValidationDescription.Text = "labelValidationDescription";
         // 
         // outputFolderButton
         // 
         this.outputFolderButton.Location = new System.Drawing.Point(12, 56);
         this.outputFolderButton.Name = "outputFolderButton";
         this.outputFolderButton.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.outputFolderButton.Size = new System.Drawing.Size(1023, 20);
         this.outputFolderButton.StyleController = this.layoutControl;
         this.outputFolderButton.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemRichEdit,
            this.layoutItemButtonStart,
            this.emptySpaceItem3,
            this.emptySpaceItem1,
            this.emptySpaceItem4,
            this.layoutControlItemDescription,
            this.layoutControlItemOutputButton,
            this.layoutItemButtonStop,
            this.emptySpaceItem2});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Size = new System.Drawing.Size(1047, 770);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutControlItemRichEdit
         // 
         this.layoutControlItemRichEdit.Control = this.richEditControl;
         this.layoutControlItemRichEdit.Location = new System.Drawing.Point(0, 81);
         this.layoutControlItemRichEdit.Name = "layoutControlItemRichEdit";
         this.layoutControlItemRichEdit.Size = new System.Drawing.Size(1027, 633);
         this.layoutControlItemRichEdit.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemRichEdit.TextVisible = false;
         // 
         // layoutItemButtonStart
         // 
         this.layoutItemButtonStart.Control = this.startButton;
         this.layoutItemButtonStart.Location = new System.Drawing.Point(232, 724);
         this.layoutItemButtonStart.Name = "layoutItemButtonStart";
         this.layoutItemButtonStart.Size = new System.Drawing.Size(795, 26);
         this.layoutItemButtonStart.TextSize = new System.Drawing.Size(153, 13);
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(0, 724);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(10, 26);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 17);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1027, 11);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(0, 68);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(1027, 13);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItemDescription
         // 
         this.layoutControlItemDescription.Control = this.labelValidationDescription;
         this.layoutControlItemDescription.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemDescription.Name = "layoutControlItemDescription";
         this.layoutControlItemDescription.Size = new System.Drawing.Size(1027, 17);
         this.layoutControlItemDescription.TextSize = new System.Drawing.Size(153, 13);
         // 
         // layoutControlItemOutputButton
         // 
         this.layoutControlItemOutputButton.Control = this.outputFolderButton;
         this.layoutControlItemOutputButton.Location = new System.Drawing.Point(0, 28);
         this.layoutControlItemOutputButton.Name = "layoutControlItemOutputButton";
         this.layoutControlItemOutputButton.Size = new System.Drawing.Size(1027, 40);
         this.layoutControlItemOutputButton.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutControlItemOutputButton.TextSize = new System.Drawing.Size(153, 13);
         // 
         // layoutItemButtonStop
         // 
         this.layoutItemButtonStop.Control = this.stopButton;
         this.layoutItemButtonStop.Location = new System.Drawing.Point(10, 724);
         this.layoutItemButtonStop.Name = "layoutItemButtonStop";
         this.layoutItemButtonStop.Size = new System.Drawing.Size(222, 26);
         this.layoutItemButtonStop.TextSize = new System.Drawing.Size(153, 13);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 714);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(1027, 10);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // MainView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "MainView";
         this.ClientSize = new System.Drawing.Size(1047, 770);
         this.Controls.Add(this.layoutControl);
         this.Name = "MainView";
         this.Text = "MainView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.outputFolderButton.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemRichEdit)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonStart)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemOutputButton)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonStop)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.ButtonEdit outputFolderButton;
      private DevExpress.XtraEditors.LabelControl labelValidationDescription;
      private DevExpress.XtraRichEdit.RichEditControl richEditControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemRichEdit;
      private DevExpress.XtraEditors.SimpleButton stopButton;
      private DevExpress.XtraEditors.SimpleButton startButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonStart;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonStop;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
      private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemOutputButton;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
   }
}