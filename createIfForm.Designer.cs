namespace Script_Builder
{
    partial class createIfForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(createIfForm));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPreview = new System.Windows.Forms.TextBox();
            this.chkElse = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.warningA = new System.Windows.Forms.Label();
            this.cboCompareeA = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboCompareeB = new System.Windows.Forms.ComboBox();
            this.cboCompareOperator = new System.Windows.Forms.ComboBox();
            this.warningB = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.chkElse, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPreview);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // txtPreview
            // 
            this.txtPreview.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtPreview, "txtPreview");
            this.txtPreview.Name = "txtPreview";
            this.txtPreview.ReadOnly = true;
            // 
            // chkElse
            // 
            resources.ApplyResources(this.chkElse, "chkElse");
            this.chkElse.Name = "chkElse";
            this.chkElse.UseVisualStyleBackColor = true;
            this.chkElse.CheckedChanged += new System.EventHandler(this.comboBoxes_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel4);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.warningA, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.cboCompareeA, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.cboCompareeB, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.cboCompareOperator, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.warningB, 2, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // warningA
            // 
            resources.ApplyResources(this.warningA, "warningA");
            this.warningA.ForeColor = System.Drawing.Color.Red;
            this.warningA.Name = "warningA";
            // 
            // cboCompareeA
            // 
            resources.ApplyResources(this.cboCompareeA, "cboCompareeA");
            this.cboCompareeA.FormattingEnabled = true;
            this.cboCompareeA.Name = "cboCompareeA";
            this.cboCompareeA.TextChanged += new System.EventHandler(this.comboBoxes_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cboCompareeB
            // 
            resources.ApplyResources(this.cboCompareeB, "cboCompareeB");
            this.cboCompareeB.FormattingEnabled = true;
            this.cboCompareeB.Name = "cboCompareeB";
            this.cboCompareeB.TextChanged += new System.EventHandler(this.comboBoxes_SelectedIndexChanged);
            // 
            // cboCompareOperator
            // 
            resources.ApplyResources(this.cboCompareOperator, "cboCompareOperator");
            this.cboCompareOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCompareOperator.FormattingEnabled = true;
            this.cboCompareOperator.Items.AddRange(new object[] {
            resources.GetString("cboCompareOperator.Items"),
            resources.GetString("cboCompareOperator.Items1"),
            resources.GetString("cboCompareOperator.Items2"),
            resources.GetString("cboCompareOperator.Items3"),
            resources.GetString("cboCompareOperator.Items4"),
            resources.GetString("cboCompareOperator.Items5")});
            this.cboCompareOperator.Name = "cboCompareOperator";
            this.cboCompareOperator.SelectedIndexChanged += new System.EventHandler(this.comboBoxes_SelectedIndexChanged);
            // 
            // warningB
            // 
            resources.ApplyResources(this.warningB, "warningB");
            this.warningB.ForeColor = System.Drawing.Color.Red;
            this.warningB.Name = "warningB";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.buttonOK, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonCancel, 0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // createIfForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "createIfForm";
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboCompareOperator;
        private System.Windows.Forms.ComboBox cboCompareeA;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtPreview;
        private System.Windows.Forms.CheckBox chkElse;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label warningA;
        private System.Windows.Forms.Label warningB;
        private System.Windows.Forms.ComboBox cboCompareeB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    }
}