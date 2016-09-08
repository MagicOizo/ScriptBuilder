namespace Script_Builder
{
    partial class optionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(optionsForm));
            this.buttonOK = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panelBasic = new System.Windows.Forms.Panel();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.chkDebugging = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.buttonBrowseDoku = new System.Windows.Forms.Button();
            this.txtDokuPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVariableMarker = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelMakroKeys = new System.Windows.Forms.Panel();
            this.keyBox = new System.Windows.Forms.PictureBox();
            this.keyScroll = new System.Windows.Forms.VScrollBar();
            this.buttonRename = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.keyOptions = new System.Windows.Forms.GroupBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.comboTargetTextBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonKeyBrowse = new System.Windows.Forms.Button();
            this.txtKeyTarget = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonLöschen = new System.Windows.Forms.Button();
            this.buttonAb = new System.Windows.Forms.Button();
            this.buttonAuf = new System.Windows.Forms.Button();
            this.buttonHinzu = new System.Windows.Forms.Button();
            this.txtKeyName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panelBrowseDialogs = new System.Windows.Forms.Panel();
            this.chkSaveStartPath = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonBrowseStartPath = new System.Windows.Forms.Button();
            this.txtStartPath = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.radAlwaysSame = new System.Windows.Forms.RadioButton();
            this.radGroupsLast = new System.Windows.Forms.RadioButton();
            this.radAnybodysLast = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelBasic.SuspendLayout();
            this.panelMakroKeys.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.keyBox)).BeginInit();
            this.keyOptions.SuspendLayout();
            this.panelBrowseDialogs.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // treeView1
            // 
            resources.ApplyResources(this.treeView1, "treeView1");
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.Name = "treeView1";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView1.Nodes"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView1.Nodes1"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView1.Nodes2")))});
            this.treeView1.ShowLines = false;
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.ShowRootLines = false;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // panelBasic
            // 
            resources.ApplyResources(this.panelBasic, "panelBasic");
            this.panelBasic.Controls.Add(this.cboLanguage);
            this.panelBasic.Controls.Add(this.label18);
            this.panelBasic.Controls.Add(this.label17);
            this.panelBasic.Controls.Add(this.label16);
            this.panelBasic.Controls.Add(this.chkDebugging);
            this.panelBasic.Controls.Add(this.label15);
            this.panelBasic.Controls.Add(this.buttonBrowseDoku);
            this.panelBasic.Controls.Add(this.txtDokuPath);
            this.panelBasic.Controls.Add(this.label6);
            this.panelBasic.Controls.Add(this.label5);
            this.panelBasic.Controls.Add(this.label4);
            this.panelBasic.Controls.Add(this.label3);
            this.panelBasic.Controls.Add(this.label2);
            this.panelBasic.Controls.Add(this.txtVariableMarker);
            this.panelBasic.Controls.Add(this.label1);
            this.panelBasic.Name = "panelBasic";
            // 
            // cboLanguage
            // 
            this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguage.Items.AddRange(new object[] {
            resources.GetString("cboLanguage.Items"),
            resources.GetString("cboLanguage.Items1")});
            resources.ApplyResources(this.cboLanguage, "cboLanguage");
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Sorted = true;
            this.cboLanguage.SelectedIndexChanged += new System.EventHandler(this.txtDokuPath_TextChanged);
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // chkDebugging
            // 
            resources.ApplyResources(this.chkDebugging, "chkDebugging");
            this.chkDebugging.Name = "chkDebugging";
            this.chkDebugging.UseVisualStyleBackColor = true;
            this.chkDebugging.CheckedChanged += new System.EventHandler(this.chkDebugging_CheckedChanged);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // buttonBrowseDoku
            // 
            resources.ApplyResources(this.buttonBrowseDoku, "buttonBrowseDoku");
            this.buttonBrowseDoku.Name = "buttonBrowseDoku";
            this.buttonBrowseDoku.UseVisualStyleBackColor = true;
            this.buttonBrowseDoku.Click += new System.EventHandler(this.buttonBrowseDoku_Click);
            // 
            // txtDokuPath
            // 
            resources.ApplyResources(this.txtDokuPath, "txtDokuPath");
            this.txtDokuPath.Name = "txtDokuPath";
            this.txtDokuPath.TextChanged += new System.EventHandler(this.txtDokuPath_TextChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtVariableMarker
            // 
            resources.ApplyResources(this.txtVariableMarker, "txtVariableMarker");
            this.txtVariableMarker.Name = "txtVariableMarker";
            this.txtVariableMarker.TextChanged += new System.EventHandler(this.txtVariableMarker_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panelMakroKeys
            // 
            resources.ApplyResources(this.panelMakroKeys, "panelMakroKeys");
            this.panelMakroKeys.Controls.Add(this.keyBox);
            this.panelMakroKeys.Controls.Add(this.keyScroll);
            this.panelMakroKeys.Controls.Add(this.buttonRename);
            this.panelMakroKeys.Controls.Add(this.label8);
            this.panelMakroKeys.Controls.Add(this.keyOptions);
            this.panelMakroKeys.Controls.Add(this.buttonLöschen);
            this.panelMakroKeys.Controls.Add(this.buttonAb);
            this.panelMakroKeys.Controls.Add(this.buttonAuf);
            this.panelMakroKeys.Controls.Add(this.buttonHinzu);
            this.panelMakroKeys.Controls.Add(this.txtKeyName);
            this.panelMakroKeys.Controls.Add(this.label7);
            this.panelMakroKeys.Name = "panelMakroKeys";
            // 
            // keyBox
            // 
            this.keyBox.BackColor = System.Drawing.SystemColors.Window;
            this.keyBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.keyBox, "keyBox");
            this.keyBox.Name = "keyBox";
            this.keyBox.TabStop = false;
            this.keyBox.Paint += new System.Windows.Forms.PaintEventHandler(this.keyBox_Paint);
            this.keyBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.keyBox_MouseClick);
            this.keyBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.keyBox_PreviewKeyDown);
            // 
            // keyScroll
            // 
            resources.ApplyResources(this.keyScroll, "keyScroll");
            this.keyScroll.Name = "keyScroll";
            this.keyScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.keyScroll_Scroll);
            // 
            // buttonRename
            // 
            resources.ApplyResources(this.buttonRename, "buttonRename");
            this.buttonRename.Image = global::Script_Builder.Resource1.textfield_rename;
            this.buttonRename.Name = "buttonRename";
            this.buttonRename.UseVisualStyleBackColor = true;
            this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // keyOptions
            // 
            this.keyOptions.Controls.Add(this.buttonApply);
            this.keyOptions.Controls.Add(this.comboTargetTextBox);
            this.keyOptions.Controls.Add(this.label10);
            this.keyOptions.Controls.Add(this.buttonKeyBrowse);
            this.keyOptions.Controls.Add(this.txtKeyTarget);
            this.keyOptions.Controls.Add(this.label9);
            resources.ApplyResources(this.keyOptions, "keyOptions");
            this.keyOptions.Name = "keyOptions";
            this.keyOptions.TabStop = false;
            // 
            // buttonApply
            // 
            resources.ApplyResources(this.buttonApply, "buttonApply");
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // comboTargetTextBox
            // 
            this.comboTargetTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboTargetTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboTargetTextBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTargetTextBox.FormattingEnabled = true;
            resources.ApplyResources(this.comboTargetTextBox, "comboTargetTextBox");
            this.comboTargetTextBox.Name = "comboTargetTextBox";
            this.comboTargetTextBox.TextChanged += new System.EventHandler(this.comboTargetTextBox_TextChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // buttonKeyBrowse
            // 
            resources.ApplyResources(this.buttonKeyBrowse, "buttonKeyBrowse");
            this.buttonKeyBrowse.Name = "buttonKeyBrowse";
            this.buttonKeyBrowse.UseVisualStyleBackColor = true;
            this.buttonKeyBrowse.Click += new System.EventHandler(this.buttonKeyBrowse_Click);
            // 
            // txtKeyTarget
            // 
            resources.ApplyResources(this.txtKeyTarget, "txtKeyTarget");
            this.txtKeyTarget.Name = "txtKeyTarget";
            this.txtKeyTarget.TextChanged += new System.EventHandler(this.txtKeyTarget_TextChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // buttonLöschen
            // 
            resources.ApplyResources(this.buttonLöschen, "buttonLöschen");
            this.buttonLöschen.Image = global::Script_Builder.Resource1.cross;
            this.buttonLöschen.Name = "buttonLöschen";
            this.buttonLöschen.UseVisualStyleBackColor = true;
            this.buttonLöschen.Click += new System.EventHandler(this.buttonLöschen_Click);
            // 
            // buttonAb
            // 
            resources.ApplyResources(this.buttonAb, "buttonAb");
            this.buttonAb.Image = global::Script_Builder.Resource1.arrow_down2;
            this.buttonAb.Name = "buttonAb";
            this.buttonAb.UseVisualStyleBackColor = true;
            this.buttonAb.Click += new System.EventHandler(this.buttonAufAb_Click);
            // 
            // buttonAuf
            // 
            resources.ApplyResources(this.buttonAuf, "buttonAuf");
            this.buttonAuf.Image = global::Script_Builder.Resource1.arrow_up2;
            this.buttonAuf.Name = "buttonAuf";
            this.buttonAuf.UseVisualStyleBackColor = true;
            this.buttonAuf.Click += new System.EventHandler(this.buttonAufAb_Click);
            // 
            // buttonHinzu
            // 
            resources.ApplyResources(this.buttonHinzu, "buttonHinzu");
            this.buttonHinzu.Image = global::Script_Builder.Resource1.add;
            this.buttonHinzu.Name = "buttonHinzu";
            this.buttonHinzu.UseVisualStyleBackColor = true;
            this.buttonHinzu.Click += new System.EventHandler(this.buttonHinzu_Click);
            // 
            // txtKeyName
            // 
            resources.ApplyResources(this.txtKeyName, "txtKeyName");
            this.txtKeyName.Name = "txtKeyName";
            this.txtKeyName.TextChanged += new System.EventHandler(this.txtKeyName_TextChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // panelBrowseDialogs
            // 
            resources.ApplyResources(this.panelBrowseDialogs, "panelBrowseDialogs");
            this.panelBrowseDialogs.Controls.Add(this.chkSaveStartPath);
            this.panelBrowseDialogs.Controls.Add(this.groupBox1);
            this.panelBrowseDialogs.Controls.Add(this.label11);
            this.panelBrowseDialogs.Controls.Add(this.label14);
            this.panelBrowseDialogs.Name = "panelBrowseDialogs";
            // 
            // chkSaveStartPath
            // 
            resources.ApplyResources(this.chkSaveStartPath, "chkSaveStartPath");
            this.chkSaveStartPath.Name = "chkSaveStartPath";
            this.chkSaveStartPath.UseVisualStyleBackColor = true;
            this.chkSaveStartPath.CheckedChanged += new System.EventHandler(this.chkSaveStartPath_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonBrowseStartPath);
            this.groupBox1.Controls.Add(this.txtStartPath);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.radAlwaysSame);
            this.groupBox1.Controls.Add(this.radGroupsLast);
            this.groupBox1.Controls.Add(this.radAnybodysLast);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // buttonBrowseStartPath
            // 
            resources.ApplyResources(this.buttonBrowseStartPath, "buttonBrowseStartPath");
            this.buttonBrowseStartPath.Name = "buttonBrowseStartPath";
            this.buttonBrowseStartPath.UseVisualStyleBackColor = true;
            this.buttonBrowseStartPath.Click += new System.EventHandler(this.buttonBrowseStartPath_Click);
            // 
            // txtStartPath
            // 
            resources.ApplyResources(this.txtStartPath, "txtStartPath");
            this.txtStartPath.Name = "txtStartPath";
            this.txtStartPath.TextChanged += new System.EventHandler(this.txtStartPath_TextChanged);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // radAlwaysSame
            // 
            resources.ApplyResources(this.radAlwaysSame, "radAlwaysSame");
            this.radAlwaysSame.Name = "radAlwaysSame";
            this.radAlwaysSame.TabStop = true;
            this.radAlwaysSame.UseVisualStyleBackColor = true;
            this.radAlwaysSame.CheckedChanged += new System.EventHandler(this.radStartPath_CheckedChanged);
            // 
            // radGroupsLast
            // 
            resources.ApplyResources(this.radGroupsLast, "radGroupsLast");
            this.radGroupsLast.Name = "radGroupsLast";
            this.radGroupsLast.TabStop = true;
            this.radGroupsLast.UseVisualStyleBackColor = true;
            this.radGroupsLast.CheckedChanged += new System.EventHandler(this.radStartPath_CheckedChanged);
            // 
            // radAnybodysLast
            // 
            resources.ApplyResources(this.radAnybodysLast, "radAnybodysLast");
            this.radAnybodysLast.Name = "radAnybodysLast";
            this.radAnybodysLast.TabStop = true;
            this.radAnybodysLast.UseVisualStyleBackColor = true;
            this.radAnybodysLast.CheckedChanged += new System.EventHandler(this.radStartPath_CheckedChanged);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // optionsForm
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panelMakroKeys);
            this.Controls.Add(this.panelBasic);
            this.Controls.Add(this.panelBrowseDialogs);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "optionsForm";
            this.ShowInTaskbar = false;
            this.panelBasic.ResumeLayout(false);
            this.panelBasic.PerformLayout();
            this.panelMakroKeys.ResumeLayout(false);
            this.panelMakroKeys.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.keyBox)).EndInit();
            this.keyOptions.ResumeLayout(false);
            this.keyOptions.PerformLayout();
            this.panelBrowseDialogs.ResumeLayout(false);
            this.panelBrowseDialogs.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panelBasic;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtVariableMarker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonBrowseDoku;
        private System.Windows.Forms.TextBox txtDokuPath;
        private System.Windows.Forms.Panel panelMakroKeys;
        private System.Windows.Forms.Button buttonRename;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox keyOptions;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.ComboBox comboTargetTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonKeyBrowse;
        private System.Windows.Forms.TextBox txtKeyTarget;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonLöschen;
        private System.Windows.Forms.Button buttonAb;
        private System.Windows.Forms.Button buttonAuf;
        private System.Windows.Forms.Button buttonHinzu;
        private System.Windows.Forms.TextBox txtKeyName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panelBrowseDialogs;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radAlwaysSame;
        private System.Windows.Forms.RadioButton radGroupsLast;
        private System.Windows.Forms.RadioButton radAnybodysLast;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkSaveStartPath;
        private System.Windows.Forms.Button buttonBrowseStartPath;
        private System.Windows.Forms.TextBox txtStartPath;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox chkDebugging;
        private System.Windows.Forms.ComboBox cboLanguage;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.PictureBox keyBox;
        private System.Windows.Forms.VScrollBar keyScroll;

    }
}