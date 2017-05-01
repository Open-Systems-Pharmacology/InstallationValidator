namespace OSPSuite.InstallationValidator.Views
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
         this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
         this.labelValidationDescription = new DevExpress.XtraEditors.LabelControl();
         this.outputFolderButton = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemOutputButton = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemRichEdit = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.outputFolderButton.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemOutputButton)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemRichEdit)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(698, 12);
         this.btnCancel.Size = new System.Drawing.Size(146, 22);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(522, 12);
         this.btnOk.Size = new System.Drawing.Size(172, 22);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 538);
         this.layoutControlBase.Size = new System.Drawing.Size(856, 46);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Size = new System.Drawing.Size(251, 22);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(856, 46);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(510, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(176, 26);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(686, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(150, 26);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(255, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(255, 26);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(255, 26);
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.richEditControl);
         this.layoutControl.Controls.Add(this.labelValidationDescription);
         this.layoutControl.Controls.Add(this.outputFolderButton);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(856, 538);
         this.layoutControl.TabIndex = 38;
         this.layoutControl.Text = "layoutControl";
         // 
         // richEditControl
         // 
         this.richEditControl.Location = new System.Drawing.Point(12, 53);
         this.richEditControl.Name = "richEditControl";
         this.richEditControl.Options.Export.Rtf.ExportTheme = true;
         this.richEditControl.Size = new System.Drawing.Size(832, 473);
         this.richEditControl.TabIndex = 7;
         this.richEditControl.Text = "richEditControl1";
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
         this.outputFolderButton.Location = new System.Drawing.Point(168, 29);
         this.outputFolderButton.Name = "outputFolderButton";
         this.outputFolderButton.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.outputFolderButton.Size = new System.Drawing.Size(676, 20);
         this.outputFolderButton.StyleController = this.layoutControl;
         this.outputFolderButton.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemOutputButton,
            this.layoutControlItemDescription,
            this.layoutControlItemRichEdit});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(856, 538);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutControlItemOutputButton
         // 
         this.layoutControlItemOutputButton.Control = this.outputFolderButton;
         this.layoutControlItemOutputButton.Location = new System.Drawing.Point(0, 17);
         this.layoutControlItemOutputButton.Name = "layoutControlItemOutputButton";
         this.layoutControlItemOutputButton.Size = new System.Drawing.Size(836, 24);
         this.layoutControlItemOutputButton.TextSize = new System.Drawing.Size(153, 13);
         // 
         // layoutControlItemDescription
         // 
         this.layoutControlItemDescription.Control = this.labelValidationDescription;
         this.layoutControlItemDescription.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemDescription.Name = "layoutControlItemDescription";
         this.layoutControlItemDescription.Size = new System.Drawing.Size(836, 17);
         this.layoutControlItemDescription.TextSize = new System.Drawing.Size(153, 13);
         // 
         // layoutControlItemRichEdit
         // 
         this.layoutControlItemRichEdit.Control = this.richEditControl;
         this.layoutControlItemRichEdit.Location = new System.Drawing.Point(0, 41);
         this.layoutControlItemRichEdit.Name = "layoutControlItemRichEdit";
         this.layoutControlItemRichEdit.Size = new System.Drawing.Size(836, 477);
         this.layoutControlItemRichEdit.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemRichEdit.TextVisible = false;
         // 
         // MainView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "MainView";
         this.ClientSize = new System.Drawing.Size(856, 584);
         this.Controls.Add(this.layoutControl);
         this.Name = "MainView";
         this.Text = "MainView";
         this.Controls.SetChildIndex(this.layoutControlBase, 0);
         this.Controls.SetChildIndex(this.layoutControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.outputFolderButton.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemOutputButton)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemRichEdit)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.ButtonEdit outputFolderButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemOutputButton;
      private DevExpress.XtraEditors.LabelControl labelValidationDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemDescription;
      private DevExpress.XtraRichEdit.RichEditControl richEditControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemRichEdit;
   }
}