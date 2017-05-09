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
         this.defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
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
         this.SuspendLayout();
         // 
         // layoutControl
         // 
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
         // startButton
         // 
         this.startButton.Location = new System.Drawing.Point(502, 452);
         this.startButton.Name = "startButton";
         this.startButton.Size = new System.Drawing.Size(202, 22);
         this.startButton.StyleController = this.layoutControl;
         this.startButton.TabIndex = 10;
         this.startButton.Text = "startButton";
         // 
         // stopButton
         // 
         this.stopButton.Location = new System.Drawing.Point(310, 452);
         this.stopButton.Name = "stopButton";
         this.stopButton.Size = new System.Drawing.Size(62, 22);
         this.stopButton.StyleController = this.layoutControl;
         this.stopButton.TabIndex = 9;
         this.stopButton.Text = "stopButton";
         // 
         // buttonEditFolder2
         // 
         this.buttonEditFolder2.Location = new System.Drawing.Point(138, 36);
         this.buttonEditFolder2.Name = "buttonEditFolder2";
         this.buttonEditFolder2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.buttonEditFolder2.Size = new System.Drawing.Size(566, 20);
         this.buttonEditFolder2.StyleController = this.layoutControl;
         this.buttonEditFolder2.TabIndex = 8;
         // 
         // buttonEditFolder1
         // 
         this.buttonEditFolder1.Location = new System.Drawing.Point(138, 12);
         this.buttonEditFolder1.Name = "buttonEditFolder1";
         this.buttonEditFolder1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.buttonEditFolder1.Size = new System.Drawing.Size(566, 20);
         this.buttonEditFolder1.StyleController = this.layoutControl;
         this.buttonEditFolder1.TabIndex = 7;
         // 
         // richEditControl
         // 
         this.richEditControl.Location = new System.Drawing.Point(12, 70);
         this.richEditControl.Name = "richEditControl";
         this.richEditControl.Options.Export.Rtf.ExportTheme = true;
         this.richEditControl.Size = new System.Drawing.Size(692, 378);
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
            this.emptySpaceItem2});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(716, 486);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutControlItemRichEdit
         // 
         this.layoutControlItemRichEdit.Control = this.richEditControl;
         this.layoutControlItemRichEdit.Location = new System.Drawing.Point(0, 58);
         this.layoutControlItemRichEdit.Name = "layoutControlItemRichEdit";
         this.layoutControlItemRichEdit.Size = new System.Drawing.Size(696, 382);
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
         this.layoutControlItemFolder1.TextSize = new System.Drawing.Size(123, 13);
         // 
         // layoutControlItemFolder2
         // 
         this.layoutControlItemFolder2.Control = this.buttonEditFolder2;
         this.layoutControlItemFolder2.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemFolder2.Name = "layoutControlItemFolder2";
         this.layoutControlItemFolder2.Size = new System.Drawing.Size(696, 24);
         this.layoutControlItemFolder2.TextLocation = DevExpress.Utils.Locations.Left;
         this.layoutControlItemFolder2.TextSize = new System.Drawing.Size(123, 13);
         // 
         // layoutItemButtonStop
         // 
         this.layoutItemButtonStop.Control = this.stopButton;
         this.layoutItemButtonStop.Location = new System.Drawing.Point(172, 440);
         this.layoutItemButtonStop.Name = "layoutItemButtonStop";
         this.layoutItemButtonStop.Size = new System.Drawing.Size(192, 26);
         this.layoutItemButtonStop.TextSize = new System.Drawing.Size(123, 13);
         // 
         // layoutItemButtonStart
         // 
         this.layoutItemButtonStart.Control = this.startButton;
         this.layoutItemButtonStart.Location = new System.Drawing.Point(364, 440);
         this.layoutItemButtonStart.Name = "layoutItemButtonStart";
         this.layoutItemButtonStart.Size = new System.Drawing.Size(332, 26);
         this.layoutItemButtonStart.TextSize = new System.Drawing.Size(123, 13);
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
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 48);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(696, 10);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // SimulationComparisonView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "SimulationComparisonView";
         this.ClientSize = new System.Drawing.Size(716, 486);
         this.Controls.Add(this.layoutControl);
         this.Name = "SimulationComparisonView";
         this.Text = "SimulationComparisonView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
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
   }
}

