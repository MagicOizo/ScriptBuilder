namespace Script_Builder
{
    partial class importForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(importForm));
            this.importButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.importFile = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelDLS = new System.Windows.Forms.Label();
            this.labelExcel = new System.Windows.Forms.Label();
            this.radTab = new System.Windows.Forms.RadioButton();
            this.txtCustom = new System.Windows.Forms.TextBox();
            this.radCustom = new System.Windows.Forms.RadioButton();
            this.radPipe = new System.Windows.Forms.RadioButton();
            this.radSpace = new System.Windows.Forms.RadioButton();
            this.radSemikolon = new System.Windows.Forms.RadioButton();
            this.radKomma = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radUTF8 = new System.Windows.Forms.RadioButton();
            this.radUnicode = new System.Windows.Forms.RadioButton();
            this.radANSI = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radFile = new System.Windows.Forms.RadioButton();
            this.radClipboard = new System.Windows.Forms.RadioButton();
            this.firstRowHeaders = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // importButton
            // 
            resources.ApplyResources(this.importButton, "importButton");
            this.importButton.Name = "importButton";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // importFile
            // 
            resources.ApplyResources(this.importFile, "importFile");
            this.importFile.Name = "importFile";
            this.importFile.TextChanged += new System.EventHandler(this.importFile_TextChanged);
            // 
            // browseButton
            // 
            resources.ApplyResources(this.browseButton, "browseButton");
            this.browseButton.Name = "browseButton";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.labelDLS);
            this.groupBox1.Controls.Add(this.labelExcel);
            this.groupBox1.Controls.Add(this.radTab);
            this.groupBox1.Controls.Add(this.txtCustom);
            this.groupBox1.Controls.Add(this.radCustom);
            this.groupBox1.Controls.Add(this.radPipe);
            this.groupBox1.Controls.Add(this.radSpace);
            this.groupBox1.Controls.Add(this.radSemikolon);
            this.groupBox1.Controls.Add(this.radKomma);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // labelDLS
            // 
            resources.ApplyResources(this.labelDLS, "labelDLS");
            this.labelDLS.Name = "labelDLS";
            // 
            // labelExcel
            // 
            resources.ApplyResources(this.labelExcel, "labelExcel");
            this.labelExcel.Name = "labelExcel";
            // 
            // radTab
            // 
            resources.ApplyResources(this.radTab, "radTab");
            this.radTab.Name = "radTab";
            this.radTab.UseVisualStyleBackColor = true;
            // 
            // txtCustom
            // 
            resources.ApplyResources(this.txtCustom, "txtCustom");
            this.txtCustom.Name = "txtCustom";
            this.txtCustom.TextChanged += new System.EventHandler(this.txtCustom_TextChanged);
            // 
            // radCustom
            // 
            resources.ApplyResources(this.radCustom, "radCustom");
            this.radCustom.Name = "radCustom";
            this.radCustom.UseVisualStyleBackColor = true;
            this.radCustom.CheckedChanged += new System.EventHandler(this.radCustom_CheckedChanged);
            // 
            // radPipe
            // 
            resources.ApplyResources(this.radPipe, "radPipe");
            this.radPipe.Name = "radPipe";
            this.radPipe.UseVisualStyleBackColor = true;
            // 
            // radSpace
            // 
            resources.ApplyResources(this.radSpace, "radSpace");
            this.radSpace.Name = "radSpace";
            this.radSpace.UseVisualStyleBackColor = true;
            // 
            // radSemikolon
            // 
            resources.ApplyResources(this.radSemikolon, "radSemikolon");
            this.radSemikolon.Checked = true;
            this.radSemikolon.Name = "radSemikolon";
            this.radSemikolon.TabStop = true;
            this.radSemikolon.UseVisualStyleBackColor = true;
            // 
            // radKomma
            // 
            resources.ApplyResources(this.radKomma, "radKomma");
            this.radKomma.Name = "radKomma";
            this.radKomma.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.radUTF8);
            this.groupBox2.Controls.Add(this.radUnicode);
            this.groupBox2.Controls.Add(this.radANSI);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // radUTF8
            // 
            resources.ApplyResources(this.radUTF8, "radUTF8");
            this.radUTF8.Name = "radUTF8";
            this.radUTF8.UseVisualStyleBackColor = true;
            // 
            // radUnicode
            // 
            resources.ApplyResources(this.radUnicode, "radUnicode");
            this.radUnicode.Name = "radUnicode";
            this.radUnicode.UseVisualStyleBackColor = true;
            // 
            // radANSI
            // 
            resources.ApplyResources(this.radANSI, "radANSI");
            this.radANSI.Checked = true;
            this.radANSI.Name = "radANSI";
            this.radANSI.TabStop = true;
            this.radANSI.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.radFile);
            this.groupBox3.Controls.Add(this.radClipboard);
            this.groupBox3.Controls.Add(this.importFile);
            this.groupBox3.Controls.Add(this.browseButton);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // radFile
            // 
            resources.ApplyResources(this.radFile, "radFile");
            this.radFile.Checked = true;
            this.radFile.Name = "radFile";
            this.radFile.TabStop = true;
            this.radFile.UseVisualStyleBackColor = true;
            this.radFile.CheckedChanged += new System.EventHandler(this.radFile_CheckedChanged);
            // 
            // radClipboard
            // 
            resources.ApplyResources(this.radClipboard, "radClipboard");
            this.radClipboard.Name = "radClipboard";
            this.radClipboard.UseVisualStyleBackColor = true;
            this.radClipboard.CheckedChanged += new System.EventHandler(this.radClipboard_CheckedChanged);
            // 
            // firstRowHeaders
            // 
            resources.ApplyResources(this.firstRowHeaders, "firstRowHeaders");
            this.firstRowHeaders.Checked = true;
            this.firstRowHeaders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.firstRowHeaders.Name = "firstRowHeaders";
            this.firstRowHeaders.UseVisualStyleBackColor = true;
            // 
            // importForm
            // 
            this.AcceptButton = this.importButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.firstRowHeaders);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.importButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "importForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtCustom;
        private System.Windows.Forms.RadioButton radCustom;
        private System.Windows.Forms.RadioButton radPipe;
        private System.Windows.Forms.RadioButton radSpace;
        private System.Windows.Forms.RadioButton radSemikolon;
        private System.Windows.Forms.RadioButton radKomma;
        private System.Windows.Forms.RadioButton radTab;
        private System.Windows.Forms.TextBox importFile;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radANSI;
        private System.Windows.Forms.RadioButton radUTF8;
        private System.Windows.Forms.RadioButton radUnicode;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radFile;
        private System.Windows.Forms.RadioButton radClipboard;
        private System.Windows.Forms.Label labelDLS;
        private System.Windows.Forms.Label labelExcel;
        private System.Windows.Forms.CheckBox firstRowHeaders;
    }
}