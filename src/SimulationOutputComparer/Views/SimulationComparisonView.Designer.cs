namespace SimulationOutputComparer.Views
{
   partial class SimulationComparisonView
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
         _screenBinderFolder1.Dispose();
         _screenBinderFolder2.Dispose();
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
         this.components = new System.ComponentModel.Container();
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.textEditNumberOfCurves = new DevExpress.XtraEditors.TextEdit();
         this.startButton = new DevExpress.XtraEditors.SimpleButton();
         this.stopButton = new DevExpress.XtraEditors.SimpleButton();
         this.buttonEditFolder2 = new DevExpress.XtraEditors.ButtonEdit();
         this.buttonEditFolder1 = new DevExpress.XtraEditors.ButtonEdit();
         this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemRichEdit = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemFolder1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemFolder2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemButtonStop = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemButtonStart = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutItemNumberOfCurvesToDisplay = new DevExpress.XtraLayout.LayoutControlItem();
         this.defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
         this.chkIgnoreAddedCurves = new DevExpress.XtraEditors.CheckEdit();
         this.layoutItemIgnoreAddedCurves = new DevExpress.XtraLayout.LayoutControlItem();
         this.chkIgnoreRemovedCurves = new DevExpress.XtraEditors.CheckEdit();
         this.layoutItemIgnoreRemovedCurves = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.textEditNumberOfCurves.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonEditFolder2.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonEditFolder1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemRichEdit)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFolder1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFolder2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonStop)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonStart)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNumberOfCurvesToDisplay)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkIgnoreAddedCurves.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIgnoreAddedCurves)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkIgnoreRemovedCurves.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIgnoreRemovedCurves)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.chkIgnoreRemovedCurves);
         this.layoutControl.Controls.Add(this.chkIgnoreAddedCurves);
         this.layoutControl.Controls.Add(this.textEditNumberOfCurves);
         this.layoutControl.Controls.Add(this.startButton);
         this.layoutControl.Controls.Add(this.stopButton);
         this.layoutControl.Controls.Add(this.buttonEditFolder2);
         this.layoutControl.Controls.Add(this.buttonEditFolder1);
         this.layoutControl.Controls.Add(this.richEditControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(716, 486);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // textEditNumberOfCurves
         // 
         this.textEditNumberOfCurves.Location = new System.Drawing.Point(196, 60);
         this.textEditNumberOfCurves.Name = "textEditNumberOfCurves";
         this.textEditNumberOfCurves.Size = new System.Drawing.Size(508, 20);
         this.textEditNumberOfCurves.StyleController = this.layoutControl;
         this.textEditNumberOfCurves.TabIndex = 11;
         // 
         // startButton
         // 
         this.startButton.Location = new System.Drawing.Point(618, 452);
         this.startButton.Name = "startButton";
         this.startButton.Size = new System.Drawing.Size(86, 22);
         this.startButton.StyleController = this.layoutControl;
         this.startButton.TabIndex = 10;
         this.startButton.Text = "startButton";
         // 
         // stopButton
         // 
         this.stopButton.Location = new System.Drawing.Point(368, 452);
         this.stopButton.Name = "stopButton";
         this.stopButton.Size = new System.Drawing.Size(62, 22);
         this.stopButton.StyleController = this.layoutControl;
         this.stopButton.TabIndex = 9;
         this.stopButton.Text = "stopButton";
         // 
         // buttonEditFolder2
         // 
         this.buttonEditFolder2.Location = new System.Drawing.Point(196, 36);
         this.buttonEditFolder2.Name = "buttonEditFolder2";
         this.buttonEditFolder2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.buttonEditFolder2.Size = new System.Drawing.Size(508, 20);
         this.buttonEditFolder2.StyleController = this.layoutControl;
         this.buttonEditFolder2.TabIndex = 8;
         // 
         // buttonEditFolder1
         // 
         this.buttonEditFolder1.Location = new System.Drawing.Point(196, 12);
         this.buttonEditFolder1.Name = "buttonEditFolder1";
         this.buttonEditFolder1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.buttonEditFolder1.Size = new System.Drawing.Size(508, 20);
         this.buttonEditFolder1.StyleController = this.layoutControl;
         this.buttonEditFolder1.TabIndex = 7;
         // 
         // richEditControl
         // 
         this.richEditControl.Location = new System.Drawing.Point(12, 142);
         this.richEditControl.Name = "richEditControl";
         this.richEditControl.Size = new System.Drawing.Size(692, 306);
         this.richEditControl.TabIndex = 6;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemRichEdit,
            this.layoutControlItemFolder1,
            this.layoutControlItemFolder2,
            this.layoutItemButtonStop,
            this.layoutItemButtonStart,
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.layoutItemNumberOfCurvesToDisplay,
            this.layoutItemIgnoreAddedCurves,
            this.layoutItemIgnoreRemovedCurves});
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(716, 486);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutControlItemRichEdit
         // 
         this.layoutControlItemRichEdit.Control = this.richEditControl;
         this.layoutControlItemRichEdit.Location = new System.Drawing.Point(0, 130);
         this.layoutControlItemRichEdit.Name = "layoutControlItemRichEdit";
         this.layoutControlItemRichEdit.Size = new System.Drawing.Size(696, 310);
         this.layoutControlItemRichEdit.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemRichEdit.TextVisible = false;
         // 
         // layoutControlItemFolder1
         // 
         this.layoutControlItemFolder1.Control = this.buttonEditFolder1;
         this.layoutControlItemFolder1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemFolder1.Name = "layoutControlItemFolder1";
         this.layoutControlItemFolder1.Size = new System.Drawing.Size(696, 24);
         this.layoutControlItemFolder1.TextLocation = DevExpress.Utils.Locations.Left;
         this.layoutControlItemFolder1.TextSize = new System.Drawing.Size(181, 13);
         // 
         // layoutControlItemFolder2
         // 
         this.layoutControlItemFolder2.Control = this.buttonEditFolder2;
         this.layoutControlItemFolder2.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemFolder2.Name = "layoutControlItemFolder2";
         this.layoutControlItemFolder2.Size = new System.Drawing.Size(696, 24);
         this.layoutControlItemFolder2.TextLocation = DevExpress.Utils.Locations.Left;
         this.layoutControlItemFolder2.TextSize = new System.Drawing.Size(181, 13);
         // 
         // layoutItemButtonStop
         // 
         this.layoutItemButtonStop.Control = this.stopButton;
         this.layoutItemButtonStop.Location = new System.Drawing.Point(172, 440);
         this.layoutItemButtonStop.Name = "layoutItemButtonStop";
         this.layoutItemButtonStop.Size = new System.Drawing.Size(250, 26);
         this.layoutItemButtonStop.TextSize = new System.Drawing.Size(181, 13);
         // 
         // layoutItemButtonStart
         // 
         this.layoutItemButtonStart.Control = this.startButton;
         this.layoutItemButtonStart.Location = new System.Drawing.Point(422, 440);
         this.layoutItemButtonStart.Name = "layoutItemButtonStart";
         this.layoutItemButtonStart.Size = new System.Drawing.Size(274, 26);
         this.layoutItemButtonStart.TextSize = new System.Drawing.Size(181, 13);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 440);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(172, 26);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 120);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(696, 10);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutItemNumberOfCurvesToDisplay
         // 
         this.layoutItemNumberOfCurvesToDisplay.Control = this.textEditNumberOfCurves;
         this.layoutItemNumberOfCurvesToDisplay.Location = new System.Drawing.Point(0, 48);
         this.layoutItemNumberOfCurvesToDisplay.Name = "layoutItemNumberOfCurvesToDisplay";
         this.layoutItemNumberOfCurvesToDisplay.Size = new System.Drawing.Size(696, 24);
         this.layoutItemNumberOfCurvesToDisplay.TextSize = new System.Drawing.Size(181, 13);
         // 
         // chkIgnoreAddedCurves
         // 
         this.chkIgnoreAddedCurves.Location = new System.Drawing.Point(12, 84);
         this.chkIgnoreAddedCurves.Name = "chkIgnoreAddedCurves";
         this.chkIgnoreAddedCurves.Properties.Caption = "chkIgnoreAddedCurves";
         this.chkIgnoreAddedCurves.Size = new System.Drawing.Size(692, 20);
         this.chkIgnoreAddedCurves.StyleController = this.layoutControl;
         this.chkIgnoreAddedCurves.TabIndex = 12;
         // 
         // layoutItemIgnoreAddedCurves
         // 
         this.layoutItemIgnoreAddedCurves.Control = this.chkIgnoreAddedCurves;
         this.layoutItemIgnoreAddedCurves.Location = new System.Drawing.Point(0, 72);
         this.layoutItemIgnoreAddedCurves.Name = "layoutItemIgnoreAddedCurves";
         this.layoutItemIgnoreAddedCurves.Size = new System.Drawing.Size(696, 24);
         this.layoutItemIgnoreAddedCurves.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemIgnoreAddedCurves.TextVisible = false;
         // 
         // chkIgnoreRemovedCurves
         // 
         this.chkIgnoreRemovedCurves.Location = new System.Drawing.Point(12, 108);
         this.chkIgnoreRemovedCurves.Name = "chkIgnoreRemovedCurves";
         this.chkIgnoreRemovedCurves.Properties.Caption = "chkIgnoreRemovedCurves";
         this.chkIgnoreRemovedCurves.Size = new System.Drawing.Size(692, 20);
         this.chkIgnoreRemovedCurves.StyleController = this.layoutControl;
         this.chkIgnoreRemovedCurves.TabIndex = 13;
         // 
         // layoutItemIgnoreRemovedCurves
         // 
         this.layoutItemIgnoreRemovedCurves.Control = this.chkIgnoreRemovedCurves;
         this.layoutItemIgnoreRemovedCurves.Location = new System.Drawing.Point(0, 96);
         this.layoutItemIgnoreRemovedCurves.Name = "layoutItemIgnoreRemovedCurves";
         this.layoutItemIgnoreRemovedCurves.Size = new System.Drawing.Size(696, 24);
         this.layoutItemIgnoreRemovedCurves.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemIgnoreRemovedCurves.TextVisible = false;
         // 
         // SimulationComparisonView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "SimulationComparisonView";
         this.ClientSize = new System.Drawing.Size(716, 486);
         this.Controls.Add(this.layoutControl);
         this.Name = "SimulationComparisonView";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "SimulationComparisonView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.textEditNumberOfCurves.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonEditFolder2.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonEditFolder1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemRichEdit)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFolder1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFolder2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonStop)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonStart)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNumberOfCurvesToDisplay)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkIgnoreAddedCurves.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIgnoreAddedCurves)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkIgnoreRemovedCurves.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIgnoreRemovedCurves)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraRichEdit.RichEditControl richEditControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemRichEdit;
      private DevExpress.XtraEditors.ButtonEdit buttonEditFolder2;
      private DevExpress.XtraEditors.ButtonEdit buttonEditFolder1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemFolder1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemFolder2;
      private DevExpress.XtraEditors.SimpleButton startButton;
      private DevExpress.XtraEditors.SimpleButton stopButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonStop;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonStart;
      private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraEditors.TextEdit textEditNumberOfCurves;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemNumberOfCurvesToDisplay;
      private DevExpress.XtraEditors.CheckEdit chkIgnoreRemovedCurves;
      private DevExpress.XtraEditors.CheckEdit chkIgnoreAddedCurves;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemIgnoreAddedCurves;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemIgnoreRemovedCurves;
   }
}

